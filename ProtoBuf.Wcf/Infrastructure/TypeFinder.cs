using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Infrastructure
{
    public static class TypeFinder
    {
        private static readonly string[] AssemblyExclusions = new[]
            {
                "mscorlib", 
                "System.",
                "System,",
                "Microsoft.",
                "protobuf-"
            };

        private static readonly ConcurrentDictionary<string, ICollection<TypeInfo>> TypeParamCache = new ConcurrentDictionary<string, ICollection<TypeInfo>>();
        public static ICollection<TypeInfo> GetContractParamTypes(Type serviceContractType, string operationContractName, string action)
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
                                retVal.Add(GetTypeInfo(parameterInfo.ParameterType));
                            }

                            if (methodInfo.ReturnParameter != null)
                                retVal.Add(GetTypeInfo(methodInfo.ReturnParameter.ParameterType));

                            break;
                        }
                    }
                    return retVal;
                };

            return TypeParamCache.GetOrAdd(action, actionName => paramGetter(serviceContractType, operationContractName));
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            var attr = type.GetCustomAttribute<DataContractAttribute>();

            if (attr != null)
                return new TypeInfo()
                {
                    Name = (attr.Namespace ?? "http://tempuri.org").TrimEnd('/') + attr.Name,
                    Type = type
                };

            if (type.IsPrimitive || type == typeof(string))
                return new TypeInfo()
                {
                    Name = type.Name,
                    Type = type
                };

            throw new InvalidOperationException(string.Format("The type {0} does not have a data contract attribute and is not a primitive type.", type.FullName));
        }

        private static readonly ConcurrentDictionary<string, Type> ServiceContractCache = new ConcurrentDictionary<string, Type>();
        public static Type FindServiceContract(string serviceContractNamespace)
        {
            Func<string, Type> contractGetter = contractNamespace =>
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (var assembly in assemblies)
                    {
                        if (AssemblyExclusions.Any(x => assembly.FullName.StartsWith(x, StringComparison.Ordinal)))
                            continue;

                        var types = assembly.GetTypes();

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
    }
}
