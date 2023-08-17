namespace Tanks.Battle.ClientCore.Impl {
    public class CalculateSplashCenterByStaticHitEvent : CalculateSplashCenterEvent {
        public CalculateSplashCenterByStaticHitEvent() { }

        public CalculateSplashCenterByStaticHitEvent(SplashHitData splashHit)
            : base(splashHit) { }
    }
}