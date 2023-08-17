namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public abstract class BaseUpdateEvent : Event {
        public float DeltaTime { get; set; }
    }
}