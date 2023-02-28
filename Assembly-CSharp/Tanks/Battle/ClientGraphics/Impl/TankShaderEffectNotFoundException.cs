using System;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankShaderEffectNotFoundException : ArgumentException {
        public TankShaderEffectNotFoundException(string key)
            : base(string.Format("Key = [{0}]", key)) { }
    }
}