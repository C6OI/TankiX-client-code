using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientQuests.API;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class InBattleQuestsScreenSystem : ECSSystem {
        [OnEventFire]
        public void CreateQuest(NodeAddedEvent e, [Combine] SingleNode<BattleQuestComponent> battleQuest, InBattleQuestsContainerNode container) {
            battleQuest.Entity.AddComponent(new BattleQuestGroupComponent(battleQuest.Entity));
            GameObject gameObject = container.inBattleQuestsContainerGUI.CreateQuestItem();
            gameObject.GetComponent<EntityBehaviour>().Entity.AddComponent(new BattleQuestGroupComponent(battleQuest.Entity));
        }

        [OnEventFire]
        public void DeleteQuests(NodeRemoveEvent e, InBattleQuestsContainerNode container) {
            container.inBattleQuestsContainerGUI.DeleteAllQuests();
        }

        [OnEventFire]
        public void FillBattleQuest(NodeAddedEvent e, BattleQuestGuiNode gui, [JoinByBattleQuest] [Context] BattleQuestNode battleQuest, [JoinAll] SingleNode<SelfUserComponent> user,
            [JoinByUser] WeaponNode weapon, [JoinByMarketItem] SingleNode<MarketItemComponent> weaponMarketItem, [JoinAll] SingleNode<SelfUserComponent> user2,
            [JoinByUser] SingleNode<TankComponent> hull, [JoinByMarketItem] SingleNode<MarketItemComponent> hullMarketItem) {
            InBattleQuestItemGUIComponent inBattleQuestItemGUI = gui.inBattleQuestItemGUI;
            inBattleQuestItemGUI.TaskText = battleQuest.descriptionItem.Description.Replace("{TargetValue}", battleQuest.battleQuestTarget.TargetValue.ToString());
            inBattleQuestItemGUI.SetImage(battleQuest.imageItem.SpriteUid);
            inBattleQuestItemGUI.TargetProgressValue = battleQuest.battleQuestTarget.TargetValue.ToString();
            inBattleQuestItemGUI.CurrentProgressValue = "0";
            BattleQuestReward battleQuestReward = battleQuest.battleQuestReward.BattleQuestReward;
            long itemId = battleQuestReward != BattleQuestReward.HULL_EXP ? weaponMarketItem.Entity.Id : hullMarketItem.Entity.Id;
            inBattleQuestItemGUI.SetReward(battleQuestReward, battleQuest.battleQuestReward.Quantity, itemId);
        }

        [OnEventFire]
        public void FillProgress(BattleQuestProgressForClientEvent e, BattleQuestNode battleQuest, [JoinByBattleQuest] BattleQuestGuiNode gui) {
            gui.inBattleQuestItemGUI.CurrentProgressValue = battleQuest.battleQuestProgress.CurrentValue.ToString();
        }

        [OnEventFire]
        public void RemoveBattleQuestGui(NodeRemoveEvent e, BattleQuestNode battleQuest, [JoinByBattleQuest] BattleQuestGuiNode gui) {
            gui.inBattleQuestItemGUI.CompleteQuest();
        }

        public class InBattleQuestsContainerNode : Node {
            public InBattleQuestsContainerGUIComponent inBattleQuestsContainerGUI;
        }

        public class BattleQuestGuiNode : Node {
            public BattleQuestGroupComponent battleQuestGroup;
            public InBattleQuestItemGUIComponent inBattleQuestItemGUI;
        }

        public class BattleQuestNode : Node {
            public BattleQuestComponent battleQuest;

            public BattleQuestProgressComponent battleQuestProgress;

            public BattleQuestRewardComponent battleQuestReward;

            public BattleQuestTargetComponent battleQuestTarget;

            public DescriptionItemComponent descriptionItem;

            public ImageItemComponent imageItem;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;
        }
    }
}