namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class PreciseTime {
        static double updateTime;

        public static double Time { get; private set; }

        public static double FixedTime { get; private set; }

        public static TimeType TimeType { get; private set; }

        internal static void Init(float updateTime, float fixedTime) {
            PreciseTime.updateTime = updateTime;
            FixedTime = fixedTime;
        }

        internal static void Update(float deltaTime) {
            updateTime += deltaTime;
            Time = updateTime;
            TimeType = TimeType.UPDATE;
        }

        internal static void FixedUpdate(float fixedDeltaTime) {
            FixedTime += fixedDeltaTime;
            Time = FixedTime;
            TimeType = TimeType.FIXED;
        }

        internal static void AfterFixedUpdate() => TimeType = TimeType.LAST_FIXED;
    }
}