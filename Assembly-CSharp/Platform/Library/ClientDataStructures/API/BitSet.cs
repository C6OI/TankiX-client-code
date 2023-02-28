using System;

namespace Platform.Library.ClientDataStructures.API {
    public class BitSet : ICloneable {
        const int ADDRESS_BITS_PER_WORD = 6;

        long[] words;

        int wordsInUse;

        public BitSet() => words = new long[20];

        public object Clone() {
            BitSet bitSet = (BitSet)MemberwiseClone();
            bitSet.words = new long[words.Length];
            Array.Copy(words, bitSet.words, words.Length);
            return bitSet;
        }

        static int WordIndex(int bitIndex) => bitIndex >> 6;

        public void Set(int bitIndex) {
            int num = WordIndex(bitIndex);
            ExpandTo(num);
            words[num] |= 1L << bitIndex;
        }

        void ExpandTo(int wordIndex) {
            int num = wordIndex + 1;

            if (wordsInUse < num) {
                EnsureCapacity(num);
                wordsInUse = num;
            }
        }

        void EnsureCapacity(int wordsRequired) {
            if (words.Length < wordsRequired) {
                int newSize = Math.Max(2 * words.Length, wordsRequired);
                Array.Resize(ref words, newSize);
            }
        }

        public virtual void Clear(int bitIndex) {
            int num = WordIndex(bitIndex);

            if (num < wordsInUse) {
                words[num] &= ~(1L << bitIndex);
                RecalculateWordsInUse();
            }
        }

        void RecalculateWordsInUse() {
            int num = wordsInUse - 1;

            while (num >= 0 && words[num] == 0) {
                num--;
            }

            wordsInUse = num + 1;
        }

        public bool Get(int bitIndex) {
            int num = WordIndex(bitIndex);
            return num < wordsInUse && (words[num] & 1L << bitIndex) != 0;
        }

        public bool Mask(BitSet set) {
            if (wordsInUse < set.wordsInUse) {
                return false;
            }

            for (int num = Math.Min(wordsInUse, set.wordsInUse) - 1; num >= 0; num--) {
                if ((words[num] & set.words[num]) != set.words[num]) {
                    return false;
                }
            }

            return true;
        }

        public virtual bool MaskNot(BitSet set) {
            for (int num = Math.Min(wordsInUse, set.wordsInUse) - 1; num >= 0; num--) {
                if ((~words[num] & set.words[num]) != set.words[num]) {
                    return false;
                }
            }

            return true;
        }

        public void Clear() {
            while (wordsInUse > 0) {
                words[--wordsInUse] = 0L;
            }
        }

        public override int GetHashCode() {
            long num = 1234L;
            int num2 = wordsInUse;

            while (--num2 >= 0) {
                num ^= words[num2] * (num2 + 1);
            }

            return (int)(num >> 32 ^ num);
        }

        public override bool Equals(object obj) {
            if (obj.GetType() != typeof(BitSet)) {
                return false;
            }

            if (this == obj) {
                return true;
            }

            BitSet bitSet = (BitSet)obj;

            if (wordsInUse != bitSet.wordsInUse) {
                return false;
            }

            for (int i = 0; i < wordsInUse; i++) {
                if (words[i] != bitSet.words[i]) {
                    return false;
                }
            }

            return true;
        }
    }
}