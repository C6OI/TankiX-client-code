using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public abstract class LocalizedScreenComponent : FromConfigBehaviour, Component, AttachToEntityListener {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public string Header { private get; set; }

        public void AttachedToEntity(Entity entity) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Animated(Header);
            EngineService.Engine.ScheduleEvent(setScreenHeaderEvent, entity);
        }

        protected override string GetRelativeConfigPath() => "/ui/screen";

        public void DetachFromEntity(Entity entity) { }
    }
}