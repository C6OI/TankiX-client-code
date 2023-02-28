using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class UpgradeModuleTutorStep7Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        [SerializeField] RectTransform popupPositionRect;

        [SerializeField] RectTransform arrowPositionRect;

        CollectionSlotView collectionSlot;

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

        void RunStep() {
            ModulesTutorialUtil.LockInteractable(modulesScreen);
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(tutorialData);
            collectionSlot = CollectionView.slots[moduleItem];
            collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            List<GameObject> list = new();
            list.Add(collectionSlot.gameObject);
            list.Add(modulesScreen.turretCollectionView.gameObject);
            list.Add(modulesScreen.selectedModuleView.UpgradeCRYButton);
            ModulesTutorialUtil.SetOffset(list);
            TutorialCanvas.Instance.AddAdditionalMaskRect(collectionSlot.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(modulesScreen.turretCollectionView.gameObject);
            TutorialCanvas.Instance.AddAdditionalMaskRect(modulesScreen.selectedModuleView.UpgradeCRYButton);
            TutorialCanvas.Instance.AddAllowSelectable(modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>());
            NewModulesScreenUIComponent.selection.Select(NewModulesScreenUIComponent.slotItems[moduleItem]);
            modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>().onClick.AddListener(OnUpgradeClick);
            tutorialData.PopupPositionRect = popupPositionRect;
            TutorialCanvas.Instance.Show(tutorialData, true, null, arrowPositionRect);
        }

        public void RunStepDelayed() {
            Debug.Log("RunStepDelayed");
            NewModulesScreenUIComponent newModulesScreenUIComponent = modulesScreen;
            newModulesScreenUIComponent.OnShowAnimationFinishedAction = (Action)Delegate.Remove(newModulesScreenUIComponent.OnShowAnimationFinishedAction, new Action(RunStepDelayed));
            RunStep();
        }

        public void OnSkipTutorial() {
            if (!(collectionSlot == null)) {
                ModulesTutorialUtil.ResetOffset();
                ModulesTutorialUtil.UnlockInteractable(modulesScreen);
                CanvasGroup component = collectionSlot.gameObject.GetComponent<CanvasGroup>();

                if (component != null) {
                    Destroy(component);
                }
            }
        }

        void OnUpgradeClick() {
            modulesScreen.selectedModuleView.UpgradeCRYButton.GetComponent<Button>().onClick.RemoveListener(OnUpgradeClick);
            StartCoroutine(Complete());
        }

        IEnumerator Complete() {
            yield return new WaitForEndOfFrame();

            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
            Destroy(collectionSlot.GetComponent<CanvasGroup>());
            ModulesTutorialSystem.tutorialActive = false;
            StepComplete();
        }
    }
}