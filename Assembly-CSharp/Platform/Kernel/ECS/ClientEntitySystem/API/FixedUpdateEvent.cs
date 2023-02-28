namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class FixedUpdateEvent : BaseUpdateEvent {
        public static readonly FixedUpdateEvent Instance = new();
    }
}