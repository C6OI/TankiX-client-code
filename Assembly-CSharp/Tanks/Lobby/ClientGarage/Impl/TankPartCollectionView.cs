using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.System;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TankPartCollectionView : MonoBehaviour {
        public TankSlotView activeSlot;

        public TankSlotView activeSlot2;

        public TankSlotView passiveSlot;

        public GameObject tankPartView;

        public ImageSkin preview;

        public TextMeshProUGUI partName;

        public LineCollectionView lineCollectionView;

        public CanvasGroup slotContainer;

        [SerializeField] UpgradeStars upgradeStars;

        [SerializeField] TextMeshProUGUI bonusFromModules;

        [SerializeField] TextMeshProUGUI basePartParam;

        [SerializeField] TextMeshProUGUI partLevel;

        [SerializeField] LocalizedField basePartParamName;

        public Entity marketEntity;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public float BonusFromModules {
            set {
                bonusFromModules.gameObject.SetActive(value > 0f);
                bonusFromModules.text = string.Format("{0}", "+ " + value);
            }
        }

        public string BasePartParam {
            set => basePartParam.text = string.Format("{0} {1}", basePartParamName.Value, value);
        }

        public string PartLevel {
            set => partLevel.text = value;
        }

        public void SetStars(float coef) {
            upgradeStars.SetPower(coef);
        }

        public void TurnOnSlotsByTypeAndHighlightForDrop(ModuleBehaviourType type) {
            if (type == ModuleBehaviourType.PASSIVE) {
                if (!passiveSlot.Locked) {
                    TurnOnAndHighlight(passiveSlot);
                }

                return;
            }

            if (!activeSlot.Locked) {
                TurnOn(activeSlot);

                if (activeSlot2.Locked) {
                    activeSlot.HighlightForDrop();
                } else if (!activeSlot.HasItem() || activeSlot.HasItem() && activeSlot2.HasItem()) {
                    activeSlot.HighlightForDrop();
                }
            }

            if (!activeSlot2.Locked) {
                TurnOn(activeSlot2);

                if (activeSlot.Locked) {
                    activeSlot2.HighlightForDrop();
                } else if (!activeSlot2.HasItem() || activeSlot.HasItem() && activeSlot2.HasItem()) {
                    activeSlot2.HighlightForDrop();
                }
            }
        }

        public TankSlotView GetSlotForDrop(ModuleBehaviourType type) {
            if (type == ModuleBehaviourType.PASSIVE) {
                return !passiveSlot.Locked ? passiveSlot : null;
            }

            if (activeSlot.Locked && activeSlot2.Locked) {
                return null;
            }

            if (activeSlot.Locked) {
                return activeSlot2;
            }

            if (activeSlot2.Locked) {
                return activeSlot;
            }

            if (activeSlot.HasItem() && activeSlot2.HasItem()) {
                return activeSlot;
            }

            return !activeSlot.HasItem() ? activeSlot : activeSlot2;
        }

        public void CancelHighlightForDrop() {
            activeSlot.GetComponent<DragAndDropCell>().enabled = true;
            activeSlot2.GetComponent<DragAndDropCell>().enabled = true;
            passiveSlot.GetComponent<DragAndDropCell>().enabled = true;
            CancelHighlight(activeSlot);
            CancelHighlight(activeSlot2);
            CancelHighlight(passiveSlot);
        }

        public void TurnOffSlots() {
            activeSlot.GetComponent<DragAndDropCell>().enabled = false;
            activeSlot2.GetComponent<DragAndDropCell>().enabled = false;
            passiveSlot.GetComponent<DragAndDropCell>().enabled = false;
        }

        public void UpdateView(TankPartItem tankPart) {
            activeSlot.UpdateView();
            activeSlot2.UpdateView();
            passiveSlot.UpdateView();
            CalculateTankPartUpgradeCoeffEvent calculateTankPartUpgradeCoeffEvent = new();
            tankPart.MarketItem.ScheduleEvent(calculateTankPartUpgradeCoeffEvent);
            SetStars(calculateTankPartUpgradeCoeffEvent.UpgradeCoeff);
            VisualProperty visualProperty = tankPart.Properties[0];
            BasePartParam = string.Format("{0}", visualProperty.InitialValue);
            BonusFromModules = (int)(visualProperty.GetValue(calculateTankPartUpgradeCoeffEvent.UpgradeCoeff) - visualProperty.InitialValue);
        }

        void TurnOnAndHighlight(TankSlotView slot) {
            TurnOn(slot);
            slot.HighlightForDrop();
        }

        void TurnOn(TankSlotView slot) {
            slot.GetComponent<DragAndDropCell>().enabled = true;
            slot.TurnOnRenderAboveAll();
        }

        void CancelHighlight(TankSlotView slot) {
            slot.TurnOffRenderAboveAll();
            slot.CancelHighlightForDrop();
        }

        public TankSlotView GetSlotBySlotEntity(Entity slotEntity) {
            if (activeSlot.SlotNode.Entity.Equals(slotEntity)) {
                return activeSlot;
            }

            if (activeSlot2.SlotNode.Entity.Equals(slotEntity)) {
                return activeSlot2;
            }

            if (passiveSlot.SlotNode.Entity.Equals(slotEntity)) {
                return passiveSlot;
            }

            return null;
        }
    }
}