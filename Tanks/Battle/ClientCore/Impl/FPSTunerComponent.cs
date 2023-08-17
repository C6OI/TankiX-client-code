using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [RequireComponent(typeof(FramesStatisticsCollectorBehaviour))]
    public class FPSTunerComponent : MonoBehaviour, Component {
        public int minFPS = 15;

        public int iterationTimeInSec = 10;

        public int upIterationShiftFPS = 3;

        FramesStatisticsCollectorBehaviour framesStatisticsCollector;

        public FPSStabilizator FPSStabilizator { get; private set; }

        public FPSUpper FPSUpper { get; private set; }

        void Awake() {
            framesStatisticsCollector = GetComponent<FramesStatisticsCollectorBehaviour>();
            FPSStabilizator = new IterativeFPSStabilizator(framesStatisticsCollector, minFPS, iterationTimeInSec);
            FPSUpper = new FPSUpperImpl(FPSStabilizator);
        }
    }
}