namespace Tanks.Battle.ClientCore.API {
    public class Layers {
        public static readonly int DEFAULT = 0;

        public static readonly int TRANSPARENT_FX = 1;

        public static readonly int IGNORE_RAYCAST = 2;

        public static readonly int WATER = 4;

        public static readonly int UI = 5;

        public static readonly int TANK_AND_STATIC = 8;

        public static readonly int FRICTION = 9;

        public static readonly int MINOR_VISUAL = 10;

        public static readonly int STATIC = 11;

        public static readonly int TRIGGER_WITH_SELF_TANK = 14;

        public static readonly int TANK_TO_STATIC = 15;

        public static readonly int SELF_SEMIACTIVE_TANK_BOUNDS = 16;

        public static readonly int REMOTE_TANK_BOUNDS = 17;

        public static readonly int TARGET = 18;

        public static readonly int TANK_TO_TANK = 20;

        public static readonly int TANK_PART_VISUAL = 22;

        public static readonly int VISUAL_STATIC = 23;

        public static readonly int DEAD_TARGET = 24;

        public static readonly int GRASS_GENERATION = 25;

        public static readonly int SELF_TANK_BOUNDS = 26;

        public static readonly int LOGIC_ELEMENTS = 28;

        public static readonly int GRASS = 29;

        public static readonly int HANGAR = 30;

        public static readonly int INVISIBLE_PHYSICS = IGNORE_RAYCAST;

        public static readonly int EXCLUSION_RAYCAST = IGNORE_RAYCAST;
    }
}