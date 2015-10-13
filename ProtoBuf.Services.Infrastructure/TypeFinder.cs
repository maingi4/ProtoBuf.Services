using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ProtoBuf.Services.Infrastructure
{
    internal static class TypeFinder
    {
        internal static readonly string[] AssemblyExclusions = new[]
            {
                "mscorlib", 
                "System.",
                "System,",
                "Microsoft.",
                "protobuf-"
            };

        private static readonly ConcurrentDictionary<string, ICollection<TypeInfo>> TypeParamCache = new ConcurrentDictionary<string, ICollection<TypeInfo>>();
        private static readonly ConcurrentDictionary<string, Type> ServiceContractCache = new ConcurrentDictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, Type> PrimitiveTypes = new ConcurrentDictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, Type> DataContractCache = new ConcurrentDictionary<string, Type>();

        static TypeFinder()
        {
            var sysAssembly = 0.GetType().Assembly;

            var sysTypes = sysAssembly.GetTypes();

            foreach (var sysType in sysTypes)
            {
                if (sysType.IsPrimitive && sysType.IsPublic)
                    PrimitiveTypes.GetOrAdd(sysType.Name.ToLower(), sysType);
            }
        }

        private static string GetCacheKey(string val, bool param)
        {
            return string.Concat(val, "`", param ? 1 : 0);
        }

        private static Type ParsePrimitiveType(string name)
        {
            if (name.StartsWith("http"))
                return null;

            name = name.ToLower();

            if (name == "string")
                return typeof(string);

            Type retVal;

            return PrimitiveTypes.TryGetValue(name, out retVal) ? retVal : null;
        }

        private static IEnumerable<TypeInfo> GetTypeInfo(Type type, ParamType paramType, bool getDetailedTypes)
        {
            var attr = type.GetCustomAttribute<DataContractAttribute>();

            if (attr != null)
            {
                yield return new TypeInfo()
                {
                    Name =
                        (attr.Namespace ?? "http://schemas.datacontract.org/2004/07/" + type.Namespace).
                            TrimEnd(
                                '/')
                        + "/" + (attr.Name ?? type.Name),
                    Type = type,
                    ParamType = paramType
                };

            }
            else if (type.IsPrimitive || type == typeof(string) || type.IsArray || type.IsGenericType)
            {
                yield return new TypeInfo()
                {
                    Name = GetDetailedName(type),
                    Type = type,
                    ParamType = paramType
                };

            }
            else
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The type {0} does not have a data contract attribute and is not a primitive type.",
                        type.FullName));
            }
        }

        private static string GetDetailedName(Type type)
        {
            if (type.IsArray)
            {
                return type.Name;
            }

            if (type.IsGenericType)
            {
                var genParams = type.GenericTypeArguments;

                if (genParams.Length == 1)
                {
                    return GetDetailedName(genParams[0]) + "[]";
                }

                if (type.GetInterfaces().Any(x => x == typeof(IDictionary)))
                {
                    return "IDictionary`" + string.Join("`", genParams.Select(GetDetailedName));
                }
            }

            return type.Name;
        }

        public static ICollection<TypeInfo> GetContractParamTypes(Type serviceContractType,
            string operationContractName, string action, bool getDetailedTypes = true)
        {
            Func<Type, string, ICollection<TypeInfo>> paramGetter = (contractType, operationName) =>
                {
                    var retVal = new List<TypeInfo>();

                    var methods = contractType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                    foreach (var methodInfo in methods)
                    {
                        var attr = methodInfo.GetCustomAttribute<OperationContractAttribute>();

                        if (attr != null &&
                            (attr.Name ?? methodInfo.Name).Equals(operationName, StringComparison.Ordinal))
                        {
                            var inputParams = methodInfo.GetParameters();

                            foreach (var parameterInfo in inputParams)
                            {
                                retVal.AddRange(GetTypeInfo(parameterInfo.ParameterType, ParamType.Input, getDetailedTypes));
                            }

                            if (methodInfo.ReturnParameter != null && methodInfo.ReturnParameter.ParameterType != typeof(void))
                                retVal.AddRange(GetTypeInfo(methodInfo.ReturnParameter.ParameterType, ParamType.Return, getDetailedTypes));

                            break;
                        }
                    }
                    return retVal;
                };

            return TypeParamCache.GetOrAdd(GetCacheKey(action, getDetailedTypes), actionName => paramGetter(serviceContractType, operationContractName));
        }

        public static IEnumerable<Type> GetDetailedTypes(Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();

                foreach (var detailedType in GetDetailedTypes(elementType))
                {
                    yield return detailedType;
                }

                yield break;
            }

            if (type.IsGenericType)
            {
                foreach (var genericTypeArgument in type.GenericTypeArguments)
                {
                    foreach (var detailedType in GetDetailedTypes(genericTypeArgument))
                    {
                        yield return detailedType;
                    }
                }
                yield break;
            }

            yield return type;
        }

        public static Type FindServiceContract(string serviceContractNamespace)
        {
            Func<string, Type> contractGetter = contractNamespace =>
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (var assembly in assemblies)
                    {
                        if (AssemblyExclusions.Any(x => assembly.FullName.StartsWith(x, StringComparison.Ordinal)))
                            continue;

                        Type[] types;
                        try
                        {
                            types = assembly.GetTypes();
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            continue;
                        }

                        foreach (var type in types)
                        {
                            var attr = type.GetCustomAttribute<ServiceContractAttribute>();

                            if (attr != null)
                            {
                                var target = (attr.Namespace ?? "http://tempuri.org").TrimEnd('/') + "/" + type.Name;

                                if (contractNamespace.Equals(target, StringComparison.Ordinal))
                                {
                                    return type;
                                }
                            }
                        }
                    }
                    throw new KeyNotFoundException("Could not find service contract: " + contractNamespace);
                };
            return ServiceContractCache.GetOrAdd(serviceContractNamespace, contractGetter);
        }

        public static Type FindDataContract(string contractNamespace, string serviceContractNamespace, string action)
        {
            Func<string, string, string, Type> getter = (contractNs, serviceContractNs, actionName) =>
                {
                    var retVal = ParsePrimitiveType(contractNs);

                    if (retVal != null)
                        return retVal;

                    var serviceContract = FindServiceContract(serviceContractNs);

                    var methods = serviceContract.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var methodInfo in methods)
                    {
                        var attr = methodInfo.GetCustomAttribute<OperationContractAttribute>();

                        if (attr == null)
                            continue;

                        var operationName = (attr.Name ?? methodInfo.Name);

                        var contractParams = GetContractParamTypes(serviceContract, operationName, actionName);

                        foreach (var contractParam in contractParams)
                        {
                            if (contractParam.Name.Equals(contractNs))
                                return contractParam.Type;
                        }
                    }
                    throw new KeyNotFoundException("The following contract was not found: " + contractNs);
                };
            return DataContractCache.GetOrAdd(contractNamespace, s => getter(s, serviceContractNamespace, action));
        }
    }
}
