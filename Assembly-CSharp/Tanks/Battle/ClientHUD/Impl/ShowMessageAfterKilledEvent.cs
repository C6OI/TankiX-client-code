using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ShowMessageAfterKilledEvent : Event {
        public long killerItem;
        public int killerRank;

        public TeamColor killerTeam;

        public string KillerUserUid { get; set; }
    }
}