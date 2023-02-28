using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class UpgradeModuleTutorStep12Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        public List<GameObject> highlightedObjects;

        [SerializeField] RectTransform popupPositionRect;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            ModulesTutorialUtil.SetOffset(highlightedObjects);

            foreach (GameObject highlightedObject in highlightedObjects) {
                TutorialCanvas.Instance.AddAdditionalMaskRect(highlightedObject);
            }

            ModulesTutorialUtil.LockInteractable(modulesScreen);
            data.ContinueOnClick = true;
            data.PopupPositionRect = popupPositionRect;
            TutorialCanvas.Instance.arrow.gameObject.SetActive(false);
            TutorialCanvas.Instance.Show(data, true);
            StartCoroutine(OverrideOnClickHandler());
        }

        public IEnumerator OverrideOnClickHandler() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            TutorialCanvas.Instance.dialog.PopupContinue = OnComplete;
        }

        void OnComplete() {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
            modulesScreen.Hide();
            StepComplete();
        }
    }
}