using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = UnityEngine.Event;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SpectatorBackButtonComponent : MonoBehaviour, Component, ComponentLifecycle {
        Entity entity;

        [Inject] public static InputManager InputManager { get; set; }

        void OnGUI() {
            if (entity != null &&
                Event.current.type == EventType.KeyDown &&
                InputManager.GetActionKeyDown(SpectatorCameraActions.GoBack)) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    flow.ScheduleEvent<SpectatorGoBackRequestEvent>(entity);
                });
            }
        }

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => this.entity = entity;
    }
}