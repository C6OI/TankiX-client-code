using System;

namespace Tanks.Battle.ClientCore.Impl {
    public class UnknownRegionTypeException : Exception {
        public UnknownRegionTypeException(BonusType bonusType)
            : base(bonusType.ToString()) { }
    }
}