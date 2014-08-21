using System;
using System.IO;
using ProtoBuf.Meta;
using ProtoBuf.Wcf.Channels.Contracts;
using ProtoBuf.Wcf.Channels.Exceptions;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Serialization
{
    public sealed class ProtoBufSerializer : ISerializer
    {
        #region ISerializer Members

        public SerializationResult Serialize(object obj)
        {
            return Serialize(obj, null);
        }

        public SerializationResult Serialize(object obj, TypeMetaData metaData)
        {
            if (obj == null)
                return null;

            var modelProvider = ObjectBuilder.GetModelProvider();

            if (modelProvider == null)
                throw new ConfigurationException("ModelProvider could not be resolved, please check configuration.");

            var info = metaData == null ?
                modelProvider.CreateModelInfo(obj.GetType()) :
                modelProvider.CreateModelInfo(obj.GetType(), metaData);

            var model = info.Model;

            var serializedData = Serialize(model, obj);

            return new SerializationResult(serializedData, info.MetaData);
        }

        public T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, null);
        }

        public T Deserialize<T>(byte[] data, TypeMetaData metaData)
        {
            if (data == null)
                return default(T);

            var modelProvider = ObjectBuilder.GetModelProvider();

            if (modelProvider == null)
                throw new ConfigurationException("ModelProvider could not be resolved, please check configuration.");

            var info = metaData == null ?
                modelProvider.CreateModelInfo(typeof(T)) :
                modelProvider.CreateModelInfo(typeof(T), metaData);

            var model = info.Model;

            var obj = Deserialize<T>(model, data);

            return obj;
        }

        #endregion

        #region Private Methods

        private T Deserialize<T>(TypeModel model, byte[] data)
        {
            try
            {
                using (var memStream = new MemoryStream(data))
                {
                    return (T)model.Deserialize(memStream, null, typeof(T));
                }
            }
            catch (Exception e)
            {
                throw new SerializationException(
                    "An error occurred while serialization, check inner exception for details.",
                    e);
            }
        }

        private byte[] Serialize(TypeModel model, object obj)
        {
            try
            {
                using (var memStream = new MemoryStream())
                {
                    model.Serialize(memStream, obj);

                    var data = memStream.ToArray();

                    return data;
                }
            }
            catch (Exception e)
            {
                throw new SerializationException(
                    "An error occurred while serialization, check inner exception for details.",
                    e);
            }
        }

        #endregion
    }
}