using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class XCrystalBonusRewardsSystem : ECSSystem {
        [OnEventFire]
        public void ShowReward(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            Entity reward = resultForClient.PersonalResult.Reward;

            if (reward != null) {
                AddRewardGroup(screen, reward);
                Log.DebugFormat("ShowReward: reward={0}", reward.Id);
                NewEvent<ShowRewardEvent>().Attach(reward).Attach(screen).Schedule();
            }
        }

        static void AddRewardGroup(ScreenNode screen, Entity reward) {
            long key = reward.GetComponent<BattleRewardGroupComponent>().Key;
            screen.Entity.RemoveComponentIfPresent<BattleRewardGroupComponent>();
            screen.Entity.AddComponent(new BattleRewardGroupComponent(key));
        }

        [OnEventFire]
        public void ShowXCrystalReward(ShowRewardEvent e, ScreenNode screen, XCrystalPersonalRewardNode personalReward,
            [JoinBy(typeof(BattleRewardGroupComponent))]
            XCrystalRewardNode reward) {
            Log.DebugFormat("ShowXCrystalReward: reward={0}", personalReward.Entity.Id);
            XCrystalBonusActivationReason activationReason = personalReward.xCrystalBonusPersonalReward.ActivationReason;
            string titleText = reward.xCrystalRewardTextConfig.Title[activationReason];
            string descriptionText = reward.xCrystalRewardTextConfig.Description[activationReason];
            string ribbonLabel = "x" + personalReward.xCrystalBonusPersonalReward.Multiplier;
            List<SpecialOfferItem> list = new();
            list.Add(new SpecialOfferItem(0, reward.xCrystalRewardItemsConfig.SpriteUid, reward.xCrystalRewardItemsConfig.Title, ribbonLabel));
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(titleText, descriptionText, list);
            specialOfferUI.SetUseDiscountButton();
            specialOfferUI.Appear();
        }

        [OnEventFire]
        public void OnBonusUse(NodeRemoveEvent e, ActivePaymentSaleNode sale, [Context] ScreenNode screen) {
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.HideDiscountButton();
        }

        [OnEventFire]
        public void OnBonusRenew(NodeAddedEvent e, ActivePaymentSaleNode sale, ScreenNode screen, XCrystalPersonalRewardNode personalReward,
            [JoinBy(typeof(BattleRewardGroupComponent))]
            XCrystalRewardNode reward) {
            if (sale.activePaymentSale.PersonalXCrystalBonus) {
                BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
                specialOfferUI.ShowDiscountButtonIfXBonus();
            }
        }

        [OnEventFire]
        public void ShowXCrystalsDialog(ShowXCrystalsDialogEvent e, Node any, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            BuyXCrystalsDialogComponent componentInChildren = dialogs.component.GetComponentInChildren<BuyXCrystalsDialogComponent>(true);
            componentInChildren.Show(e.ShowTitle);
        }

        [OnEventFire]
        public void OnBonusUsed(FinishPersonalXCrystalBonusEvent e, SingleNode<SelfUserComponent> user, [JoinAll] ResultsNode results) {
            Entity reward = results.battleResults.ResultForClient.PersonalResult.Reward;
            ScheduleEvent<ChangeRewardUiOnSuccessPaymentEvent>(reward);
        }

        [OnEventFire]
        public void ShowThankYou(ChangeRewardUiOnSuccessPaymentEvent e, XCrystalPersonalRewardNode personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] XCrystalRewardNode reward,
            [JoinAll] ScreenNode screen, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            BuyXCrystalsDialogComponent componentInChildren = dialogs.component.GetComponentInChildren<BuyXCrystalsDialogComponent>(true);
            componentInChildren.Hide();
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowSmile(reward.xCrystalRewardTextConfig.PurchaseText);
        }

        [OnEventFire]
        public void disabeleGoBackWhenXCrystalsDialogActive(NodeAddedEvent e, SingleNode<BuyXCrystalsDialogComponent> dialog, [JoinAll] SingleNode<BackButtonComponent> back) {
            back.component.Disabled = true;
        }

        [OnEventFire]
        public void enableGoBackWhenXCrystalsDialogHide(NodeRemoveEvent e, SingleNode<BuyXCrystalsDialogComponent> dialog, [JoinAll] SingleNode<BackButtonComponent> back) {
            back.component.Disabled = false;
        }

        public class ResultsNode : Node {
            public BattleResultsComponent battleResults;
        }

        public class ScreenNode : Node {
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }

        public class PersonalRewardNode : Node {
            public BattleRewardGroupComponent battleRewardGroup;
            public PersonalBattleRewardComponent personalBattleReward;
        }

        public class XCrystalPersonalRewardNode : PersonalRewardNode {
            public XCrystalBonusPersonalRewardComponent xCrystalBonusPersonalReward;
        }

        public class XCrystalRewardNode : Node {
            public XCrystalRewardItemsConfigComponent xCrystalRewardItemsConfig;
            public XCrystalRewardTextConfigComponent xCrystalRewardTextConfig;
        }

        public class ActivePaymentSaleNode : Node {
            public ActivePaymentSaleComponent activePaymentSale;

            public SelfUserComponent selfUser;
        }
    }
}