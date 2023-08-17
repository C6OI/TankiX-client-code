using System;
using System.Text;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class OptionalMap : IOptionalMap {
        const int SIZE_QUANT = 5000;

        int capacity;

        byte[] map;

        int readPosition;

        int size;

        public OptionalMap() => Fill(new byte[5000], 0, 40000);

        public OptionalMap(byte[] map, int size) => Fill(map, size);

        public void Fill(byte[] map, int size) {
            this.map = map;
            this.size = size;
            readPosition = 0;
            capacity = size << 3;
        }

        public void Clear() {
            Array.Clear(map, 0, map.Length);
            size = 0;
            readPosition = 0;
            capacity = 40000;
        }

        public void Add(bool isNull) {
            SetBit(size, isNull);
            size++;
        }

        public bool GetLast() => GetBit(--size);

        public void Concat(IOptionalMap otherMap) {
            int num = otherMap.GetSize();

            for (int i = 0; i < num; i++) {
                Add(((OptionalMap)otherMap).GetBit(i));
            }
        }

        public bool Get() {
            if (readPosition >= size) {
                throw new IndexOutOfRangeException();
            }

            bool bit = GetBit(readPosition);
            readPosition++;
            return bit;
        }

        public bool Has() => GetReadPosition() < GetSize();

        public int GetSize() => size;

        public IOptionalMap Duplicate() {
            OptionalMap optionalMap = new();
            optionalMap.capacity = capacity;
            optionalMap.readPosition = readPosition;
            optionalMap.size = size;
            Array.Copy(map, 0, optionalMap.map, 0, map.Length);
            return optionalMap;
        }

        public void Flip() => readPosition = 0;

        void Fill(byte[] map, int size, int capacity) {
            this.map = map;
            this.size = size;
            readPosition = 0;
            this.capacity = capacity;
        }

        public void SetSize(int size) => this.size = size;

        public void Reset() => readPosition = 0;

        public byte[] GetMap() => map;

        bool GetBit(int position) {
            int num = position >> 3;
            int num2 = 7 ^ position & 7;
            return (map[num] & 1 << num2) != 0;
        }

        void SetBit(int position, bool value) {
            int num = position >> 3;
            int num2 = 7 ^ position & 7;

            if (value) {
                map[num] = (byte)(map[num] | 1 << num2);
                return;
            }

            byte b = (byte)(0xFFu ^ (uint)(1 << num2));
            map[num] &= b;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new();
            stringBuilder.Append("optional map [ size:").Append(size).Append(" data:");

            for (int i = 0; i < size; i++) {
                if (GetBit(i)) {
                    stringBuilder.Append("1");
                } else {
                    stringBuilder.Append("0");
                }

                if (i == readPosition) {
                    stringBuilder.Append("<->");
                }
            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public int GetReadPosition() => readPosition;
    }
}