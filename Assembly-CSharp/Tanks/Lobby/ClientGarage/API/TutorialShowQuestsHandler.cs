using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialShowQuestsHandler : TutorialStepHandler {
        [SerializeField] CustomizationUIComponent customizationUI;

        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            gameObject.SetActive(true);
            StepComplete();
        }

        public void OpenHullModules() {
            customizationUI.TurretModules();
        }
    }
}