using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class IterativeFPSStabilizator : FPSStabilizator {
        static readonly int STABILIZE_MAX_FPS = 55;

        static readonly int HI_FPS = 60;

        readonly FramesStatisticsCollector framesStatisticsCollector;

        readonly int iterationTimeInSec;

        readonly int minFPS;

        Action onComplete;

        int stableIterationCount;

        public IterativeFPSStabilizator(FramesStatisticsCollector framesStatisticsCollector, int minFPS,
            int iterationTimeInSec) {
            this.framesStatisticsCollector = framesStatisticsCollector;
            this.minFPS = minFPS;
            this.iterationTimeInSec = iterationTimeInSec;
        }

        public void Stabilize(Action onComplete) {
            Stop();
            this.onComplete = onComplete;
            CollectFramesStatistics();
        }

        public void Stabilize() {
            Stop();
            CollectFramesStatistics();
        }

        public void Stop() {
            framesStatisticsCollector.Stop();
            onComplete = null;
            stableIterationCount = 0;
        }

        void CollectFramesStatistics() {
            int maxFrameDurationInMs = Mathf.CeilToInt(1000f / minFPS) + 1;
            framesStatisticsCollector.Collect(iterationTimeInSec, maxFrameDurationInMs, OnFramesDataCollected);
        }

        void OnFramesDataCollected(StatisticCollection statisticCollection) {
            int num = CalculateTargetFrameRate(statisticCollection);

            if (num != Application.targetFrameRate) {
                FPSUtil.SetTargetFrameRate(num);
                stableIterationCount = 0;
            } else {
                stableIterationCount++;
            }

            if (stableIterationCount < 3) {
                CollectFramesStatistics();
            } else if (onComplete != null) {
                Action action = onComplete;
                onComplete = null;
                action();
            }
        }

        int CalculateTargetFrameRate(StatisticCollection statisticCollection) {
            int num;

            if (statisticCollection.StandartDeviation <= 5f) {
                num = Application.targetFrameRate;
            } else {
                float num2 = statisticCollection.Average + statisticCollection.StandartDeviation / 2f;
                num = (int)(1000f / num2);
            }

            num = num <= STABILIZE_MAX_FPS ? num : HI_FPS;
            return Mathf.Max(minFPS, num);
        }
    }
}