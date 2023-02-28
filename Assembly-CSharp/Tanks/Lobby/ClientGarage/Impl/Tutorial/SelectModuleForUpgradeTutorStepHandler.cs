using System;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class SelectModuleForUpgradeTutorStepHandler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public override void RunStep(TutorialData data) {
            base.RunStep(data);

            if (!modulesScreen.showAnimationFinished) {
                NewModulesScreenUIComponent newModulesScreenUIComponent = modulesScreen;

                newModulesScreenUIComponent.OnShowAnimationFinishedAction =
                    (Action)Delegate.Combine(newModulesScreenUIComponent.OnShowAnimationFinishedAction, new Action(RunStepDelayed));
            } else {
                RunStep();
            }
        }

        public void RunStepDelayed() {
            Debug.Log("RunStepDelayed");
            NewModulesScreenUIComponent newModulesScreenUIComponent = modulesScreen;
            newModulesScreenUIComponent.OnShowAnimationFinishedAction = (Action)Delegate.Remove(newModulesScreenUIComponent.OnShowAnimationFinishedAction, new Action(RunStepDelayed));
            RunStep();
        }

        void RunStep() {
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(tutorialData);
            CollectionSlotView collectionSlotView = CollectionView.slots[moduleItem];
            List<GameObject> list = new();
            list.Add(collectionSlotView.gameObject);
            list.Add(modulesScreen.turretCollectionView.gameObject);
            ModulesTutorialUtil.SetOffset(list);
            NewModulesScreenUIComponent.selection.Select(NewModulesScreenUIComponent.slotItems[moduleItem]);
            TutorialCanvas.Instance.AddAdditionalMaskRect(collectionSlotView.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(modulesScreen.turretCollectionView.gameObject);
            StepComplete();
        }
    }
}