using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Infrastructure.Encryption;
using ProtoBuf.Services.Serialization;

namespace ProtoBuf.Services.WebAPI
{
    public class ProtoBufMediaTypeFormatter : MediaTypeFormatter
    {
        public ProtoBufMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(RestfulServiceConstants.ProtoContentType));
        }

        #region Reading / Deserialization

        public override bool CanReadType(Type type)
        {
            return CanReadWriteType(type);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, 
            System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, new CancellationToken());
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, 
            System.Net.Http.HttpContent content, IFormatterLogger formatterLogger, 
            CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => ReadFromStream(type, readStream, content, formatterLogger, cancellationToken));
        }

        private object ReadFromStream(Type type, Stream readStream, 
            System.Net.Http.HttpContent content, IFormatterLogger formatterLogger, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return null;

                var buffer = new byte[16*1024];
                byte[] data;
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = readStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return null;

                        ms.Write(buffer, 0, read);
                    }

                    data = ms.ToArray();
                }
                if (cancellationToken.IsCancellationRequested)
                    return null;

                var serializer = ObjectBuilder.GetSerializer();

                var deserialized = serializer.Deserialize(data, null, type);

                return deserialized;
            }
            catch(Exception ex)
            {
                if (formatterLogger != null)
                    formatterLogger.LogError(this.GetType().FullName, ex);

                throw;
            }
        }

        #endregion

        #region Writing / Serialization

        public override bool CanWriteType(Type type)
        {
            return CanReadWriteType(type);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, 
                                                System.Net.Http.HttpContent content, 
                                                System.Net.TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, new CancellationToken());
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, 
                                                System.Net.Http.HttpContent content, 
                                                System.Net.TransportContext transportContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => WriteToStream(type, value, writeStream, content, 
                                                             transportContext, cancellationToken));
        }

        private void WriteToStream(Type type, object value,
                                   Stream writeStream, System.Net.Http.HttpContent content,
                                   System.Net.TransportContext transportContext, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            using (var writer = new BinaryWriter(writeStream))
            {
                var serializer = ObjectBuilder.GetSerializer();

                var result = serializer.Serialize(value);

                if (cancellationToken.IsCancellationRequested)
                    return;

                if (result == null)
                    return;

                writer.Write(result.Data);
            }
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            headers.Add(RestfulServiceConstants.RqModelTypeHeaderKey, EncryptionManager.Encrypt(type.AssemblyQualifiedName));

            base.SetDefaultContentHeaders(type, headers, mediaType);
        }

        #endregion

        #region Helpers

        private static readonly ConcurrentDictionary<Type, bool> ValidTypes = new ConcurrentDictionary<Type, bool>();
        private static bool CanReadWriteType(Type type)
        {
            return ValidTypes.GetOrAdd(type, t => CanReadWriteTypeUnCached(type));
        }

        private static bool CanReadWriteTypeUnCached(Type type)
        {
            var modelGen = ObjectBuilder.GetModelProvider();

            try
            {
                modelGen.CreateModelInfo(type);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}