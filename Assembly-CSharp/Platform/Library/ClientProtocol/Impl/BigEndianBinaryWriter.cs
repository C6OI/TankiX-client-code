using System;
using System.IO;

namespace Platform.Library.ClientProtocol.Impl {
    public class BigEndianBinaryWriter : BinaryWriter {
        readonly byte[] numBuffer = new byte[8];

        public BigEndianBinaryWriter(Stream output)
            : base(output) { }

        void ReverseIfNeedAndWrite(byte[] bytes) {
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }

            Write(bytes, 0, bytes.Length);
        }

        public override void Write(double value) {
            ReverseIfNeedAndWrite(BitConverter.GetBytes(value));
        }

        public override void Write(float value) {
            ReverseIfNeedAndWrite(BitConverter.GetBytes(value));
        }

        public override void Write(short value) {
            Write((ushort)value);
        }

        public override void Write(int value) {
            Write((uint)value);
        }

        public override void Write(long value) {
            Write((ulong)value);
        }

        public override void Write(ushort value) {
            numBuffer[0] = (byte)(value >> 8);
            numBuffer[1] = (byte)value;
            Write(numBuffer, 0, 2);
        }

        public override void Write(uint value) {
            numBuffer[0] = (byte)(value >> 24);
            numBuffer[1] = (byte)(value >> 16);
            numBuffer[2] = (byte)(value >> 8);
            numBuffer[3] = (byte)value;
            Write(numBuffer, 0, 4);
        }

        public override void Write(ulong value) {
            numBuffer[0] = (byte)(value >> 56);
            numBuffer[1] = (byte)(value >> 48);
            numBuffer[2] = (byte)(value >> 40);
            numBuffer[3] = (byte)(value >> 32);
            numBuffer[4] = (byte)(value >> 24);
            numBuffer[5] = (byte)(value >> 16);
            numBuffer[6] = (byte)(value >> 8);
            numBuffer[7] = (byte)value;
            Write(numBuffer, 0, 8);
        }
    }
}