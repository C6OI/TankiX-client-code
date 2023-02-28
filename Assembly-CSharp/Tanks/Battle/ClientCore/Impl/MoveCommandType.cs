using System;

namespace Tanks.Battle.ClientCore.Impl {
    [Flags]
    public enum MoveCommandType {
        NONE = 0,
        TANK = 1,
        WEAPON = 2,
        FULL = 3
    }
}