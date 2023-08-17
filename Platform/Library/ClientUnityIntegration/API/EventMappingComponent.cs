using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public abstract class EventMappingComponent : BehaviourComponent, ComponentLifecycle {
        protected Entity entity;

        protected virtual void Awake() => Subscribe();

        void ComponentLifecycle.AttachToEntity(Entity entity) => this.entity = entity;

        void ComponentLifecycle.DetachFromEntity(Entity entity) => this.entity = null;

        protected abstract void Subscribe();

        protected virtual void SendEvent<T>() where T : Event, new() {
            if (entity != null) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<T>(entity);
                });
            }
        }
    }
}