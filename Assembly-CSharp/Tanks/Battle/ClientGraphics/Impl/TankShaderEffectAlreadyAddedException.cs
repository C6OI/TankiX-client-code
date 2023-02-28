using System;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankShaderEffectAlreadyAddedException : ArgumentException {
        public TankShaderEffectAlreadyAddedException(string key)
            : base(string.Format("Key = [{0}]", key)) { }
    }
}