using System;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.Events;

namespace Tanks.Lobby.ClientGarage.API {
    public class SelectTurretStepHandler : TutorialStepHandler {
        [SerializeField] Carousel carousel;

        GarageItem selectedItem;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public override void RunStep(TutorialData data) {
            TutorialCanvas.Instance.BlockInteractable();
            base.RunStep(data);
            selectedItem = GarageItemsRegistry.GetItem<TankPartItem>(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);

            if (carousel.Selected.Item == selectedItem) {
                Complete(carousel.Selected);
                return;
            }

            Carousel obj = carousel;
            obj.onItemSelected = (UnityAction<GarageItemUI>)Delegate.Combine(obj.onItemSelected, new UnityAction<GarageItemUI>(Complete));
            carousel.Select(selectedItem);
        }

        void Complete(GarageItemUI garageItem) {
            if (garageItem.Item == selectedItem) {
                TutorialCanvas.Instance.UnblockInteractable();
                base.StepComplete();
            }
        }
    }
}