using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CalculateSplashCenterEvent : Event {
        public CalculateSplashCenterEvent() { }

        public CalculateSplashCenterEvent(SplashHitData splashHit) => SplashHit = splashHit;

        public SplashHitData SplashHit { get; set; }
    }
}