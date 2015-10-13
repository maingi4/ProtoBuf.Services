using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Infrastructure.Exceptions;
using ProtoBuf.Services.Serialization;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public abstract class ProtoBufMessageFormatterBase
    {
        protected IList<TypeInfo> ParameterTypes { get; set; }
        protected ContractInfo ContractInfo { get; set; }
        protected CompressionTypeOptions DefaultCompressionType { get; set; }

        protected ProtoBufMessageFormatterBase(IList<TypeInfo> parameterTypes, string action,
            CompressionTypeOptions defaultCompressionType)
        {
            ParameterTypes = parameterTypes;
            DefaultCompressionType = defaultCompressionType;
            ContractInfo = ContractInfo.FromAction(action);
        }

        protected void DeserializeRequestInternal(Message message, object[] parameters)
        {
            var provider = ObjectBuilder.GetModelProvider();

            var serializer = ObjectBuilder.GetSerializer();

            var compressionType = GetMessageCompressionTypeOptions(message);

            CompressionProvider compressionProvider = null;
            if (compressionType != CompressionTypeOptions.None)
                compressionProvider = new CompressionProvider();

            var reader = message.GetReaderAtBodyContents();

            reader.Read();

            for (var i = 0; i < parameters.Length; i++)
            {
                var model = provider.CreateModelInfo(ParameterTypes[i].Type);

                reader.Read();

                var val = reader.Value;

                var data = BinaryConverter.FromString(val);

                if (compressionProvider != null)
                {
                    data = compressionProvider.DeCompress(data, compressionType);
                }

                object retVal;
                try
                {
                    retVal = serializer.Deserialize(data, model.MetaData, ParameterTypes[i].Type);
                }
                catch (SerializationException)
                {
                    throw FaultException.CreateFault(
                        MessageFault.CreateFault(
                        new FaultCode(Constants.SerializationFaultCode.ToString(CultureInfo.InvariantCulture)),
                                                 "Serialization failed, meta data removal is recommended."));
                }

                parameters[i] = retVal;
            }
        }

        protected Message SerializeReplyInternal(MessageVersion messageVersion, object[] parameters, object result)
        {
            var retParamInfo = ParameterTypes.FirstOrDefault(x => x.ParamType == ParamType.Return);

            //if (retParamInfo == null)
            //    throw new InvalidOperationException("The return parameter type was not found.");

            var compressionType = DefaultCompressionType;

            Func<string[]> valueGetter = () =>
                {
                    if (retParamInfo == null)
                        return new string[0];

                    var modelProvider = ObjectBuilder.GetModelProvider();

                    var model = modelProvider.CreateModelInfo(retParamInfo.Type);

                    var serializer = ObjectBuilder.GetSerializer();

                    var data = (serializer.Serialize(result, model.MetaData) ??
                               new SerializationResult(new byte[0], null)).Data;

                    if (compressionType != CompressionTypeOptions.None)
                    {
                        var compressionProvider = new CompressionProvider();

                        data = compressionProvider.Compress(data, compressionType);
                    }

                    var value = BinaryConverter.ToString(data);

                    return new[] { value };
                };

            var message = Message.CreateMessage(messageVersion, ContractInfo.Action + "Response",
                new ProtoBodyWriter(ContractInfo.OperationContractName, ContractInfo.ServiceNamespace,
                    valueGetter));

            if (compressionType != CompressionTypeOptions.None)
            {
                AddCompressionHeader(message, DefaultCompressionType);
            }

            return message;
        }

        protected Message SerializeRequestInternal(MessageVersion messageVersion, object[] parameters)
        {
            var compressionType = DefaultCompressionType;

            var parameterTypes = ParameterTypes;

            Func<string[]> valueGetter = () =>
            {
                var store = ObjectBuilder.GetModelStore();

                CompressionProvider compressionProvider = null;
                if (compressionType != CompressionTypeOptions.None)
                    compressionProvider = new CompressionProvider();

                var serializer = ObjectBuilder.GetSerializer();

                var retVal = new string[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];

                    var model = store.GetModel(parameterTypes[i].Type);

                    if (model == null)
                        throw new InvalidOperationException("The model cannot be null, meta data fetch failed. Type: " + parameterTypes[i].Type.FullName);

                    SerializationResult data;
                    try
                    {
                        data = serializer.Serialize(param, model.MetaData) ??
                               new SerializationResult(new byte[0], null);
                    }
                    catch (SerializationException)
                    {
                        if (param != null)
                            store.RemoveModel(param.GetType());

                        throw;
                    }

                    var byteData = data.Data;

                    if (compressionProvider != null)
                    {
                        byteData = compressionProvider.Compress(byteData, compressionType);
                    }

                    retVal[i] = BinaryConverter.ToString(byteData);
                }

                return retVal;
            };

            var message = Message.CreateMessage(messageVersion, ContractInfo.Action,
                                                    new ProtoBodyWriter(ContractInfo.OperationContractName,
                                                                        ContractInfo.ServiceNamespace,
                                                                        valueGetter));
            if (compressionType != CompressionTypeOptions.None)
            {
                AddCompressionHeader(message, DefaultCompressionType);
            }

            return message;
        }

        protected object DeserializeReplyInternal(Message message, object[] parameters)
        {
            var retParamInfo = ParameterTypes.FirstOrDefault(x => x.ParamType == ParamType.Return);

            if (retParamInfo == null)
                return null;

            var store = ObjectBuilder.GetModelStore();

            var serializer = ObjectBuilder.GetSerializer();
            
            var reader = message.GetReaderAtBodyContents();

            reader.Read();

            var model = store.GetModel(retParamInfo.Type);

            if (model == null)
                throw new InvalidOperationException("The model cannot be null, meta data fetch failed. Type: " + retParamInfo.Type.FullName);

            reader.Read();

            var val = reader.Value;

            var data = BinaryConverter.FromString(val);

            var compressionType = GetMessageCompressionTypeOptions(message);

            if (compressionType != CompressionTypeOptions.None)
            {
                var compressionProvider = new CompressionProvider();

                data = compressionProvider.DeCompress(data, compressionType);
            }

            object retVal;
            try
            {
                retVal = serializer.Deserialize(data, model.MetaData, retParamInfo.Type);
            }
            catch (SerializationException)
            {
                store.RemoveModel(retParamInfo.Type);

                throw;
            }
            return retVal;
        }

        private static CompressionTypeOptions GetMessageCompressionTypeOptions(Message message)
        {
            var headerLocation = message.Headers.FindHeader(Constants.CompressionHeaderKey,
                                                    Constants.DefaultCustomHeaderNamespace);

            if (headerLocation < 0)
                return CompressionTypeOptions.None;

            var compressionType = (CompressionTypeOptions)message.Headers.GetHeader<int>(headerLocation);

            return compressionType;
        }

        private static void AddCompressionHeader(Message message, CompressionTypeOptions compressionType)
        {
            message.Headers.Add(
                MessageHeader.CreateHeader(Constants.CompressionHeaderKey,
                Constants.DefaultCustomHeaderNamespace, (int)compressionType)
                );
        }
    }
}