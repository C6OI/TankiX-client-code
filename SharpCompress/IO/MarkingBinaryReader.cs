using System;
using System.IO;

namespace SharpCompress.IO {
    class MarkingBinaryReader : BinaryReader {
        public MarkingBinaryReader(Stream stream)
            : base(stream) { }

        public long CurrentReadByteCount { get; private set; }

        public void Mark() => CurrentReadByteCount = 0L;

        public override int Read() {
            CurrentReadByteCount += 4L;
            return base.Read();
        }

        public override int Read(byte[] buffer, int index, int count) {
            CurrentReadByteCount += count;
            return base.Read(buffer, index, count);
        }

        public override int Read(char[] buffer, int index, int count) => throw new NotImplementedException();

        public override bool ReadBoolean() {
            CurrentReadByteCount++;
            return base.ReadBoolean();
        }

        public override byte ReadByte() {
            CurrentReadByteCount++;
            return base.ReadByte();
        }

        public override byte[] ReadBytes(int count) {
            CurrentReadByteCount += count;
            return base.ReadBytes(count);
        }

        public override char ReadChar() => throw new NotImplementedException();

        public override char[] ReadChars(int count) => throw new NotImplementedException();

        public override decimal ReadDecimal() {
            CurrentReadByteCount += 16L;
            return base.ReadDecimal();
        }

        public override double ReadDouble() {
            CurrentReadByteCount += 8L;
            return base.ReadDouble();
        }

        public override short ReadInt16() {
            CurrentReadByteCount += 2L;
            return base.ReadInt16();
        }

        public override int ReadInt32() {
            CurrentReadByteCount += 4L;
            return base.ReadInt32();
        }

        public override long ReadInt64() {
            CurrentReadByteCount += 8L;
            return base.ReadInt64();
        }

        public override sbyte ReadSByte() {
            CurrentReadByteCount++;
            return base.ReadSByte();
        }

        public override float ReadSingle() {
            CurrentReadByteCount += 4L;
            return base.ReadSingle();
        }

        public override string ReadString() => throw new NotImplementedException();

        public override ushort ReadUInt16() {
            CurrentReadByteCount += 2L;
            return base.ReadUInt16();
        }

        public override uint ReadUInt32() {
            CurrentReadByteCount += 4L;
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64() {
            CurrentReadByteCount += 8L;
            return base.ReadUInt64();
        }
    }
}