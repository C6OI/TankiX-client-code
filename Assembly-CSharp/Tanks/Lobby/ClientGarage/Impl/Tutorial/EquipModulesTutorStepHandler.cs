using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class EquipModulesTutorStepHandler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        [SerializeField] RectTransform popupPositionRect;

        [SerializeField] RectTransform arrowPositionRect;

        CollectionSlotView collectionSlot;

        SlotItemView moduleSlotItem;

        bool tryToRunStep;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void Update() {
            if (tryToRunStep) {
                ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(tutorialData);

                if (moduleItem != null && NewModulesScreenUIComponent.slotItems.ContainsKey(moduleItem)) {
                    tryToRunStep = false;
                    collectionSlot = CollectionView.slots[moduleItem];
                    List<GameObject> list = new();
                    list.Add(modulesScreen.turretCollectionView.gameObject);
                    list.Add(collectionSlot.gameObject);
                    ModulesTutorialUtil.SetOffset(list);
                    moduleSlotItem = NewModulesScreenUIComponent.slotItems[moduleItem];
                    NewModulesScreenUIComponent.selection.Select(moduleSlotItem);
                    DragAndDropController dragAndDropController = modulesScreen.dragAndDropController;

                    dragAndDropController.onDrop =
                        (Action<DropDescriptor, DropDescriptor>)Delegate.Combine(dragAndDropController.onDrop, new Action<DropDescriptor, DropDescriptor>(OnDrop));

                    TutorialCanvas.Instance.AddAdditionalMaskRect(modulesScreen.turretCollectionView.gameObject);
                    TutorialCanvas.Instance.AddAdditionalMaskRect(collectionSlot.gameObject.transform.parent.gameObject);
                    tutorialData.PopupPositionRect = popupPositionRect;
                    TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(UnlockInteractable);
                    TutorialCanvas.Instance.Show(tutorialData, true, null, arrowPositionRect);
                }
            }
        }

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            ModulesTutorialUtil.TUTORIAL_MODE = true;
            tryToRunStep = true;
            LockInteractable();
            tryToRunStep = true;
        }

        void OnDrop(DropDescriptor dropDescriptor, DropDescriptor backDescriptor) {
            if (dropDescriptor.sourceCell.gameObject != collectionSlot.gameObject) {
                Debug.Log("dropDescriptor.sourceCell.gameObject != collectionSlot.gameObject");
            } else if (dropDescriptor.item.gameObject != moduleSlotItem.gameObject) {
                Debug.Log("dropDescriptor.item.gameObject != moduleSlotItem.gameObject");
            } else {
                StartCoroutine(Complete());
            }
        }

        IEnumerator Complete() {
            yield return new WaitForEndOfFrame();

            ModulesTutorialUtil.ResetOffset();
            UnlockInteractable();
            ModulesTutorialUtil.TUTORIAL_MODE = false;
            StepComplete();
        }

        public void OnSkipTutorial() {
            ModulesTutorialUtil.ResetOffset();
            UnlockInteractable();
            ModulesTutorialUtil.TUTORIAL_MODE = false;
        }

        void LockInteractable() {
            Debug.Log("LockInteractable equip");
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = false;
            modulesScreen.turretCollectionView.GetComponent<CanvasGroup>().alpha = 1f;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.GetComponent<Animator>().enabled = false;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.backButton.interactable = false;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        void UnlockInteractable() {
            Debug.Log("UnlockInteractable equip");
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = true;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.backButton.interactable = true;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(UnlockInteractable);
        }
    }
}