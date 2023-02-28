using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public static class ItemRarityExtensions {
        static readonly Color COMMON_COLOR = new(0.86f, 0.86f, 0.86f);

        static readonly Color RARE_COLOR = new(0.24f, 0.72f, 0.97f);

        static readonly Color EPIC_COLOR = new(0.71f, 0.57f, 1f);

        static readonly Color LEGENDARY_COLOR = new(1f, 0.42f, 0.22f);

        public static Color GetRarityColor(this ItemRarityType rarity) {
            switch (rarity) {
                case ItemRarityType.COMMON:
                    return COMMON_COLOR;

                case ItemRarityType.RARE:
                    return RARE_COLOR;

                case ItemRarityType.EPIC:
                    return EPIC_COLOR;

                case ItemRarityType.LEGENDARY:
                    return LEGENDARY_COLOR;

                default:
                    return COMMON_COLOR;
            }
        }
    }
}