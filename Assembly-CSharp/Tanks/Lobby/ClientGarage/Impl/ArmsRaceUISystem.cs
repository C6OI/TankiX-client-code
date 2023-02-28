using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ArmsRaceUISystem : ECSSystem {
        [OnEventFire]
        public void ShowArmsRaceIntro(ShowArmsRaceIntroEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] UserNode user) {
            ArmsRaceIntroDialog armsRaceIntroDialog = dialogs.component.Get<ArmsRaceIntroDialog>();
            armsRaceIntroDialog.screen1.SetActive(true);
            armsRaceIntroDialog.screen2.SetActive(false);

            if (user.userExperience.Experience <= 800) {
                armsRaceIntroDialog.container.SetActive(false);
            }

            e.Dialog = armsRaceIntroDialog;
            armsRaceIntroDialog.Show(new List<Animator>());
        }

        [OnEventFire]
        public void HideArmsRaceIntro(ButtonClickEvent e, SingleNode<ArmsRaceIntroCloseButtonCompoent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.Get<ArmsRaceIntroDialog>().Hide();
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserExperienceComponent userExperience;
        }
    }
}