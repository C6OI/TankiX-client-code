using System;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialShowArmsRaceGiftsHandler : TutorialStepHandler {
        public override void RunStep(TutorialData data) {
            base.RunStep(data);
            ShowArmsRaceIntroEvent showArmsRaceIntroEvent = new();
            ScheduleEvent(showArmsRaceIntroEvent, EngineService.EntityStub);

            if (showArmsRaceIntroEvent.Dialog != null) {
                ConfirmDialogComponent dialog = showArmsRaceIntroEvent.Dialog;
                dialog.dialogClosed = (Action)Delegate.Combine(dialog.dialogClosed, new Action(DialogClosed));
            } else {
                StepComplete();
            }
        }

        void DialogClosed() {
            StepComplete();
        }
    }
}