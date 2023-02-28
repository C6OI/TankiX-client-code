using System.Collections;
using System.Collections.Generic;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class ModulesTutorialStep4Handler : TutorialStepHandler {
        public NewModulesScreenUIComponent modulesScreen;

        [SerializeField] RectTransform popupPositionRect;

        [SerializeField] RectTransform arrowPositionRect;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            StartCoroutine(RunStepInNextFrame());
        }

        public IEnumerator RunStepInNextFrame() {
            yield return new WaitForEndOfFrame();

            ModulesTutorialUtil.LockInteractable(modulesScreen);
            List<GameObject> list = new();
            list.Add(modulesScreen.selectedModuleView.ResearchButton);
            ModulesTutorialUtil.SetOffset(list);
            TutorialCanvas.Instance.AddAdditionalMaskRect(modulesScreen.selectedModuleView.ResearchButton);
            tutorialData.PopupPositionRect = popupPositionRect;
            modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.AddListener(OnResearchClick);
            TutorialCanvas.Instance.AddAllowSelectable(modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>());
            TutorialCanvas.Instance.Show(tutorialData, true, null, arrowPositionRect);
        }

        void OnResearchClick() {
            modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(OnResearchClick);
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
            StepComplete();
        }

        public void OnSkipTutorial() {
            modulesScreen.selectedModuleView.ResearchButton.GetComponent<Button>().onClick.RemoveListener(OnResearchClick);
            ModulesTutorialUtil.ResetOffset();
            ModulesTutorialUtil.UnlockInteractable(modulesScreen);
        }
    }
}