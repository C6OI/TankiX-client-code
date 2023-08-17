using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientNavigation.API {
    public abstract class LocalizedScreenComponent : FromConfigBehaviour, Component, ComponentLifecycle {
        public string Header { private get; set; }

        public void AttachToEntity(Entity entity) => ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Animated(Header);
            e.ScheduleEvent(setScreenHeaderEvent, entity);
        });

        public void DetachFromEntity(Entity entity) { }

        protected override string GetRelativeConfigPath() => "/ui/screen";
    }
}