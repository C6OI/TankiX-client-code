using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientQuests.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class ChangeQuestSystem : ECSSystem {
        [OnEventFire]
        public void ChangeQuests(ChangeQuestEvent e, QuestNode quest, [JoinByUser] QuestBonusNode bonus) {
            NewEvent<UseBonusEvent>().Attach(bonus).Attach(quest).Schedule();
        }

        [Not(typeof(TakenBonusComponent))]
        public class QuestBonusNode : Node {
            public QuestExchangeBonusComponent questExchangeBonus;
            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public LeagueGroupComponent leagueGroup;

            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class QuestNode : Node {
            public QuestComponent quest;

            public QuestRarityComponent questRarity;

            public UserGroupComponent userGroup;
        }
    }
}