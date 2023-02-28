using System.Collections;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class UpgradeModulesTutorStep4Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        [SerializeField] RectTransform popupPositionRect;

        CollectionSlotView collectionSlot;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            Debug.Log("Run 4");
            ModulesTutorialUtil.LockInteractable(modulesScreen);
            ModuleItem moduleItem = ModulesTutorialUtil.GetModuleItem(tutorialData);
            collectionSlot = CollectionView.slots[moduleItem];
            collectionSlot.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(OnSkipTutorial);
            data.ContinueOnClick = true;
            data.PopupPositionRect = popupPositionRect;
            TutorialCanvas.Instance.arrow.gameObject.SetActive(false);
            TutorialCanvas.Instance.Show(data, true);
            StartCoroutine(OverrideOnClickHandler());
        }

        void OnSkipTutorial() {
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(OnSkipTutorial);
            Destroy(collectionSlot.gameObject.GetComponent<CanvasGroup>());
        }

        public IEnumerator OverrideOnClickHandler() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            TutorialCanvas.Instance.dialog.PopupContinue = OnComplete;
        }

        void OnComplete() {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            Destroy(collectionSlot.gameObject.GetComponent<CanvasGroup>());
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
            modulesScreen.Hide();
            ModulesTutorialSystem.tutorialActive = true;
            StepComplete();
        }
    }
}