using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.API {
    public class SelectModuleStepHandler : TutorialStepHandler {
        [SerializeField] ModulesScreenUIComponent modulesScreen;

        ModuleCardItemUIComponent selectedModule;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            selectedModule = modulesScreen.GetCard(data.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId);

            if (selectedModule != null) {
                TutorialCanvas.Instance.AddAdditionalMaskRect(selectedModule.gameObject);
                TutorialCanvas.Instance.AddAllowSelectable(selectedModule.GetComponent<Toggle>());
                selectedModule.GetComponent<Toggle>().isOn = true;
            }

            StepComplete();
        }
    }
}