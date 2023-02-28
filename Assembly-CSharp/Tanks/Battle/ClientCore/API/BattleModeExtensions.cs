using System;

namespace Tanks.Battle.ClientCore.API {
    public static class BattleModeExtensions {
        public static Type GetBattleModeComponent(this BattleMode battleMode) {
            switch (battleMode) {
                case BattleMode.DM:
                    return typeof(DMComponent);

                case BattleMode.TDM:
                    return typeof(TDMComponent);

                case BattleMode.CTF:
                    return typeof(CTFComponent);

                case BattleMode.CP:
                    return typeof(CPComponent);

                default:
                    throw new Exception();
            }
        }
    }
}