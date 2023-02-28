using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl.Tutorial {
    public class ModulesTutorialSystem : ECSSystem {
        public static bool tutorialActive;

        [OnEventFire]
        public void StepComplete(NodeAddedEvent e, TutorialStepNode stepNode, [JoinByTutorial] TutorialNode tutorialNode, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            if (tutorialActive && tutorialNode.tutorialData.TutorialId.Equals(-1229949270L) && stepNode.tutorialStepData.StepId.Equals(1525362180L)) {
                Debug.Log("Bingo");
                dialogs.component.Get<NewModulesScreenUIComponent>().Show(TankPartModuleType.WEAPON);
            }
        }

        public class TutorialStepNode : Node {
            public TutorialGroupComponent tutorialGroup;

            public TutorialStepCompleteComponent tutorialStepComplete;
            public TutorialStepDataComponent tutorialStepData;
        }

        public class TutorialNode : Node {
            public TutorialDataComponent tutorialData;

            public TutorialGroupComponent tutorialGroup;
        }
    }
}