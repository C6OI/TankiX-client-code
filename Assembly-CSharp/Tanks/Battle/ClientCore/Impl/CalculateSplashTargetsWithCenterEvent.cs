using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CalculateSplashTargetsWithCenterEvent : Event {
        public CalculateSplashTargetsWithCenterEvent() { }

        public CalculateSplashTargetsWithCenterEvent(SplashHitData splashHit) => SplashHit = splashHit;

        public SplashHitData SplashHit { get; set; }
    }
}