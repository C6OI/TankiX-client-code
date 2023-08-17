using System;

namespace Tanks.Battle.ClientCore.Impl {
    public interface FPSUpper {
        void TryToUp(int iterationShift, Action onComplete);

        void TryToUp(int iterationShift);

        void Stop();
    }
}