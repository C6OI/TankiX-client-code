using System;
using System.IO;

namespace Platform.Library.ClientProtocol.Impl {
    public class LengthCodecHelper {
        public static void EncodeLength(Stream buf, int length) {
            if (length < 0) {
                throw new IndexOutOfRangeException("length=" + length);
            }

            if (length < 128) {
                buf.WriteByte((byte)((uint)length & 0x7Fu));
                return;
            }

            if (length < 16384) {
                long num = (length & 0x3FFF) + 32768;
                buf.WriteByte((byte)((num & 0xFF00) >> 8));
                buf.WriteByte((byte)(num & 0xFF));
                return;
            }

            if (length < 4194304) {
                long num2 = (length & 0x3FFFFF) + 12582912;
                buf.WriteByte((byte)((num2 & 0xFF0000) >> 16));
                buf.WriteByte((byte)((num2 & 0xFF00) >> 8));
                buf.WriteByte((byte)(num2 & 0xFF));
                return;
            }

            throw new IndexOutOfRangeException("length=" + length);
        }

        public static int DecodeLength(BinaryReader buf) {
            byte b = buf.ReadByte();

            if ((b & 0x80) == 0) {
                return b;
            }

            byte b2 = buf.ReadByte();

            if ((b & 0x40) == 0) {
                return ((b & 0x3F) << 8) + (b2 & 0xFF);
            }

            byte b3 = buf.ReadByte();
            return ((b & 0x3F) << 16) + ((b2 & 0xFF) << 8) + (b3 & 0xFF);
        }
    }
}