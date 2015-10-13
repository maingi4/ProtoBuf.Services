using System;
using System.IO;
using ProtoBuf.Meta;
using ProtoBuf.Services.Wcf.Contracts;
using ProtoBuf.Services.Wcf.Exceptions;
using ProtoBuf.Services.Wcf.Infrastructure;

namespace ProtoBuf.Services.Wcf.Serialization
{
    internal sealed class ProtoBufSerializer : ISerializer
    {
        #region ISerializer Members

        public SerializationResult Serialize(object obj)
        {
            try
            {
                return Serialize(obj, null);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Serialization failed, check inner exception for more details", ex);
            }
        }

        public SerializationResult Serialize(object obj, TypeMetaData metaData)
        {
            try
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
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Serialization failed, check inner exception for more details", ex);
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                return Deserialize<T>(data, null);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Serialization failed, check inner exception for more details", ex);
            }
        }

        public T Deserialize<T>(byte[] data, TypeMetaData metaData)
        {
            try
            {
                return (T)Deserialize(data, metaData, typeof(T));
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Serialization failed, check inner exception for more details", ex);
            }

        }

        public object Deserialize(byte[] data, TypeMetaData metaData, Type type)
        {
            try
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
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Serialization failed, check inner exception for more details", ex);
            }
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