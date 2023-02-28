using System;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TankSlotView : SlotView {
        public Image lockIcon;

        public LocalizedField emptySlotTooltipText;

        [SerializeField] LocalizedField lockedSlotTooltipText;

        [SerializeField] LocalizedField turretLockedSlotText;

        [SerializeField] LocalizedField hullLockedSlotText;

        [SerializeField] GameObject border;

        public bool Locked { get; private set; }

        public NewModulesScreenSystem.SlotNode SlotNode { get; set; }

        public void UpdateView() {
            Locked = SlotNode.Entity.HasComponent<SlotLockedComponent>();
            lockIcon.gameObject.SetActive(Locked);
            UpdateTooltip();
            border.SetActive(!HasItem());
        }

        public override void SetItem(SlotItemView item) {
            if (!item.ModuleItem.Slot.Equals(SlotNode.Entity)) {
                throw new ArgumentException(string.Format("Screen slot entity {0} doesn't match module item slot entity {1}", SlotNode.Entity.Id, item.ModuleItem.Slot.Id));
            }

            base.SetItem(item);
        }

        void UpdateTooltip() {
            if (Locked) {
                string text = SlotNode.slotTankPart.TankPart != 0 ? turretLockedSlotText.Value : hullLockedSlotText.Value;
                text = text.Replace("{0}", SlotNode.slotUserItemInfo.UpgradeLevel.ToString());
                tooltip.TipText = string.Concat(lockedSlotTooltipText, "\n", text);
            } else if (!HasItem()) {
                tooltip.TipText = emptySlotTooltipText.Value;
            } else {
                tooltip.TipText = string.Empty;
            }
        }
    }
}