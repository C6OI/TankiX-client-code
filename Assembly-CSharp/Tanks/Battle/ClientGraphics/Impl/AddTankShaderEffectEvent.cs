namespace Tanks.Battle.ClientGraphics.Impl {
    public class AddTankShaderEffectEvent : BaseTankShaderEffectEvent {
        public AddTankShaderEffectEvent(string key, bool enableException = false)
            : base(key, enableException) { }
    }
}