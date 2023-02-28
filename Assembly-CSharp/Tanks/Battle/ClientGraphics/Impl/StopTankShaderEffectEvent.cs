namespace Tanks.Battle.ClientGraphics.Impl {
    public class StopTankShaderEffectEvent : BaseTankShaderEffectEvent {
        public StopTankShaderEffectEvent(string key, bool enableException = true)
            : base(key, enableException) { }
    }
}