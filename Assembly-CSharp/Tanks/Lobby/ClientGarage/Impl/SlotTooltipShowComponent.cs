using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotTooltipShowComponent : TooltipShowBehaviour {
        [SerializeField] GameObject slotLockedTooltip;

        [SerializeField] GameObject moduleTooltip;

        [SerializeField] LocalizedField slotLockedTitle;

        [SerializeField] LocalizedField weaponSlotLocked;

        [SerializeField] LocalizedField hullSlotLocked;

        [SerializeField] LocalizedField emptySlot;

        [SerializeField] PaletteColorField lockedHeaderColor;

        SlotUIComponent slot => GetComponent<SlotUIComponent>();

        public override void ShowTooltip(Vector3 mousePosition) {
            Engine engine = EngineService.Engine;
            CheckForTutorialEvent checkForTutorialEvent = new();
            engine.ScheduleEvent(checkForTutorialEvent, EngineService.EntityStub);

            if (!checkForTutorialEvent.TutorialIsActive) {
                tooltipShowed = true;

                if (slot.Locked) {
                    ShowLockedModuleTooltip();
                } else if (slot.SlotEntity != null) {
                    engine.ScheduleEvent<ModuleTooltipShowEvent>(slot.SlotEntity);
                }
            }
        }

        void ShowLockedModuleTooltip() {
            string text = "<color=#" + lockedHeaderColor.Color.ToHexString() + ">" + slotLockedTitle.Value + "</color>";
            string text2 = slot.TankPart != 0 ? weaponSlotLocked.Value : hullSlotLocked.Value;
            string text3 = text2.Replace("{0}", slot.Rank.ToString());
            string[] data = new string[2] { text, text3 };
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, slotLockedTooltip);
        }

        public void ShowModuleTooltip(object data) {
            TooltipController.Instance.ShowTooltip(Input.mousePosition, data, moduleTooltip);
        }

        public void ShowEmptySlotTooltip() {
            TooltipController.Instance.ShowTooltip(Input.mousePosition, emptySlot.Value);
        }
    }
}