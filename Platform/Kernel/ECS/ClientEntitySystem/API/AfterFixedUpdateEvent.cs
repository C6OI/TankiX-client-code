namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class AfterFixedUpdateEvent : Event {
        public static readonly AfterFixedUpdateEvent Instance = new();
    }
}