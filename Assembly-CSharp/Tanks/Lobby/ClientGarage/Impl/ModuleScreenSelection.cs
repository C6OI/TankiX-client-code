using System;
using System.Collections.Generic;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleScreenSelection {
        static Dictionary<SlotView, ModuleItem> slotToModuleItem;

        static readonly List<ModuleItem> registeredSlotItems = new();
        public Action<ModuleItem> onSelectionChange;

        public CollectionSlotView selectedSlot;

        public SlotItemView selectedSlotItem;

        public ModuleScreenSelection() {
            DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;

            if (slotToModuleItem != null) {
                slotToModuleItem.Clear();
                slotToModuleItem = null;
            }

            registeredSlotItems.Clear();
        }

        public void Update(Dictionary<ModuleItem, CollectionSlotView> collectionViewSlots, Dictionary<ModuleItem, SlotItemView> slotItems) {
            if (slotToModuleItem == null) {
                slotToModuleItem = new Dictionary<SlotView, ModuleItem>();

                foreach (KeyValuePair<ModuleItem, CollectionSlotView> collectionViewSlot in collectionViewSlots) {
                    CollectionSlotView value = collectionViewSlot.Value;
                    value.onClick = (Action<CollectionSlotView>)Delegate.Combine(value.onClick, new Action<CollectionSlotView>(OnSlotClick));
                    slotToModuleItem.Add(value, collectionViewSlot.Key);
                }
            }

            foreach (KeyValuePair<ModuleItem, SlotItemView> slotItem in slotItems) {
                if (!registeredSlotItems.Contains(slotItem.Key)) {
                    registeredSlotItems.Add(slotItem.Key);
                    SlotItemView value2 = slotItem.Value;
                    value2.onClick = (Action<SlotItemView>)Delegate.Combine(value2.onClick, new Action<SlotItemView>(OnItemClick));
                }
            }
        }

        void OnItemClick(SlotItemView slotItem) {
            Select(slotItem);
        }

        void OnSlotClick(CollectionSlotView slot) {
            Select(slot);
        }

        public ModuleItem GetSelectedModuleItem() {
            if (selectedSlot != null) {
                return slotToModuleItem[selectedSlot];
            }

            if (selectedSlotItem != null) {
                return selectedSlotItem.ModuleItem;
            }

            return null;
        }

        public void Select(SlotItemView slotItem) {
            if (!(selectedSlotItem == slotItem)) {
                if (selectedSlot != null) {
                    selectedSlot.Deselect();
                    selectedSlot = null;
                }

                if (selectedSlotItem != null) {
                    selectedSlotItem.Deselect();
                }

                slotItem.Select();
                selectedSlotItem = slotItem;

                if (onSelectionChange != null) {
                    onSelectionChange(slotItem.ModuleItem);
                }
            }
        }

        public void Select(CollectionSlotView slot) {
            if (!(selectedSlot == slot)) {
                if (selectedSlotItem != null) {
                    selectedSlotItem.Deselect();
                    selectedSlotItem = null;
                }

                if (selectedSlot != null) {
                    selectedSlot.Deselect();
                }

                slot.Select();
                selectedSlot = slot;

                if (onSelectionChange != null) {
                    onSelectionChange(slotToModuleItem[selectedSlot]);
                }
            }
        }

        void OnAnyItemDragStart(DragAndDropItem item, PointerEventData eventData) {
            SlotItemView component = item.GetComponent<SlotItemView>();
            Select(component);
        }

        public void Clear() {
            if (selectedSlot != null) {
                selectedSlot.Deselect();
                selectedSlot = null;

                if (onSelectionChange != null) {
                    onSelectionChange(null);
                }
            } else if (selectedSlotItem != null) {
                selectedSlotItem.Deselect();
                selectedSlotItem = null;

                if (onSelectionChange != null) {
                    onSelectionChange(null);
                }
            }
        }
    }
}