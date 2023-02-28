namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class PreciseTime {
        public static double Time => UnityEngine.Time.timeSinceLevelLoad;

        public static TimeType TimeType { get; private set; }

        internal static void Update(float deltaTime) {
            TimeType = TimeType.UPDATE;
        }

        internal static void FixedUpdate(float fixedDeltaTime) {
            TimeType = TimeType.FIXED;
        }

        internal static void AfterFixedUpdate() {
            TimeType = TimeType.LAST_FIXED;
        }
    }
}