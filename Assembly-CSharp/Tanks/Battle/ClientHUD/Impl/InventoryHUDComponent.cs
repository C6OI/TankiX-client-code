using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientHUD.Impl {
    public class InventoryHUDComponent : BehaviourComponent, AttachToEntityListener {
        [SerializeField] List<SlotUIItem> slots;

        [SerializeField] EntityBehaviour modulePrefab;

        [SerializeField] GameObject goldBonusCounterPrefab;

        readonly List<GameObject> modules = new();

        [Inject] public static InputManager InputManager { get; set; }

        public void AttachedToEntity(Entity entity) {
            gameObject.SetActive(false);

            foreach (GameObject module in modules) {
                DestroyImmediate(module);
            }

            modules.Clear();

            foreach (SlotUIItem slot in slots) {
                slot.slotRectTransform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public EntityBehaviour CreateModule(Slot slot) {
            gameObject.SetActive(true);
            RectTransform slotRectTransform = GetSlotRectTransform(slot);
            EntityBehaviour result = Instantiate(modulePrefab, slotRectTransform);
            SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
            return result;
        }

        T Instantiate<T>(T prefab, RectTransform parent) where T : Component {
            parent.GetChild(0).gameObject.SetActive(false);
            T result = Instantiate(prefab, parent, false);
            modules.Add(result.gameObject);
            RectTransform rectTransform = (RectTransform)result.transform;
            rectTransform.anchorMin = default;
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.anchoredPosition = default;
            rectTransform.sizeDelta = default;
            return result;
        }

        public void CreateGoldBonusesCounter(EntityBehaviour module) {
            Instantiate(goldBonusCounterPrefab, module.transform, false);
        }

        RectTransform GetSlotRectTransform(Slot slot) {
            return slots.First(s => s.slot.Equals(slot)).slotRectTransform;
        }

        public string GetKeyBindForItem(ItemButtonComponent item) {
            string[] array = new string[5] {
                InventoryAction.INVENTORY_SLOT1,
                InventoryAction.INVENTORY_SLOT2,
                InventoryAction.INVENTORY_SLOT3,
                InventoryAction.INVENTORY_SLOT4,
                InventoryAction.INVENTORY_GOLDBOX
            };

            Transform parent = item.transform.parent.parent;

            for (int i = 0; i < array.Length; i++) {
                Transform child = parent.GetChild(i);

                if (item.transform.parent == child) {
                    InputAction action = InputManager.GetAction(new InputActionId("Tanks.Battle.ClientCore.Impl.InventoryAction", array[i]),
                        new InputActionContextId("Tanks.Battle.ClientCore.Impl.BasicContexts"));

                    if (action == null || action.keys.Length == 0) {
                        return string.Empty;
                    }

                    return KeyboardSettingsUtil.KeyCodeToString(action.keys[0]);
                }
            }

            return string.Empty;
        }

        [Serializable]
        public class SlotUIItem {
            public Slot slot;

            public RectTransform slotRectTransform;
        }
    }
}