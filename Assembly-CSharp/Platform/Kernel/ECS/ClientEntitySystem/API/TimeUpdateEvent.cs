namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class TimeUpdateEvent : BaseUpdateEvent {
        public static readonly TimeUpdateEvent Instance = new();
    }
}