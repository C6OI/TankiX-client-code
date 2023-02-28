using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientQuests.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestsRewardBufferSystem : ECSSystem {
        [OnEventFire]
        public void DeleteQuestRewardFromBuffer(QuestsScreenSystem.TryShowQuestRewardNotification e, RewardedNewQuestNode questNode, [JoinAll] SelfUserNode user,
            [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals) {
            DeleteFromBuffer(questNode.questReward.Reward, user, crystals, xCrystals);
        }

        [OnEventFire]
        public void DeleteQuestRewardFromBuffer(QuestsScreenSystem.TryShowQuestRewardNotification e, RewardedOldQuestNode questNode, [JoinAll] SelfUserNode user,
            [JoinAll] CrystalMarketItemNode crystals, [JoinAll] XCrystalMarketItemNode xCrystals) {
            QuestParameters questParameters = questNode.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(questNode.userRank.Rank));
            DeleteFromBuffer(questParameters.QuestReward, user, crystals, xCrystals);
        }

        void DeleteFromBuffer(Dictionary<long, int> reward, SelfUserNode user, CrystalMarketItemNode crystals, XCrystalMarketItemNode xCrystals) {
            int itemCount = GetItemCount(reward, crystals.Entity.Id);
            int itemCount2 = GetItemCount(reward, xCrystals.Entity.Id);

            NewEvent(new ChangeUserMoneyBufferEvent {
                Crystals = -itemCount,
                XCrystals = -itemCount2
            }).Attach(user).Schedule();
        }

        [OnEventFire]
        public void AddQuestRewardToBuffer(NodeAddedEvent e, RewardedNotResultNewQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals,
            [JoinAll] XCrystalMarketItemNode xCrystals) {
            AddToBuffer(questNode.questReward.Reward, user, crystals, xCrystals);
        }

        [OnEventFire]
        public void AddQuestRewardToBuffer(NodeAddedEvent e, RewardedNotResultOldQuestNode questNode, [JoinAll] SelfUserNode user, [JoinAll] CrystalMarketItemNode crystals,
            [JoinAll] XCrystalMarketItemNode xCrystals) {
            QuestParameters questParameters = questNode.questVariations.Quests.Find(r => IndexRange.ParseString(r.Range).Contains(questNode.userRank.Rank));
            AddToBuffer(questParameters.QuestReward, user, crystals, xCrystals);
        }

        void AddToBuffer(Dictionary<long, int> reward, SelfUserNode user, CrystalMarketItemNode crystals, XCrystalMarketItemNode xCrystals) {
            int itemCount = GetItemCount(reward, crystals.Entity.Id);
            int itemCount2 = GetItemCount(reward, xCrystals.Entity.Id);

            NewEvent(new ChangeUserMoneyBufferEvent {
                Crystals = itemCount,
                XCrystals = itemCount2
            }).Attach(user).Schedule();
        }

        int GetItemCount(Dictionary<long, int> items, long itemId) => items != null && items.ContainsKey(itemId) ? items[itemId] : 0;

        public class SelfUserNode : Node {
            public SelfComponent self;
            public UserComponent user;

            public UserMoneyComponent userMoney;
        }

        public class RewardedQuestNode : Node {
            public QuestComponent quest;

            public QuestOrderComponent questOrder;

            public QuestProgressComponent questProgress;

            public RewardedQuestComponent rewardedQuest;
        }

        public class RewardedNewQuestNode : RewardedQuestNode {
            public QuestRewardComponent questReward;
        }

        [Not(typeof(QuestResultComponent))]
        public class RewardedNotResultNewQuestNode : RewardedNewQuestNode { }

        public class RewardedOldQuestNode : RewardedQuestNode {
            public QuestVariationsComponent questVariations;
            public UserRankComponent userRank;
        }

        [Not(typeof(QuestResultComponent))]
        public class RewardedNotResultOldQuestNode : RewardedOldQuestNode { }

        public class CrystalMarketItemNode : Node {
            public CrystalItemComponent crystalItem;

            public MarketItemComponent marketItem;
        }

        public class XCrystalMarketItemNode : Node {
            public MarketItemComponent marketItem;
            public XCrystalItemComponent xCrystalItem;
        }
    }
}