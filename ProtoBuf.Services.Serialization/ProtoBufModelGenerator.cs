using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ProtoBuf.Meta;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.Serialization
{
    internal sealed class ProtoBufModelGenerator
    {
        #region Construction

        public ProtoBufModelGenerator(Type type)
        {
            _targetType = type;
            _typeMetaData = new TypeMetaData();
            _usePreDefinedFieldNumbers = false;
        }

        public ProtoBufModelGenerator(Type type, TypeMetaData typeMetaData)
        {
            _targetType = type;
            _typeMetaData = typeMetaData;
            _usePreDefinedFieldNumbers = true;
        }

        #endregion

        #region Configuration

        #region Fields

        private readonly Type _targetType;
        private readonly TypeMetaData _typeMetaData;
        private readonly bool _usePreDefinedFieldNumbers;

        private readonly ISet<string> _heirarchySet = new HashSet<string>();
        private readonly ISet<Type> _typeSet = new HashSet<Type>();

        #endregion

        public ModelInfo ConfigureType(Type type, bool recursive, bool configureChildren, ModeType appMode)
        {
            var method = this.GetType().GetMethod("ConfigureTypeInternal", BindingFlags.Instance | BindingFlags.NonPublic);
            var genericMethod = method.MakeGenericMethod(type);
            var info = genericMethod.Invoke(this, new object[] { true, true, appMode });

            return (ModelInfo)info;
        }

        private ModelInfo ConfigureTypeInternal<T>(bool recursive, bool configureChildren, ModeType appMode)
        {
            var targetType = typeof(T);

            var model = GetTypeModel(targetType);

            ConfigureType<T>(targetType, recursive, configureChildren, new HashSet<Type>(),
                model, targetType, appMode);

            return new ModelInfo(model, _typeMetaData);
        }

        private void ConfigureType<T>(Type type, bool recursive, bool configureChildren,
            ISet<Type> navigatedTypes, RuntimeTypeModel model, Type originalType, ModeType appMode)
        {
            var root = type == originalType;

            if (!IsValidType(type, appMode, root))
                return;

            if (type.IsArray)
            {
                type = type.GetElementType();

                if (root)
                    originalType = type;

                if (!IsValidType(type, appMode, root))
                    return;
            }

            model.Add(type, false);

            var baseTypes = GetRecursiveBase(type, appMode).ToArray();

            for (var i = 0; i < baseTypes.Length; i++)
            {
                var targetBaseType = baseTypes[i];

                if (IsValidType(targetBaseType, appMode))
                    model.Add(targetBaseType, false);

                if (appMode == ModeType.Wcf && IsValidType(targetBaseType, appMode) && targetBaseType.GetCustomAttribute<DataContractAttribute>() == null)
                    throw new InvalidOperationException("type does not have DataContract attribute: " + targetBaseType.AssemblyQualifiedName);

                var targetChild = i == 0 ? type : baseTypes[i - 1];

                ConfigureSubType(targetBaseType, targetChild, model, originalType, appMode);

                if (!recursive)
                    break;
            }

            ConfigureTypeOnModel(type, model, originalType, appMode);

            if (configureChildren && IsValidType(type, appMode, root))
            {
                var children = GetReferencedTypes(type, originalType, appMode).Concat(GetChildren(type, originalType, appMode)).Distinct().ToList();

                if (appMode == ModeType.Wcf && children.Any(x => IsValidType(x, appMode) && x.GetCustomAttribute<DataContractAttribute>() == null))
                {
                    throw new InvalidOperationException("type does not have DataContract attribute: " +
                        children.Where(x => IsValidType(x, appMode) && x.GetCustomAttribute<DataContractAttribute>() == null).Select(x => x.AssemblyQualifiedName)
                        .Aggregate((s, s1) => s + Environment.NewLine + s1));
                }

                foreach (var child in children)
                {
                    if (!navigatedTypes.Add(child))
                        continue;

                    ConfigureType<NullType>(child, recursive, true, navigatedTypes, model, originalType, appMode);
                }
            }

            if (typeof(T) != typeof(NullType))
                PrepareSerializer<T>(model);
        }

        private RuntimeTypeModel GetTypeModel(Type type)
        {
            var model = TypeModel.Create();
            model.AutoAddMissingTypes = true; //TODO: consider setting to false.
            model.UseImplicitZeroDefaults = true;
            model.AutoCompile = true;

            return model;
        }

        private class NullType
        { }

        private void ConfigureSubType(Type baseType, Type type, RuntimeTypeModel model, Type originalType, ModeType appMode)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (IsValidType(baseType, appMode) && _heirarchySet.Add(GetHashKey(originalType, type)))
            {
                model.Add(baseType, false).AddSubType(GetAndStoreBaseFieldNumber(baseType, GetChildren(baseType, originalType, appMode).ToArray(), type, appMode), type);
            }
        }

        private void ConfigureTypeOnModel(Type type, RuntimeTypeModel model, Type originalType, ModeType appMode)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!IsValidType(type, appMode) || !_typeSet.Add(type))
                return;

            var baseType = type.BaseType;

            ConfigureTypeOnModel(baseType, model, originalType, appMode);

            var fields = GetValidProperties(type, appMode).Concat(GetValidFields(type, appMode));

            var metaData = _typeMetaData;

            var fieldNumber = _usePreDefinedFieldNumbers ? 0 :
                metaData.GetMaxFieldNumber(GetTypeNameSpace(baseType, appMode), GetTypeName(baseType));

            foreach (var fieldInfo in fields)
            {
                var typeNamespace = GetTypeNameSpace(type, appMode);

                var typeName = GetTypeName(type);

                var fieldName = fieldInfo.Name;

                if (!_usePreDefinedFieldNumbers)
                {
                    fieldNumber++;
                    metaData.StoreFieldNumber(typeNamespace, typeName, fieldName, fieldNumber);
                }
                else
                {
                    int? tempFieldNumber;

                    if (!metaData.GetFieldNumber(typeNamespace,
                        typeName, fieldName, out tempFieldNumber))
                        continue;

                    fieldNumber = tempFieldNumber.Value;
                }

                var typeModel = model.Add(type, false).AddField(fieldNumber, fieldName);

                typeModel.OverwriteList = true;
            }
        }

        private string GetTypeNameSpace(Type type, ModeType appMode)
        {
            if (appMode != ModeType.Wcf)
                return string.Empty;

            var attribute = type.GetCustomAttribute<DataContractAttribute>();

            return attribute == null ? string.Empty :
                (attribute.Namespace ?? "http://schemas.datacontract.org/2004/07/" + type.Namespace);
        }

        private string GetTypeName(Type type)
        {
            return type.Name;
        }

        private string GetHashKey(Type type1, Type type2)
        {
            return type1.AssemblyQualifiedName + "-" + type2.AssemblyQualifiedName;
        }

        private readonly IDictionary<Type, SortedList<string, string>> _typeDic = new Dictionary<Type, SortedList<string, string>>();

        private int GetAndStoreBaseFieldNumber(Type baseType, Type[] subTypes, Type subType, ModeType appMode)
        {
            var typeNamespace = GetTypeNameSpace(subType, appMode);

            var typeName = GetTypeName(subType);

            int? number;

            if (_usePreDefinedFieldNumbers &&
                _typeMetaData.GetBaseNumber(typeNamespace, typeName, out number))
            {
                return number.Value;
            }

            SortedList<string, string> sortedList;

            var bases = GetRecursiveBase(subType, appMode).ToArray();

            var baseMost = subType;
            if (bases.Any())
                baseMost = bases.Last();

            if (_typeDic.ContainsKey(baseMost))
                sortedList = _typeDic[baseMost];
            else
            {
                sortedList = new SortedList<string, string>(subTypes.Length);

                foreach (var type in subTypes)
                {
                    sortedList.Add(type.FullName, type.FullName);
                }
                _typeDic[baseMost] = sortedList;
            }

            number = CalculateFieldNumber(Math.Abs(baseMost.FullName.GetHashCode()),
                                          sortedList.IndexOfValue(subType.FullName));

            _typeMetaData.StoreBaseNumber(typeNamespace, typeName, number.Value);

            return number.Value;
        }

        private int CalculateFieldNumber(int baseNumber, int child)
        {
            return checked(Normalize(baseNumber) + (child * 4) + 4);
        }

        private int Normalize(int number)
        {
            const int nFactor = 23827;

            return Math.Abs(number % nFactor);
        }

        private bool IsValidType(Type type, ModeType appMode, bool root = false)
        {
            if (root)
            {
                return type != null && type != typeof(object) && type != typeof(ValueType)
                       && type.Namespace != null && !type.IsPrimitive && type != typeof(string);
            }

            return type != null && type != typeof(object) && type != typeof(ValueType)
                         && type.Namespace != null
                         && type.IsArray == false
                         && type.IsPrimitive == false
                         && (type.Namespace != "System" && !type.Namespace.StartsWith("System."))
                         && type.GetCustomAttribute<ProtoIgnoreAttribute>() == null
                         && (appMode != ModeType.Wcf || type.GetCustomAttribute<DataContractAttribute>() != null);
        }

        private void PrepareSerializer<T>(RuntimeTypeModel model)
        {
            model.CompileInPlace();

            try
            {
                if (_targetType != typeof(TypeMetaData))
                    model.Compile();
            }
            catch (InvalidOperationException ex)
            {
                throw new SerializationException(
                   string.Format(
                   "The model {0} could not be serialized, this could be because private members in the type (or down its graph) are decorated with the 'DataMember attribute', check inner exception for more details.",
                   _targetType.FullName), ex);
            }

        }

        private IEnumerable<Type> GetReferencedTypes(Type type, Type originalType, ModeType appMode)
        {
            var fields = GetValidFields(type, appMode).Concat(GetValidProperties(type, appMode));

            var baseTypes = GetRecursiveBase(type, appMode);

            foreach (var baseType in baseTypes)
            {
                fields = fields.Concat(GetValidFields(baseType, appMode)).Concat(GetValidProperties(baseType, appMode));
            }

            return fields
                .Where(x => IsFieldTypeValid(x, originalType, appMode))
                .SelectMany(x => GetDetailedTypes(GetTypeFromMemberInfo(x)))
                .Concat(GetDetailedTypes(type))
                .Distinct();
        }

        private bool IsFieldTypeValid(MemberInfo memberInfo, Type originalType, ModeType appMode)
        {
            var memberType = GetTypeFromMemberInfo(memberInfo);

            if (!(memberInfo.GetCustomAttribute<ProtoIgnoreAttribute>() == null &&
                   memberType != typeof(object) &&
                   (appMode != ModeType.Wcf || memberInfo.GetCustomAttribute<DataMemberAttribute>() != null
                    || memberInfo.GetCustomAttribute<EnumMemberAttribute>() != null)))
                return false;

            if (appMode != ModeType.Wcf && originalType != typeof(TypeMetaData))
            {
                var propertyType = memberInfo as PropertyInfo;

                if (propertyType != null)
                {
                    if (!propertyType.GetGetMethod().IsPublic)
                    {
                        return false;
                    }
                }

                var fieldType = memberInfo as FieldInfo;

                if (fieldType != null)
                {
                    if (!fieldType.IsPublic)
                    {
                        return false;
                    }
                }
            }

            var types = GetDetailedTypes(memberInfo.GetType());

            foreach (var type in types)
            {
                if (type == typeof(object))
                {
                    return false;
                }
            }

            return true;
        }

        private Type GetTypeFromMemberInfo(MemberInfo memberInfo)
        {
            var propertyType = memberInfo as PropertyInfo;

            if (propertyType != null)
                return propertyType.PropertyType;

            var fieldType = memberInfo as FieldInfo;

            if (fieldType != null)
                return fieldType.FieldType;

            throw new ArgumentOutOfRangeException("memberInfo", "memberinfo was of unexpected type - " + memberInfo.GetType());
        }

        private static IEnumerable<MemberInfo> GetValidFields(Type type, ModeType appMode)
        {
            if (type.IsEnum)
            {
                return type
                    .GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(x => appMode != ModeType.Wcf || x.GetCustomAttribute<EnumMemberAttribute>() != null);
            }

            var flags = BindingFlags.Instance | BindingFlags.Public;

            if (type == typeof(TypeMetaData))
                flags = flags | BindingFlags.NonPublic;

            return type
                .GetFields(flags)
                .Where(x => appMode != ModeType.Wcf || x.GetCustomAttribute<DataMemberAttribute>() != null);
        }

        private static IEnumerable<MemberInfo> GetValidProperties(Type type, ModeType appMode)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => appMode != ModeType.Wcf || x.GetCustomAttribute<DataMemberAttribute>() != null);
        }

        private IEnumerable<Type> GetDetailedTypes(Type type)
        {
            return TypeFinder.GetDetailedTypes(type);
        }

        private IEnumerable<Type> GetChildren(Type baseType, Type originalType, ModeType appMode)
        {
            if (baseType == null)
                throw new ArgumentNullException("baseType");

            var targetAssemblies = GetTargetAssemblies(originalType);

            var targets = targetAssemblies.SelectMany(x =>
                {
                    try
                    {
                        return x.GetTypes();
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        return new Type[0];
                    }
                });

            var list = new List<Type>();

            foreach (var type in targets)
            {
                if (baseType == type)
                    continue;

                if (baseType.IsAssignableFrom(type))
                {
                    var parameterlessConstructor = type.GetConstructor(Type.EmptyTypes);

                    if (parameterlessConstructor != null)
                    {
                        list.AddRange(GetChildren(type, originalType, appMode));
                        list.Add(type);
                    }
                }
            }
            list.AddRange(GetRecursiveBase(baseType, appMode));

            return list.Distinct();
        }

        private IEnumerable<Assembly> GetTargetAssemblies(Type originalType)
        {
            var validAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !TypeFinder.AssemblyExclusions.Any(y => x.FullName.StartsWith(y)));

            return validAssemblies;
        }

        private IEnumerable<Type> GetRecursiveBase(Type type, ModeType appMode)
        {
            var lType = type;

            while (IsValidType(lType.BaseType, appMode))
            {
                yield return lType.BaseType;

                lType = lType.BaseType;
            }
        }

        #endregion
    }
}