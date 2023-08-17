namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class InternalPreciseTime {
        public static void Init(float updateTime, float fixedTime) => PreciseTime.Init(updateTime, fixedTime);

        public static void Update(float deltaTime) => PreciseTime.Update(deltaTime);

        public static void FixedUpdate(float fixedDeltaTime) => PreciseTime.FixedUpdate(fixedDeltaTime);

        public static void AfterFixedUpdate() => PreciseTime.AfterFixedUpdate();
    }
}