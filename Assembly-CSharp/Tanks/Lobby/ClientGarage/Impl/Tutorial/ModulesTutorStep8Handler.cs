using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class ModulesTutorStep8Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        public List<GameObject> highlightedObjects;

        [SerializeField] RectTransform popupPositionRect;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            ModulesTutorialUtil.LockInteractable(modulesScreen);
            GameObject gameObject = modulesScreen.collectionView.turretCollectionView.activeTierCollectionList.transform.GetChild(0).gameObject;

            for (int i = 0; i < gameObject.transform.childCount; i++) {
                highlightedObjects.Add(gameObject.transform.GetChild(i).gameObject);
            }

            GameObject gameObject2 = modulesScreen.collectionView.turretCollectionView.passiveTierCollectionList.transform.GetChild(0).gameObject;

            for (int j = 0; j < gameObject2.transform.childCount; j++) {
                highlightedObjects.Add(gameObject.transform.GetChild(j).gameObject);
            }

            foreach (GameObject highlightedObject in highlightedObjects) {
                TutorialCanvas.Instance.AddAdditionalMaskRect(highlightedObject);
            }

            ModulesTutorialUtil.SetOffset(highlightedObjects);
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
            OnSkipTutorial();
            StepComplete();
        }

        public void OnSkipTutorial() {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
            modulesScreen.Hide();
        }
    }
}