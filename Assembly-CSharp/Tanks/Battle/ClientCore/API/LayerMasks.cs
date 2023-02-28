namespace Tanks.Battle.ClientCore.API {
    public class LayerMasks {
        public static readonly int STATIC = LayerMasksUtils.CreateLayerMask(Layers.STATIC);

        public static readonly int VISUAL_STATIC = LayerMasksUtils.CreateLayerMask(Layers.VISUAL_STATIC);

        public static readonly int TANK_TO_TANK = LayerMasksUtils.CreateLayerMask(Layers.TANK_TO_TANK);

        public static readonly int ALL_STATIC = LayerMasksUtils.AddLayerToMask(STATIC, Layers.VISUAL_STATIC);

        public static readonly int VISIBLE_FOR_CHASSIS_ACTIVE = LayerMasksUtils.AddLayerToMask(STATIC, Layers.TANK_TO_TANK);

        public static readonly int VISIBLE_FOR_CHASSIS_SEMI_ACTIVE = STATIC;

        public static readonly int VISIBLE_FOR_CHASSIS_ANIMATION = ALL_STATIC;

        public static readonly int GUN_TARGETING_WITHOUT_DEAD_UNITS = LayerMasksUtils.AddLayersToMask(VISUAL_STATIC, Layers.TARGET);

        public static readonly int GUN_TARGET = LayerMasksUtils.AddLayersToMask(Layers.TARGET);

        public static readonly int GUN_TARGETING_WITH_DEAD_UNITS = LayerMasksUtils.AddLayersToMask(GUN_TARGETING_WITHOUT_DEAD_UNITS, Layers.DEAD_TARGET);

        public static readonly int GUN_TARGETING_WITH_DEAD_UNITS_WITHOUT__VISUAL_STATIC = LayerMasksUtils.RemoveLayerFromMask(GUN_TARGETING_WITH_DEAD_UNITS, Layers.VISUAL_STATIC);

        public static readonly int GUN_TARGETING_WITH_DEAD_UNITS_BY_SIMPLE_STATIC = LayerMasksUtils.AddLayerToMask(GUN_TARGETING_WITH_DEAD_UNITS_WITHOUT__VISUAL_STATIC, Layers.STATIC);

        public static readonly int VISUAL_TARGETING = LayerMasksUtils.AddLayersToMask(VISUAL_STATIC, Layers.TANK_PART_VISUAL);

        public static readonly int BOT_COLLISION = LayerMasksUtils.AddLayerToMask(Layers.TANK_TO_TANK, Layers.STATIC);
    }
}