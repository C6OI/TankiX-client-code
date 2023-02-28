using System;
using System.IO;

namespace Platform.Library.ClientProtocol.Impl {
    public class BigEndianBinaryReader : BinaryReader {
        readonly byte[] numBuffer = new byte[8];

        public BigEndianBinaryReader(Stream input)
            : base(input) { }

        void FillNumBuffer(int numBytes) {
            int num;

            for (int i = 0; i < numBytes; i += num) {
                num = Read(numBuffer, i, numBytes - i);

                if (num == 0) {
                    throw new EndOfStreamException();
                }
            }
        }

        void Reverse(int numBytes) {
            for (int i = 0; i < numBytes / 2; i++) {
                byte b = numBuffer[i];
                int num = numBytes - i - 1;
                numBuffer[i] = numBuffer[num];
                numBuffer[num] = b;
            }
        }

        void FillBufferAndReverseIfNeed(int numBytes) {
            FillNumBuffer(numBytes);

            if (BitConverter.IsLittleEndian) {
                Reverse(numBytes);
            }
        }

        public override double ReadDouble() {
            FillBufferAndReverseIfNeed(8);
            return BitConverter.ToDouble(numBuffer, 0);
        }

        public override float ReadSingle() {
            FillBufferAndReverseIfNeed(4);
            return BitConverter.ToSingle(numBuffer, 0);
        }

        public override short ReadInt16() => (short)ReadUInt16();

        public override int ReadInt32() => (int)ReadUInt32();

        public override long ReadInt64() => (long)ReadUInt64();

        public override ushort ReadUInt16() {
            FillNumBuffer(2);
            return (ushort)(numBuffer[0] << 8 | numBuffer[1]);
        }

        public override uint ReadUInt32() {
            FillNumBuffer(4);
            return (uint)(numBuffer[0] << 24 | numBuffer[1] << 16 | numBuffer[2] << 8 | numBuffer[3]);
        }

        public override ulong ReadUInt64() {
            FillNumBuffer(8);
            uint num = (uint)(numBuffer[0] << 24 | numBuffer[1] << 16 | numBuffer[2] << 8 | numBuffer[3]);
            uint num2 = (uint)(numBuffer[4] << 24 | numBuffer[5] << 16 | numBuffer[6] << 8 | numBuffer[7]);
            return (ulong)num << 32 | num2;
        }
    }
}