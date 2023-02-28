using System;

namespace Tanks.Lobby.ClientGarage.API {
    public class GarageCategory {
        public static GarageCategory WEAPONS = new("weapons", typeof(WeaponItemComponent), false);

        public static GarageCategory HULLS = new("hulls", typeof(TankItemComponent), false);

        public static GarageCategory PAINTS = new("paints", typeof(PaintItemComponent), false);

        public static GarageCategory SHELLS = new("shells", typeof(ShellItemComponent), true);

        public static GarageCategory SKINS = new("skins", typeof(SkinItemComponent), true);

        public static GarageCategory GRAFFITI = new("graffiti", typeof(GraffitiItemComponent), false);

        public static GarageCategory BLUEPRINTS = new("blueprints", typeof(GameplayChestItemComponent), false);

        public static GarageCategory CONTAINERS = new("containers", typeof(ContainerMarkerComponent), false);

        public static GarageCategory MODULES = new("modules", typeof(ModuleItemComponent), true);

        public static GarageCategory[] Values = new GarageCategory[9] { WEAPONS, PAINTS, HULLS, SHELLS, SKINS, GRAFFITI, BLUEPRINTS, CONTAINERS, MODULES };

        public GarageCategory(string linkPart, Type itemMarkerComponentType, bool needParent) {
            LinkPart = linkPart;
            ItemMarkerComponentType = itemMarkerComponentType;
            NeedParent = needParent;
        }

        public string Name => LinkPart.ToUpper();

        public string LinkPart { get; }

        public Type ItemMarkerComponentType { get; }

        public bool NeedParent { get; }

        public override string ToString() => Name;
    }
}