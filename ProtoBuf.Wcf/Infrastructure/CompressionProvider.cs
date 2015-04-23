using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Infrastructure
{
    internal sealed class CompressionProvider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public byte[] Compress(byte[] data, CompressionTypeOptions compressionType)
        {
            if (compressionType == CompressionTypeOptions.None)
                return data;

            if (data == null || data.Length == 0)
                return new byte[] { };
            // create an empty resultant data stream

           try
            {
                using (var dataStream = new MemoryStream(data))
                {
                    var outputStream = new MemoryStream();
                    // Create the compression stream
                    using (var
                               compressionStream = CreateCompressionStream(compressionType, outputStream,
                                                                           CompressionMode.Compress))
                    {
                        CopyTo(dataStream, compressionStream);

                        compressionStream.Close();

                        return outputStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("An error during compression of an object. Check inner exception for further details.", ex);
            }
        }

        public byte[] DeCompress(byte[] data, CompressionTypeOptions compressionType)
        {
            try
            {
                if (compressionType == CompressionTypeOptions.None)
                    return data;

                if (data == null || data.Length == 0)
                    return new byte[] { };

                // create an resultant data stream
                var dataStream = new MemoryStream(data);

                // Create the compression stream
                using (var compressionStream = CreateCompressionStream(compressionType, dataStream,
                                                                       CompressionMode.Decompress))
                {
                    return ReadAllBytesFromStream(compressionStream);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("An error during decompression of an object. Check inner exception for further details.", ex);
            }
        }

        private Stream CreateCompressionStream(CompressionTypeOptions compressionType, Stream underlyingStream,
            CompressionMode mode)
        {
            switch (compressionType)
            {
                case CompressionTypeOptions.Deflate:
                    return new DeflateStream(underlyingStream, mode);
                case CompressionTypeOptions.Zip:
                default:
                    if (mode == CompressionMode.Compress)
                    {
                        return new GZipStream(underlyingStream, CompressionLevel.Fastest);
                    }

                    return new GZipStream(underlyingStream, mode);
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        private byte[] ReadAllBytesFromStream(Stream stream)
        {
            // Use this method is used to read all bytes from a stream.
            const int bufferSize = 1;
            var outStream = new MemoryStream();
            var buffer = new byte[bufferSize];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, bufferSize);
                if (bytesRead == 0)
                {
                    break;
                }
                outStream.Write(buffer, 0, bytesRead);
            }
            long length = outStream.Length;
            var result = new byte[length];
            outStream.Position = 0;
            outStream.Read(result, 0, (int)length);
            return result;
        }
    }

    public enum CompressionTypeOptions : int
    {
        None = 0,
        Zip = 1,
        Deflate = 2
    }
}
