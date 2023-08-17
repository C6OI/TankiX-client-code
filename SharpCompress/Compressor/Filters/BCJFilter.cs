using System.IO;

namespace SharpCompress.Compressor.Filters {
    public class BCJFilter : Filter {
        static readonly bool[] MASK_TO_ALLOWED_STATUS = new bool[8] { true, true, true, false, true, false, false, false };

        static readonly int[] MASK_TO_BIT_NUMBER = new int[8] { 0, 1, 2, 2, 3, 3, 3, 3 };

        int pos;

        int prevMask;

        public BCJFilter(bool isEncoder, Stream baseStream)
            : base(isEncoder, baseStream, 5) => pos = 5;

        static bool test86MSByte(byte b) => b == 0 || b == byte.MaxValue;

        protected override int Transform(byte[] buffer, int offset, int count) {
            int num = offset - 1;
            int num2 = offset + count - 5;
            int i;

            for (i = offset; i <= num2; i++) {
                if ((buffer[i] & 0xFE) != 232) {
                    continue;
                }

                num = i - num;

                if (((uint)num & 0xFFFFFFFCu) != 0) {
                    prevMask = 0;
                } else {
                    prevMask = prevMask << num - 1 & 7;

                    if (prevMask != 0 &&
                        (!MASK_TO_ALLOWED_STATUS[prevMask] || test86MSByte(buffer[i + 4 - MASK_TO_BIT_NUMBER[prevMask]]))) {
                        num = i;
                        prevMask = prevMask << 1 | 1;
                        continue;
                    }
                }

                num = i;

                if (test86MSByte(buffer[i + 4])) {
                    int num3 = buffer[i + 1] | buffer[i + 2] << 8 | buffer[i + 3] << 16 | buffer[i + 4] << 24;
                    int num4;

                    while (true) {
                        num4 = !isEncoder ? num3 - (pos + i - offset) : num3 + (pos + i - offset);

                        if (prevMask == 0) {
                            break;
                        }

                        int num5 = MASK_TO_BIT_NUMBER[prevMask] * 8;

                        if (!test86MSByte((byte)(num4 >> 24 - num5))) {
                            break;
                        }

                        num3 = num4 ^ (1 << 32 - num5) - 1;
                    }

                    buffer[i + 1] = (byte)num4;
                    buffer[i + 2] = (byte)(num4 >> 8);
                    buffer[i + 3] = (byte)(num4 >> 16);
                    buffer[i + 4] = (byte)~((num4 >> 24 & 1) - 1);
                    i += 4;
                } else {
                    prevMask = prevMask << 1 | 1;
                }
            }

            num = i - num;
            prevMask = (num & -4) == 0 ? prevMask << num - 1 : 0;
            i -= offset;
            pos += i;
            return i;
        }
    }
}