using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class ModulesTutorialUtil {
        public static float Z_OFFSET = -7f;

        public static bool TUTORIAL_MODE = false;

        static readonly List<GameObject> movedObjects = new();

        static NewModulesScreenUIComponent modulesScreen;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public static void SetOffset(List<GameObject> objects) {
            foreach (GameObject @object in objects) {
                SetZOffset(@object, Z_OFFSET);
            }

            SetZOffset(TutorialCanvas.Instance.overlay, Z_OFFSET + 5f);
            SetZOffset(TutorialCanvas.Instance.SkipTutorialButton, Z_OFFSET);
            movedObjects.AddRange(objects);
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(OnTutorialSkipButtonComponent_ResetOffset);
        }

        public static void ResetOffset() {
            foreach (GameObject movedObject in movedObjects) {
                SetZOffset(movedObject, 0f);
            }

            movedObjects.Clear();
            SetZOffset(TutorialCanvas.Instance.overlay, 0f);
            SetZOffset(TutorialCanvas.Instance.SkipTutorialButton, 0f);
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(OnTutorialSkipButtonComponent_ResetOffset);
        }

        static void OnTutorialSkipButtonComponent_ResetOffset() {
            ResetOffset();
        }

        static void SetZOffset(GameObject gameObject, float zOffset) {
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Vector3 anchoredPosition3D = component.anchoredPosition3D;
            anchoredPosition3D.z = zOffset;
            component.anchoredPosition3D = anchoredPosition3D;
        }

        public static ModuleItem GetModuleItem(TutorialData tutorialData) {
            long itemMarketId = tutorialData.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId;
            ModuleItem moduleItem = null;

            foreach (ModuleItem module in GarageItemsRegistry.Modules) {
                if (module.MarketItem.Id.Equals(itemMarketId)) {
                    return module;
                }
            }

            return null;
        }

        public static void LockInteractable(NewModulesScreenUIComponent modulesScreen) {
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = false;
            modulesScreen.turretCollectionView.GetComponent<CanvasGroup>().alpha = 1f;
            modulesScreen.turretCollectionView.slotContainer.blocksRaycasts = false;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.GetComponent<Animator>().enabled = false;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.backButton.interactable = false;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ModulesTutorialUtil.modulesScreen = modulesScreen;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(OnTutorialSkipButton_Unlock);
        }

        static void OnTutorialSkipButton_Unlock() {
            UnlockInteractable(modulesScreen);
        }

        public static void UnlockInteractable(NewModulesScreenUIComponent modulesScreen) {
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = true;
            modulesScreen.turretCollectionView.slotContainer.blocksRaycasts = true;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.GetComponent<Animator>().enabled = true;
            modulesScreen.backButton.interactable = true;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(OnTutorialSkipButton_Unlock);
        }
    }
}