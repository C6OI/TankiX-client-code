using System;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class CantFindAssetGuidException : Exception {
        public CantFindAssetGuidException(long marketItemId)
            : base(string.Format("marketItemId {0}", marketItemId)) { }
    }
}