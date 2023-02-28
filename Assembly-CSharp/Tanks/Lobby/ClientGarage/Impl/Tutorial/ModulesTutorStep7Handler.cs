using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class ModulesTutorStep7Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        public List<GameObject> highlightedObjects;

        [SerializeField] RectTransform popupPositionRect;

        public override void RunStep(TutorialData data) {
            Debug.Log("RunStep 7");
            base.RunStep(data);
            highlightedObjects.Add(modulesScreen.collectionView.turretCollectionView.activeTierCollectionList.transform.GetChild(0).gameObject);
            highlightedObjects.Add(modulesScreen.collectionView.turretCollectionView.passiveTierCollectionList.transform.GetChild(0).gameObject);

            foreach (GameObject highlightedObject in highlightedObjects) {
                TutorialCanvas.Instance.AddAdditionalMaskRect(highlightedObject);
            }

            ModulesTutorialUtil.SetOffset(highlightedObjects);
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
            OnSkipTutorial();
            StepComplete();
        }

        public void OnSkipTutorial() {
            TutorialCanvas.Instance.dialog.PopupContinue = null;
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
        }
    }
}