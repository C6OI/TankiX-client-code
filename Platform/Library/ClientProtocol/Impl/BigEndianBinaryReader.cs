using System;
using System.IO;

namespace Platform.Library.ClientProtocol.Impl {
    public class BigEndianBinaryReader : BinaryReader {
        readonly byte[] numBuffer = new byte[8];

        public BigEndianBinaryReader(Stream input)
            : base(input) { }

        void fillNumBuffer(int numBytes) {
            int num;

            for (int i = 0; i < numBytes; i += num) {
                num = Read(numBuffer, i, numBytes - i);

                if (num == 0) {
                    throw new EndOfStreamException();
                }
            }
        }

        void fillBufferAndReverseIfNeed(int numBytes) {
            fillNumBuffer(numBytes);

            if (BitConverter.IsLittleEndian) {
                Array.Reverse(numBuffer, 0, numBytes);
            }
        }

        public override double ReadDouble() {
            fillBufferAndReverseIfNeed(8);
            return BitConverter.ToDouble(numBuffer, 0);
        }

        public override float ReadSingle() {
            fillBufferAndReverseIfNeed(4);
            return BitConverter.ToSingle(numBuffer, 0);
        }

        public override short ReadInt16() => (short)ReadUInt16();

        public override int ReadInt32() => (int)ReadUInt32();

        public override long ReadInt64() => (long)ReadUInt64();

        public override ushort ReadUInt16() {
            fillNumBuffer(2);
            return (ushort)(numBuffer[0] << 8 | numBuffer[1]);
        }

        public override uint ReadUInt32() {
            fillNumBuffer(4);
            return (uint)(numBuffer[0] << 24 | numBuffer[1] << 16 | numBuffer[2] << 8 | numBuffer[3]);
        }

        public override ulong ReadUInt64() {
            fillNumBuffer(8);
            uint num = (uint)(numBuffer[0] << 24 | numBuffer[1] << 16 | numBuffer[2] << 8 | numBuffer[3]);
            uint num2 = (uint)(numBuffer[4] << 24 | numBuffer[5] << 16 | numBuffer[6] << 8 | numBuffer[7]);
            return (ulong)num << 32 | num2;
        }
    }
}