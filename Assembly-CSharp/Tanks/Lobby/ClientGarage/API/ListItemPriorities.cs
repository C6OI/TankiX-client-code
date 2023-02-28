namespace Tanks.Lobby.ClientGarage.API {
    public class ListItemPriorities {
        public byte MarketOrUser { get; set; }

        public byte MountedOrUnmounted { get; set; }

        public int StaticEffectValue { get; set; }

        public int StaticEffectWeaponType { get; set; }

        public byte StaticEffectType { get; set; }
    }
}