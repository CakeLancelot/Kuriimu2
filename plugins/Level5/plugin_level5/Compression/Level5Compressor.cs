﻿using System;
using System.Buffers.Binary;
using System.IO;
using Kontract.Kompression.Configuration;

namespace plugin_level5.Compression
{
    public static class Level5Compressor
    {
        public static int PeekDecompressedSize(Stream input)
        {
            var sizeMethodBuffer = new byte[4];
            input.Read(sizeMethodBuffer, 0, 4);
            input.Position -= 4;

            return (int)(BinaryPrimitives.ReadUInt32LittleEndian(sizeMethodBuffer) >> 3);
        }

        public static Level5CompressionMethod PeekCompressionMethod(Stream input)
        {
            var sizeMethodBuffer = new byte[4];
            input.Read(sizeMethodBuffer, 0, 4);
            input.Position -= 4;

            return (Level5CompressionMethod)(BinaryPrimitives.ReadUInt32LittleEndian(sizeMethodBuffer) & 0x7);
        }

        public static IKompressionConfiguration GetKompressionConfiguration(Level5CompressionMethod method)
        {
            switch (method)
            {
                case Level5CompressionMethod.NoCompression:
                    return null;

                case Level5CompressionMethod.Lz10:
                    return Kompression.Implementations.Compressions.Level5.Lz10;

                case Level5CompressionMethod.Huffman4Bit:
                    return Kompression.Implementations.Compressions.Level5.Huffman4Bit;

                case Level5CompressionMethod.Huffman8Bit:
                    return Kompression.Implementations.Compressions.Level5.Huffman8Bit;

                case Level5CompressionMethod.Rle:
                    return Kompression.Implementations.Compressions.Level5.Rle;

                default:
                    throw new NotSupportedException($"Unknown compression method {method}");
            }
        }

        public static void Decompress(Stream input, Stream output)
        {
            var method = PeekCompressionMethod(input);
            if (method == Level5CompressionMethod.NoCompression)
            {
                input.Position += 4;
                input.CopyTo(output);
                return;
            }

            var configuration = GetKompressionConfiguration(method);
            configuration.Build().Decompress(input, output);
        }

        public static void Compress(Stream input, Stream output, Level5CompressionMethod method)
        {
            IKompressionConfiguration configuration;
            switch (method)
            {
                case Level5CompressionMethod.NoCompression:
                    var compressionHeader = new[] {
                        (byte)(input.Length << 3),
                        (byte)(input.Length >> 5),
                        (byte)(input.Length >> 13),
                        (byte)(input.Length >> 21) };
                    output.Write(compressionHeader, 0, 4);

                    input.CopyTo(output);
                    return;

                case Level5CompressionMethod.Lz10:
                    configuration = Kompression.Implementations.Compressions.Level5.Lz10;
                    break;

                case Level5CompressionMethod.Huffman4Bit:
                    configuration = Kompression.Implementations.Compressions.Level5.Huffman4Bit;
                    break;

                case Level5CompressionMethod.Huffman8Bit:
                    configuration = Kompression.Implementations.Compressions.Level5.Huffman8Bit;
                    break;

                case Level5CompressionMethod.Rle:
                    configuration = Kompression.Implementations.Compressions.Level5.Rle;
                    break;

                default:
                    throw new NotSupportedException($"Unknown compression method {method}");
            }

            configuration.Build().Compress(input, output);
        }
    }
}
