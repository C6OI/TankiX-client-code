using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Battle.ClientCore.API {
    public class InBattlesTutorialHandlersSystem : ECSSystem {
        [OnEventFire]
        public void CheckForBattleResultReward(NodeAddedEvent e, SingleNode<BattleResultRewardCheckComponent> node, [JoinAll] ICollection<TutorialNode> tutorials,
            [JoinAll] SelfBattleUserNode selfBattleUser, [JoinByUser] SelfRoundUser selfRoundUser) {
            CheckForQuickGameEvent checkForQuickGameEvent = new();
            ScheduleEvent(checkForQuickGameEvent, node);

            if (!checkForQuickGameEvent.IsQuickGame) {
                return;
            }

            long quickBattleEndTutorialId = node.component.QuickBattleEndTutorialId;
            bool flag = false;

            foreach (TutorialNode tutorial in tutorials) {
                if (tutorial.tutorialData.TutorialId == quickBattleEndTutorialId && !tutorial.Entity.HasComponent<TutorialCompleteComponent>()) {
                    flag = true;
                    break;
                }
            }

            if (flag) {
                ScheduleEvent<TutorialTriggeredEvent>(selfRoundUser);
            }
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class SelfRoundUser : Node {
            public BattleGroupComponent battleGroup;
            public RoundUserStatisticsComponent roundUserStatistics;

            public UserGroupComponent userGroup;
        }

        public class TutorialNode : Node {
            public TutorialDataComponent tutorialData;

            public TutorialGroupComponent tutorialGroup;

            public TutorialRequiredCompletedTutorialsComponent tutorialRequiredCompletedTutorials;
        }
    }
}