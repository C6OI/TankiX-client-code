using System;
using System.IO;
using SharpCompress.Compressor.LZMA.LZ;

namespace SharpCompress.Compressor.LZMA {
    public class LzmaStream : Stream {
        readonly int dictionarySize;

        readonly Encoder encoder;

        readonly long inputSize;
        readonly Stream inputStream;

        readonly bool isLZMA2;

        readonly long outputSize;

        readonly OutWindow outWindow = new();

        readonly RangeCoder.Decoder rangeDecoder = new();
        long availableBytes;

        Decoder decoder;

        bool endReached;

        long inputPosition;

        bool needDictReset = true;

        bool needProps = true;

        long position;

        long rangeDecoderLimit;

        bool uncompressedChunk;

        public LzmaStream(byte[] properties, Stream inputStream)
            : this(properties, inputStream, -1L, -1L, null, properties.Length < 5) { }

        public LzmaStream(byte[] properties, Stream inputStream, long inputSize)
            : this(properties, inputStream, inputSize, -1L, null, properties.Length < 5) { }

        public LzmaStream(byte[] properties, Stream inputStream, long inputSize, long outputSize)
            : this(properties, inputStream, inputSize, outputSize, null, properties.Length < 5) { }

        public LzmaStream(byte[] properties, Stream inputStream, long inputSize, long outputSize, Stream presetDictionary,
            bool isLZMA2) {
            this.inputStream = inputStream;
            this.inputSize = inputSize;
            this.outputSize = outputSize;
            this.isLZMA2 = isLZMA2;

            if (!isLZMA2) {
                dictionarySize = BitConverter.ToInt32(properties, 1);
                outWindow.Create(dictionarySize);

                if (presetDictionary != null) {
                    outWindow.Train(presetDictionary);
                }

                rangeDecoder.Init(inputStream);
                decoder = new Decoder();
                decoder.SetDecoderProperties(properties);
                Properties = properties;
                availableBytes = outputSize >= 0 ? outputSize : long.MaxValue;
                rangeDecoderLimit = inputSize;
            } else {
                dictionarySize = 2 | properties[0] & 1;
                dictionarySize <<= (properties[0] >> 1) + 11;
                outWindow.Create(dictionarySize);

                if (presetDictionary != null) {
                    outWindow.Train(presetDictionary);
                    needDictReset = false;
                }

                Properties = new byte[1];
                availableBytes = 0L;
            }
        }

        public LzmaStream(LzmaEncoderProperties properties, bool isLZMA2, Stream outputStream)
            : this(properties, isLZMA2, null, outputStream) { }

        public LzmaStream(LzmaEncoderProperties properties, bool isLZMA2, Stream presetDictionary, Stream outputStream) {
            this.isLZMA2 = isLZMA2;
            availableBytes = 0L;
            endReached = true;

            if (isLZMA2) {
                throw new NotImplementedException();
            }

            encoder = new Encoder();
            encoder.SetCoderProperties(properties.propIDs, properties.properties);
            MemoryStream memoryStream = new(5);
            encoder.WriteCoderProperties(memoryStream);
            Properties = memoryStream.ToArray();
            encoder.SetStreams(null, outputStream, -1L, -1L);

            if (presetDictionary != null) {
                encoder.Train(presetDictionary);
            }
        }

        public override bool CanRead => encoder == null;

        public override bool CanSeek => false;

        public override bool CanWrite => encoder != null;

        public override long Length => position + availableBytes;

        public override long Position {
            get => position;
            set => throw new NotImplementedException();
        }

        public byte[] Properties { get; } = new byte[5];

        public override void Flush() { }

        protected override void Dispose(bool disposing) {
            if (disposing && encoder != null) {
                position = encoder.Code(null, true);
            }

            base.Dispose(disposing);
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (endReached) {
                return 0;
            }

            int num = 0;

            while (num < count) {
                if (availableBytes == 0L) {
                    if (isLZMA2) {
                        decodeChunkHeader();
                    } else {
                        endReached = true;
                    }

                    if (endReached) {
                        break;
                    }
                }

                int num2 = count - num;

                if (num2 > availableBytes) {
                    num2 = (int)availableBytes;
                }

                outWindow.SetLimit(num2);

                if (uncompressedChunk) {
                    inputPosition += outWindow.CopyStream(inputStream, num2);
                } else if (decoder.Code(dictionarySize, outWindow, rangeDecoder) && outputSize < 0) {
                    availableBytes = outWindow.AvailableBytes;
                }

                int num3 = outWindow.Read(buffer, offset, num2);
                num += num3;
                offset += num3;
                position += num3;
                availableBytes -= num3;

                if (availableBytes == 0L && !uncompressedChunk) {
                    rangeDecoder.ReleaseStream();

                    if (!rangeDecoder.IsFinished || rangeDecoderLimit >= 0 && rangeDecoder.Total != rangeDecoderLimit) {
                        throw new DataErrorException();
                    }

                    inputPosition += rangeDecoder.Total;

                    if (outWindow.HasPending) {
                        throw new DataErrorException();
                    }
                }
            }

            if (endReached) {
                if (inputSize >= 0 && inputPosition != inputSize) {
                    throw new DataErrorException();
                }

                if (outputSize >= 0 && position != outputSize) {
                    throw new DataErrorException();
                }
            }

            return num;
        }

        void decodeChunkHeader() {
            int num = inputStream.ReadByte();
            inputPosition++;

            if (num == 0) {
                endReached = true;
                return;
            }

            if (num >= 224 || num == 1) {
                needProps = true;
                needDictReset = false;
                outWindow.Reset();
            } else if (needDictReset) {
                throw new DataErrorException();
            }

            if (num >= 128) {
                uncompressedChunk = false;
                availableBytes = (num & 0x1F) << 16;
                availableBytes += (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2L;
                rangeDecoderLimit = (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2L;

                if (num >= 192) {
                    needProps = false;
                    Properties[0] = (byte)inputStream.ReadByte();
                    inputPosition++;
                    decoder = new Decoder();
                    decoder.SetDecoderProperties(Properties);
                } else {
                    if (needProps) {
                        throw new DataErrorException();
                    }

                    if (num >= 160) {
                        decoder = new Decoder();
                        decoder.SetDecoderProperties(Properties);
                    }
                }

                rangeDecoder.Init(inputStream);
            } else {
                if (num > 2) {
                    throw new DataErrorException();
                }

                uncompressedChunk = true;
                availableBytes = (inputStream.ReadByte() << 8) + inputStream.ReadByte() + 1;
                inputPosition += 2L;
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) {
            if (encoder != null) {
                position = encoder.Code(new MemoryStream(buffer, offset, count), false);
            }
        }
    }
}