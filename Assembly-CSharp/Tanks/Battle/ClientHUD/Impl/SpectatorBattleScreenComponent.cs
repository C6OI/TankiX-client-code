using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SpectatorBattleScreenComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener {
        public GameObject scoreTable;

        public GameObject scoreTableShadow;

        public GameObject spectatorChat;

        Entity entity;

        [Inject] public static InputManager InputManager { get; set; }

        void OnDisable() {
            spectatorChat.SetActive(false);
        }

        void OnGUI() {
            if (entity != null && Event.current.type == EventType.KeyDown && InputManager.GetActionKeyDown(SpectatorCameraActions.GoBack)) {
                EngineService.Engine.ScheduleEvent<SpectatorGoBackRequestEvent>(entity);
            }
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity) {
            this.entity = null;
        }
    }
}