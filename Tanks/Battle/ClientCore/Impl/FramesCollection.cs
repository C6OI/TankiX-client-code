using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FramesCollection {
        readonly StatisticCollection frames;
        readonly int maxFrameDurationInMs;

        readonly int measuringIntervalInSec;

        StatisticCollection intervalFrames;

        float intervalStartTime = float.NaN;

        int maxAverageForInterval = int.MinValue;

        int minAverageForInterval = int.MaxValue;

        public FramesCollection(int maxFrameDurationInMs, int measuringIntervalInSec) {
            this.maxFrameDurationInMs = maxFrameDurationInMs;
            this.measuringIntervalInSec = measuringIntervalInSec;
            frames = new StatisticCollection(maxFrameDurationInMs);
            intervalFrames = new StatisticCollection(maxFrameDurationInMs);
        }

        [Inject] public static UnityTime UnityTime { get; set; }

        public int Moda => frames.Moda;

        public int Average => (int)Mathf.Round(frames.Average);

        public int StandartDevation => (int)frames.StandartDeviation;

        public int HugeFrameCount { get; private set; }

        public int MinAverageForInterval {
            get {
                ProcessCurrentInterval();
                return minAverageForInterval;
            }
        }

        public int MaxAverageForInterval {
            get {
                ProcessCurrentInterval();
                return maxAverageForInterval;
            }
        }

        public void AddFrame(int durationInMs) {
            if (FrameIsHuge(durationInMs)) {
                HugeFrameCount++;
                return;
            }

            frames.Add(durationInMs);

            if (CurrentIntervalNotExist()) {
                StartNewInterval();
            }

            if (CurrentIntervalCompleted()) {
                ProcessCurrentInterval();
                StartNewInterval();
            }

            AddFrameToInterval(durationInMs);
        }

        void AddFrameToInterval(int durationInMs) => intervalFrames.Add(durationInMs);

        bool FrameIsHuge(int durationInMs) => durationInMs >= maxFrameDurationInMs;

        void ProcessCurrentInterval() {
            if (intervalFrames.TotalCount != 0) {
                if (intervalFrames.Average < minAverageForInterval) {
                    minAverageForInterval = (int)intervalFrames.Average;
                }

                if (intervalFrames.Average > maxAverageForInterval) {
                    maxAverageForInterval = (int)intervalFrames.Average;
                }
            }
        }

        bool CurrentIntervalCompleted() => UnityTime.realtimeSinceStartup - intervalStartTime >= measuringIntervalInSec;

        bool CurrentIntervalNotExist() => float.IsNaN(intervalStartTime);

        void StartNewInterval() {
            intervalStartTime = UnityTime.realtimeSinceStartup;
            intervalFrames = new StatisticCollection(maxFrameDurationInMs);
        }
    }
}