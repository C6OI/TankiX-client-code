using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class TutorialFirstDamageHideTriggerComponent : TutorialHideTriggerComponent {
        [SerializeField] float showTime = 30f;

        float timer;

        [Inject] public static InputManager InputManager { get; set; }

        protected void Update() {
            if (!triggered) {
                timer += Time.deltaTime;

                if (InputManager.GetActionKeyDown(InventoryAction.INVENTORY_SLOT2) || timer >= showTime) {
                    Triggered();
                }
            }
        }

        void OnEnable() {
            timer = 0f;
        }
    }
}