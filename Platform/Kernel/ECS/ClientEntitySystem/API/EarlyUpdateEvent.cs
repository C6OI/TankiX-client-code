namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class EarlyUpdateEvent : BaseUpdateEvent {
        public static readonly EarlyUpdateEvent Instance = new();
    }
}