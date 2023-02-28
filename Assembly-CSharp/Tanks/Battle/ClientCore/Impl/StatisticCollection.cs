using System;

namespace Tanks.Battle.ClientCore.Impl {
    public class StatisticCollection {
        readonly int maxValue;

        float average = -1f;

        int moda = -1;

        float standardDeviation = -1f;

        int[] valueToCount;

        public StatisticCollection(int maxValue) {
            this.maxValue = maxValue;
            valueToCount = new int[maxValue];
        }

        public int Moda {
            get {
                if (moda != -1) {
                    return moda;
                }

                int num = 0;

                for (int i = 0; i < valueToCount.Length; i++) {
                    int num2 = i;
                    int num3 = valueToCount[i];

                    if (num3 > num) {
                        num = num3;
                        moda = num2;
                    }
                }

                return moda;
            }
        }

        public float Average {
            get {
                if (!average.Equals(-1f)) {
                    return average;
                }

                if (TotalCount == 0) {
                    return average;
                }

                int num = 0;

                for (int i = 0; i < valueToCount.Length; i++) {
                    int num2 = i;
                    int num3 = valueToCount[i];
                    num += num3 * num2;
                }

                average = num / (float)TotalCount;
                return average;
            }
        }

        public float StandartDeviation {
            get {
                if (!standardDeviation.Equals(-1f)) {
                    return standardDeviation;
                }

                if (TotalCount == 0) {
                    return standardDeviation;
                }

                float num = 0f;

                for (int i = 0; i < valueToCount.Length; i++) {
                    int num2 = i;
                    int num3 = valueToCount[i];
                    num += (num2 - Average) * (num2 - Average) * num3;
                }

                standardDeviation = (int)Math.Sqrt(num / TotalCount);
                return standardDeviation;
            }
        }

        public int TotalCount { get; private set; }

        public void Add(int value) {
            if (value >= maxValue) {
                value = maxValue - 1;
            }

            valueToCount[value]++;
            TotalCount++;
            SetDirty();
        }

        public void Add(int value, int count) {
            if (count > 0) {
                if (value >= maxValue) {
                    value = maxValue - 1;
                }

                valueToCount[value] += count;
                TotalCount += count;
                SetDirty();
            }
        }

        public StatisticCollection Clone() {
            StatisticCollection statisticCollection = new(maxValue);
            statisticCollection.valueToCount = new int[valueToCount.GetLength(0)];
            valueToCount.CopyTo(statisticCollection.valueToCount, 0);
            statisticCollection.moda = moda;
            statisticCollection.average = average;
            statisticCollection.standardDeviation = standardDeviation;
            statisticCollection.TotalCount = TotalCount;
            return statisticCollection;
        }

        void SetDirty() {
            moda = -1;
            average = -1f;
            standardDeviation = -1f;
        }
    }
}