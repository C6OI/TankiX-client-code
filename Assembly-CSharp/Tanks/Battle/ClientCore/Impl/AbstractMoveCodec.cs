using System;
using System.Collections;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class AbstractMoveCodec : NotOptionalCodec {
        protected void CopyBits(byte[] buffer, BitArray bits) {
            for (int i = 0; i < buffer.Length; i++) {
                for (int j = 0; j < 8; j++) {
                    int index = i * 8 + j;
                    bool value = (buffer[i] & 1 << j) != 0;
                    bits.Set(index, value);
                }
            }
        }

        protected static float ReadFloat(BitArray bits, ref int position, int size, float factor) {
            float num = (Read(bits, ref position, size) - (1 << size - 1)) * factor;

            if (!PhysicsUtil.IsValidFloat(num)) {
                Debug.LogError("AbstractMoveCodec.ReadFloat: invalid float: " + num);
                return 0f;
            }

            return num;
        }

        protected static void WriteFloat(BitArray bits, ref int position, float value, int size, float factor) {
            int offset = 1 << size - 1;
            Write(bits, ref position, size, PrepareValue(value, offset, factor));
        }

        static int PrepareValue(float val, int offset, float factor) {
            int num = (int)(val / factor);
            int num2 = 0;

            if (num < -offset) {
                Debug.LogWarning(string.Format("Value too small {0} offset={1} factor={2}", val, offset, factor));
            } else {
                num2 = num - offset;
            }

            if (num2 >= offset) {
                Debug.LogWarning(string.Format("Value too big {0} offset={1} factor={2}", val, offset, factor));
                num2 = offset;
            }

            return num2;
        }

        static int Read(BitArray bits, ref int position, int bitsCount) {
            if (bitsCount > 32) {
                throw new Exception("Cannot read more that 32 bit at once (requested " + bitsCount + ")");
            }

            if (position + bitsCount > bits.Length) {
                throw new Exception("BitArea is out of data: requesed " + bitsCount + " bits, avaliable:" + (bits.Length - position));
            }

            int num = 0;

            for (int num2 = bitsCount - 1; num2 >= 0; num2--) {
                if (bits.Get(position)) {
                    num += 1 << num2;
                }

                position++;
            }

            return num;
        }

        static void Write(BitArray bits, ref int position, int bitsCount, int value) {
            if (bitsCount > 32) {
                throw new Exception("Cannot write more that 32 bit at once (requested " + bitsCount + ")");
            }

            if (position + bitsCount > bits.Length) {
                throw new Exception("BitArea overflow attempt to write " + bitsCount + " bits, space avaliable:" + (bits.Length - position));
            }

            for (int num = bitsCount - 1; num >= 0; num--) {
                bool value2 = (value & 1 << num) != 0;
                bits.Set(position, value2);
                position++;
            }
        }
    }
}