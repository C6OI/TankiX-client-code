namespace Tanks.Battle.ClientCore.API {
    public class LayerMasks {
        public static readonly int STATIC = LayerMasksUtils.CreateLayerMask(Layers.STATIC);

        public static readonly int VISUAL_STATIC = LayerMasksUtils.CreateLayerMask(Layers.VISUAL_STATIC);

        public static readonly int ALL_STATIC = LayerMasksUtils.AddLayerToMask(STATIC, Layers.VISUAL_STATIC);

        public static readonly int VISIBLE_FOR_CHASSIS_ACTIVE = LayerMasksUtils.AddLayerToMask(STATIC, Layers.TANK_TO_TANK);

        public static readonly int VISIBLE_FOR_CHASSIS_SEMI_ACTIVE = STATIC;

        public static readonly int VISIBLE_FOR_CHASSIS_ANIMATION = ALL_STATIC;

        public static readonly int GUN_TARGETING_WITHOUT_DEAD_UNITS =
            LayerMasksUtils.AddLayersToMask(VISUAL_STATIC, Layers.TARGET);

        public static readonly int GUN_TARGETING_WITH_DEAD_UNITS =
            LayerMasksUtils.AddLayersToMask(GUN_TARGETING_WITHOUT_DEAD_UNITS, Layers.DEAD_TARGET);

        public static readonly int VISUAL_TARGETING =
            LayerMasksUtils.AddLayerToMask(VISUAL_STATIC, Layers.TANK_PART_VISUAL_TRIGGER);
    }
}