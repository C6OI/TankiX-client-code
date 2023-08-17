using System;
using System.IO;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class OptionalMapCodecHelper {
        const byte INPLACE_MASK_1_BYTES = 32;

        const byte INPLACE_MASK_2_BYTES = 64;

        const byte INPLACE_MASK_3_BYTES = 96;

        const byte MASK_LENGTH_1_BYTE = 128;

        const int MASK_LENGTH_3_BYTE = 12582912;

        const byte INPLACE_MASK_FLAG = 128;

        const byte MASK_LENGTH_2_BYTES_FLAG = 64;

        public static void EncodeOptionalMap(IOptionalMap optionalMap, Stream dest) {
            int size = optionalMap.GetSize();
            byte[] map = ((OptionalMap)optionalMap).GetMap();

            if (size <= 5) {
                dest.WriteByte((byte)(map[0] >> 3));
                return;
            }

            if (size <= 13) {
                dest.WriteByte((byte)((map[0] >> 3) + 32));
                dest.WriteByte((byte)((map[1] >> 3) + (map[0] << 5)));
                return;
            }

            if (size <= 21) {
                dest.WriteByte((byte)((map[0] >> 3) + 64));
                dest.WriteByte((byte)((map[1] >> 3) + (map[0] << 5)));
                dest.WriteByte((byte)((map[2] >> 3) + (map[1] << 5)));
                return;
            }

            if (size <= 29) {
                dest.WriteByte((byte)((map[0] >> 3) + 96));
                dest.WriteByte((byte)((map[1] >> 3) + (map[0] << 5)));
                dest.WriteByte((byte)((map[2] >> 3) + (map[1] << 5)));
                dest.WriteByte((byte)((map[3] >> 3) + (map[2] << 5)));
                return;
            }

            if (size <= 504) {
                int num = (size >> 3) + (((uint)size & 7u) != 0 ? 1 : 0);
                byte b = (byte)((num & 0xFF) + 128);
                byte[] array = new byte[1 + num];
                array[0] = b;
                Array.Copy(map, 0, array, 1, num);
                dest.Write(array, 0, array.Length);
                return;
            }

            if (size <= 33554432) {
                int num2 = (size >> 3) + (((uint)size & 7u) != 0 ? 1 : 0);
                int num3 = num2 + 12582912;
                byte b2 = (byte)((num3 & 0xFF0000) >> 16);
                byte b3 = (byte)((num3 & 0xFF00) >> 8);
                byte b4 = (byte)((uint)num3 & 0xFFu);
                byte[] array2 = new byte[3 + num2];
                array2[0] = b2;
                array2[1] = b3;
                array2[2] = b4;
                Array.Copy(map, 0, array2, 3, num2);
                dest.Write(array2, 0, array2.Length);
                return;
            }

            throw new IndexOutOfRangeException("NullMap overflow");
        }

        public static void UnwrapOptionalMap(Stream packet, ProtocolBuffer dest) {
            BinaryReader binaryReader = new(packet);
            byte b = binaryReader.ReadByte();
            OptionalMap optionalMap = (OptionalMap)dest.OptionalMap;
            bool flag = (b & 0x80) != 0;
            byte[] map = optionalMap.GetMap();

            if (flag) {
                byte b2 = (byte)(b & 0x3Fu);
                int num;

                if ((b & 0x40u) != 0) {
                    byte b3 = binaryReader.ReadByte();
                    byte b4 = binaryReader.ReadByte();
                    num = (b2 << 16) + ((b3 & 0xFF) << 8) + (b4 & 0xFF);
                } else {
                    num = b2;
                }

                int size = num << 3;
                binaryReader.Read(map, 0, num);
                optionalMap.SetSize(size);
                return;
            }

            byte b5 = (byte)(b << 3);

            switch ((b & 0x60) >> 5) {
                case 0:
                    map[0] = b5;
                    optionalMap.SetSize(5);
                    break;

                case 1: {
                    byte b6 = binaryReader.ReadByte();
                    map[0] = (byte)(b5 + (b6 >> 5));
                    map[1] = (byte)(b6 << 3);
                    optionalMap.SetSize(13);
                    break;
                }

                case 2: {
                    byte b6 = binaryReader.ReadByte();
                    byte b7 = binaryReader.ReadByte();
                    map[0] = (byte)(b5 + (b6 >> 5));
                    map[1] = (byte)((b6 << 3) + (b7 >> 5));
                    map[2] = (byte)(b7 << 3);
                    optionalMap.SetSize(21);
                    break;
                }

                case 3: {
                    byte b6 = binaryReader.ReadByte();
                    byte b7 = binaryReader.ReadByte();
                    byte b8 = binaryReader.ReadByte();
                    map[0] = (byte)(b5 + (b6 >> 5));
                    map[1] = (byte)((b6 << 3) + (b7 >> 5));
                    map[2] = (byte)((b7 << 3) + (b8 >> 5));
                    map[3] = (byte)(b8 << 3);
                    optionalMap.SetSize(29);
                    break;
                }
            }
        }
    }
}