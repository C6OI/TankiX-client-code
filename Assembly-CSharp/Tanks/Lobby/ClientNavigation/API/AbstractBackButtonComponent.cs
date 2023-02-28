using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public abstract class AbstractBackButtonComponent<T> : ECSBehaviour, Component, AttachToEntityListener, DetachFromEntityListener where T : Event, new() {
        Entity entity;

        public bool Disabled { get; set; }

        void Update() {
            if (entity != null && !Disabled && !InputFieldComponent.IsAnyInputFieldInFocus() && InputMapping.Cancel) {
                ScheduleEvent<T>(entity);
            }
        }

        void OnEnable() {
            Disabled = false;
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity) {
            this.entity = null;
        }
    }
}