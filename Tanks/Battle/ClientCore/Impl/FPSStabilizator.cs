using System;

namespace Tanks.Battle.ClientCore.Impl {
    public interface FPSStabilizator {
        void Stabilize(Action onComplete);

        void Stabilize();

        void Stop();
    }
}