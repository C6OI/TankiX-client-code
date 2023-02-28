using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = UnityEngine.Event;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824351578525226L)]
    public class BattleScreenComponent : ECSBehaviour, Component, AttachToEntityListener {
        public GameObject hud;

        public GameObject topPanel;

        public GameObject tankInfo;

        public GameObject battleChat;

        public GameObject combatEventLog;

        public bool showTankInfo;

        public bool showBattleChat;

        public bool showCombatEventLog;
        Entity entity;

        [Inject] public static InputManager InputManager { get; set; }

        void OnEnable() {
            hud.SetActive(true);
            topPanel.GetComponent<CanvasGroup>().alpha = 1f;
            tankInfo.SetActive(showTankInfo);
            battleChat.SetActive(showBattleChat);
        }

        void OnDisable() {
            if (hud != null) {
                hud.SetActive(false);
            }

            if (tankInfo != null) {
                tankInfo.SetActive(false);
            }

            if (battleChat != null) {
                battleChat.SetActive(false);
            }

            if (combatEventLog != null) {
                combatEventLog.SetActive(false);
            }
        }

        void OnGUI() {
            if (entity != null && Event.current.type == EventType.KeyDown && InputManager.GetActionKeyDown(BattleActions.EXIT_BATTLE)) {
                ScheduleEvent<RequestGoBackFromBattleEvent>(entity);
            }
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }
    }
}