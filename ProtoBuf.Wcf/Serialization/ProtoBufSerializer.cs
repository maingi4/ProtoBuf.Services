using System;
using System.IO;
using ProtoBuf.Meta;
using ProtoBuf.Wcf.Channels.Contracts;
using ProtoBuf.Wcf.Channels.Exceptions;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Serialization
{
    internal sealed class ProtoBufSerializer : ISerializer
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
            return (T)Deserialize(data, metaData, typeof (T));
        }

        public object Deserialize(byte[] data, TypeMetaData metaData, Type type)
        {
            if (data == null || data.Length == 0)
                return null;

            var modelProvider = ObjectBuilder.GetModelProvider();

            if (modelProvider == null)
                throw new ConfigurationException("ModelProvider could not be resolved, please check configuration.");

            var info = metaData == null ?
                modelProvider.CreateModelInfo(type) :
                modelProvider.CreateModelInfo(type, metaData);

            var model = info.Model;

            var obj = Deserialize(model, data, type);

            return obj;
        }

        #endregion

        #region Private Methods

        private object Deserialize(TypeModel model, byte[] data, Type type)
        {
            try
            {
                using (var memStream = new MemoryStream(data))
                {
                    return (object)model.Deserialize(memStream, null, type);
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