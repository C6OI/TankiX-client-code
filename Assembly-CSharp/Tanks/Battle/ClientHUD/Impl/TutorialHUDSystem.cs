using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class TutorialHUDSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void ShowOrHideKeymap(UpdateEvent e, TutorialKeymapNode tutorialKeymap) {
            if (InputManager.GetActionKeyDown(BattleActions.HELP)) {
                tutorialKeymap.tutorialKeymap.Visible = !tutorialKeymap.tutorialKeymap.Visible;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && tutorialKeymap.tutorialKeymap.Visible) {
                tutorialKeymap.tutorialKeymap.Visible = false;
            }
        }

        public class TutorialKeymapNode : Node {
            public TutorialKeymapComponent tutorialKeymap;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;
            public UserExperienceComponent userExperience;
        }
    }
}