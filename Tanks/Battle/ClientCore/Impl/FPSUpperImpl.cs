using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FPSUpperImpl : FPSUpper {
        readonly FPSStabilizator stabilizator;

        int iterationShift;

        Action onComplete;

        int targetFrameRate;

        public FPSUpperImpl(FPSStabilizator stabilizator) => this.stabilizator = stabilizator;

        public void TryToUp(int iterationShift, Action onComplete) {
            Stop();
            this.onComplete = onComplete;
            IncreaseFPSAndStabilize(iterationShift);
        }

        public void TryToUp(int iterationShift) {
            Stop();
            IncreaseFPSAndStabilize(iterationShift);
        }

        public void Stop() {
            onComplete = null;
            stabilizator.Stop();
        }

        void IncreaseFPSAndStabilize(int iterationShift) {
            this.iterationShift = iterationShift;
            targetFrameRate = Mathf.Min(60, Application.targetFrameRate + iterationShift);
            FPSUtil.SetTargetFrameRate(targetFrameRate);
            stabilizator.Stabilize(OnStabilizationComplete);
        }

        void OnStabilizationComplete() {
            if (Application.targetFrameRate == targetFrameRate) {
                IncreaseFPSAndStabilize(iterationShift);
            } else if (onComplete != null) {
                Action action = onComplete;
                onComplete = null;
                action();
            }
        }
    }
}