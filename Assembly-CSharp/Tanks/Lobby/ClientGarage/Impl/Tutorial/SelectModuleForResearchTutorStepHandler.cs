using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class SelectModuleForResearchTutorStepHandler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        CollectionSlotView collectionSlot;

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
            collectionSlot = CollectionView.slots[moduleItem];
            List<GameObject> list = new();
            list.Add(collectionSlot.gameObject);
            ModulesTutorialUtil.SetOffset(list);
            TutorialCanvas.Instance.AddAdditionalMaskRect(collectionSlot.gameObject);
            NewModulesScreenUIComponent.selection.Select(collectionSlot);
            collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(OnSkipTutorial);
            modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.AddListener(OnResearchClick);
            StepComplete();
        }

        public void OnSkipTutorial() {
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(OnSkipTutorial);

            if (!(collectionSlot == null)) {
                Destroy(collectionSlot.gameObject.GetComponent<CanvasGroup>());
            }
        }

        void OnResearchClick() {
            modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(OnResearchClick);
            Destroy(collectionSlot.gameObject.GetComponent<CanvasGroup>());
        }
    }
}