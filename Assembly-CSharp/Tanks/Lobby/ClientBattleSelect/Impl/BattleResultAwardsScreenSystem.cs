using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientQuests.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultAwardsScreenSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void ScreenInit(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results, [JoinAll] SelfUserNode selfUser, [JoinAll] ICollection<QuestNode> quests) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            bool flag = personalResult.MaxEnergySource == EnergySource.MVP_BONUS;
            bool flag2 = personalResult.MaxEnergySource == EnergySource.UNFAIR_MM || personalResult.MaxEnergySource == EnergySource.DISBALANCE_BONUS;
            GetBattleTypeEvent getBattleTypeEvent = new();
            getBattleTypeEvent.WithCashback = flag || flag2;
            GetBattleTypeEvent getBattleTypeEvent2 = getBattleTypeEvent;
            ScheduleEvent(getBattleTypeEvent2, screen);
            screen.battleResultsAwardsScreen.SetBattleType(getBattleTypeEvent2.BattleType);
            int userDMPlace = resultForClient.BattleMode == BattleMode.DM ? resultForClient.DmUsers.IndexOf(resultForClient.FindUserResultByUserId(selfUser.Entity.Id)) + 1 : 0;
            ShowTitle(screen, results, userDMPlace);
            ShowReputation(screen, results, selfUser);
        }

        [OnEventFire]
        public void AttachScreenToUserGroup(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelfUserNode selfUser) {
            selfUser.userGroup.Attach(screen.Entity);
        }

        [OnEventFire]
        public void PutReputationToEnter(UpdateTopLeagueInfoEvent e, SelfUserNode user, [JoinAll] ScreenNode screen, [JoinAll] TopLeagueNode topLeague, [JoinAll] ResultsNode results) {
            if (!results.battleResults.ResultForClient.Spectator && screen.battleResultsAwardsScreen.CanShowLeagueProgress()) {
                screen.battleResultsAwardsScreen.leagueResultUI.PutReputationToEnter(topLeague.Entity.Id, e.LastPlaceReputation);
            }
        }

        public void ShowTitle(ScreenNode screen, ResultsNode results, int userDMPlace) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(resultForClient.MapId);
            string name = entity.GetComponent<DescriptionItemComponent>().Name;
            BattleMode battleMode = resultForClient.BattleMode;
            BattleType matchMakingModeType = resultForClient.MatchMakingModeType;
            TeamBattleResult teamBattleResult = personalResult.TeamBattleResult;
            screen.battleResultsAwardsScreen.SetupHeader(battleMode, matchMakingModeType, teamBattleResult, name, userDMPlace);
        }

        public void ShowReputation(ScreenNode screen, ResultsNode results, SelfUserNode user) {
            if (!screen.battleResultsAwardsScreen.CanShowLeagueProgress()) {
                screen.battleResultsAwardsScreen.HideLeagueProgress();
                return;
            }

            screen.battleResultsAwardsScreen.ShowLeagueProgress();
            LeagueResultUI leagueResultUI = screen.battleResultsAwardsScreen.leagueResultUI;
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            UserResult userResult = resultForClient.FindUserResultByUserId(user.Entity.Id);
            Entity prevLeague = personalResult.PrevLeague;
            Entity league = personalResult.League;
            double reputation = personalResult.Reputation;
            double reputationDelta = personalResult.ReputationDelta;
            bool topLeague = league.HasComponent<TopLeagueComponent>();
            int leaguePlace = personalResult.LeaguePlace;
            bool unfairMatching = userResult.UnfairMatching;

            if (prevLeague != league) {
                leagueResultUI.SetPreviousLeague(prevLeague);
            }

            leagueResultUI.SetCurrentLeague(league, reputation, leaguePlace, topLeague, reputationDelta, unfairMatching);
            leagueResultUI.DealWithReputationChange();

            if (prevLeague != league) {
                leagueResultUI.ShowNewLeague();
            }
        }

        [OnEventFire]
        public void ShowExp(NodeAddedEvent e, ScreenNode screen, [JoinAll] ResultsNode results, [JoinAll] SelfUserNode selfUser, [JoinByLeague] LeagueNode league,
            [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] RankNamesNode rankNames, [JoinAll] SingleNode<RanksExperiencesConfigComponent> ranksExperiencesConfig) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            PersonalBattleResultForClient personalResult = resultForClient.PersonalResult;
            UserResult userResult = resultForClient.FindUserResultByUserId(selfUser.Entity.Id);
            int rank = selfUser.userRank.Rank;
            int num = rank - 2;
            int initValue = num >= 0 && num < ranksExperiencesConfig.component.RanksExperiences.Length ? ranksExperiencesConfig.component.RanksExperiences[num] : 0;
            int num2 = rank - 1;
            int maxValue = num2 >= 0 && num2 < ranksExperiencesConfig.component.RanksExperiences.Length ? ranksExperiencesConfig.component.RanksExperiences[num2] : 0;

            screen.battleResultsAwardsScreen.ShowRankProgress(initValue, personalResult.RankExp, maxValue, personalResult.RankExpDelta, userResult.ScoreWithoutPremium, rank,
                rankNames.ranksNames.Names);

            Entity entity = Flow.Current.EntityRegistry.GetEntity(league.chestBattleReward.ChestId);
            GamePlayChestItemNode gamePlayChestItemNode = Select<GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault();
            screen.battleResultsAwardsScreen.openChestButton.SetActive(gamePlayChestItemNode != null && gamePlayChestItemNode.userItemCounter.Count != 0);

            screen.battleResultsAwardsScreen.ShowContainerProgress(personalResult.ContainerScore, personalResult.ContainerScoreDelta, userResult.ScoreWithoutPremium,
                personalResult.ContainerScoreLimit,
                personalResult.Container == null || !personalResult.Container.HasComponent<ImageItemComponent>() ? string.Empty
                    : personalResult.Container.GetComponent<ImageItemComponent>().SpriteUid);

            screen.battleResultsAwardsScreen.SetTankInfo(userResult.HullId, userResult.WeaponId, userResult.Modules, moduleUpgradeConfig.moduleUpgradablePowerConfig);

            screen.battleResultsAwardsScreen.SetHullExp(personalResult.TankInitExp, personalResult.TankExp, personalResult.TankFinalExp, personalResult.ItemsExpDelta,
                userResult.ScoreWithoutPremium, personalResult.TankLevel);

            screen.battleResultsAwardsScreen.SetTurretExp(personalResult.WeaponInitExp, personalResult.WeaponExp, personalResult.WeaponFinalExp, personalResult.ItemsExpDelta,
                userResult.ScoreWithoutPremium, personalResult.WeaponLevel);
        }

        [OnEventFire]
        public void OpenGameplayChest(ButtonClickEvent e, SingleNode<OpenGameplayChestButtonComponent> button, [JoinAll] UserNode user, [JoinByLeague] LeagueNode league,
            [JoinAll] ScreenNode screen) {
            screen.battleResultsAwardsScreen.openChestButton.SetActive(false);
            Entity entity = Flow.Current.EntityRegistry.GetEntity(league.chestBattleReward.ChestId);
            GamePlayChestItemNode gamePlayChestItemNode = Select<GamePlayChestItemNode>(entity, typeof(MarketItemGroupComponent)).FirstOrDefault();

            if (gamePlayChestItemNode != null && gamePlayChestItemNode.userItemCounter.Count != 0) {
                ScheduleEvent(new OpenContainerEvent {
                    Amount = gamePlayChestItemNode.userItemCounter.Count
                }, gamePlayChestItemNode);
            }
        }

        [OnEventFire]
        public void TutorialsTriggered(GetBattleTypeEvent e, Node any, [JoinAll] [Combine] SingleNode<TutorialEndGameTriggerComponent> tutorialTrigger) {
            tutorialTrigger.component.GetComponent<TutorialShowTriggerComponent>().Triggered();
        }

        [OnEventComplete]
        public void GetBattleType(GetBattleTypeEvent e, Node any, [JoinAll] ResultsNode results, [JoinAll] ICollection<SingleNode<TutorialSetupEndgameScreenHandler>> tutorialHandlers) {
            BattleResultForClient resultForClient = results.battleResults.ResultForClient;
            BattleResultsAwardsScreenComponent.BattleTypes battleType = BattleResultsAwardsScreenComponent.BattleTypes.None;

            if (resultForClient.Custom) {
                battleType = BattleResultsAwardsScreenComponent.BattleTypes.Custom;
            } else if (tutorialHandlers.Count > 0) {
                foreach (SingleNode<TutorialSetupEndgameScreenHandler> tutorialHandler in tutorialHandlers) {
                    tutorialHandler.component.gameObject.SetActive(false);
                }

                battleType = BattleResultsAwardsScreenComponent.BattleTypes.Tutorial;
            } else {
                switch (resultForClient.MatchMakingModeType) {
                    case BattleType.ENERGY:
                        battleType = BattleResultsAwardsScreenComponent.BattleTypes.Quick;
                        break;

                    case BattleType.ARCADE:
                        battleType = BattleResultsAwardsScreenComponent.BattleTypes.Arcade;
                        break;

                    case BattleType.RATING:
                        battleType = !e.WithCashback ? BattleResultsAwardsScreenComponent.BattleTypes.Ranked : BattleResultsAwardsScreenComponent.BattleTypes.RankedWithCashback;
                        break;
                }
            }

            e.BattleType = battleType;
        }

        [OnEventFire]
        public void ShowTutorialRewards(ShowTutorialRewardsEvent e, TutorialStepWithRewardsNode tutorialStepWithRewards, [JoinAll] ScreenNode screen) {
            List<SpecialOfferItem> list = new();

            foreach (Reward reward in tutorialStepWithRewards.tutorialRewardData.Rewards) {
                GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(reward.ItemId);

                if (item != null) {
                    list.Add(new SpecialOfferItem((int)reward.Count, item.Preview, item.Name));
                }
            }

            long crysCount = tutorialStepWithRewards.tutorialRewardData.CrysCount;

            if (crysCount > 0) {
                list.Add(new SpecialOfferItem((int)crysCount, screen.battleResultsAwardsScreen.crysImageSkin.SpriteUid, screen.battleResultsAwardsScreen.crysLocalizedField.Value));
            }

            BattleResultSpecialOfferUiComponent specialOfferUI = screen.battleResultsAwardsScreen.specialOfferUI;
            specialOfferUI.ShowContent(screen.battleResultsAwardsScreen.tutorialCongratulationLocalizedField.Value, tutorialStepWithRewards.tutorialStepData.Message, list);
            specialOfferUI.SetTutorialRewardsButton();
            specialOfferUI.Appear();
        }

        [OnEventFire]
        public void QuickGameCheck(CheckForQuickGameEvent e, Node any, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) {
            if (chosenMode.IsPresent()) {
                e.IsQuickGame = chosenMode.Get().component.ModeEntity.HasComponent<MatchMakingEnergyModeComponent>();
            }
        }

        [OnEventFire]
        public void ScreenPartShown(ScreenPartShownEvent e, Node any, [JoinAll] ScreenNode screen, [JoinAll] UserNode user) {
            ShowBattleResultsScreenNotificationEvent showBattleResultsScreenNotificationEvent = new();
            showBattleResultsScreenNotificationEvent.Index = 1;
            NewEvent(showBattleResultsScreenNotificationEvent).Attach(any).ScheduleDelayed(0.3f);
        }

        [OnEventComplete]
        public void ContinueOnProgressShow(ShowBattleResultsScreenNotificationEvent e, Node any, [JoinAll] ScreenNode screen) {
            if (!e.NotificationExist) {
                screen.battleResultAwardsScreenAnimation.playActions = true;
            }
        }

        public class ScreenNode : Node {
            public BattleResultAwardsScreenAnimationComponent battleResultAwardsScreenAnimation;
            public BattleResultsAwardsScreenComponent battleResultsAwardsScreen;
        }

        public class ResultsNode : Node {
            public BattleResultsComponent battleResults;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserExperienceComponent userExperience;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;

            public UserStatisticsComponent userStatistics;
        }

        public class TopLeagueNode : Node {
            public TopLeagueComponent topLeague;
        }

        public class ModuleUpgradeConfigNode : Node {
            public ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig;
        }

        public class GamePlayChestItemNode : Node {
            public GameplayChestItemComponent gameplayChestItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class LeagueNode : Node {
            public ChestBattleRewardComponent chestBattleReward;
            public LeagueComponent league;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;
        }

        public class UserNode : Node {
            public EarnedGameplayChestScoreComponent earnedGameplayChestScore;

            public GameplayChestScoreComponent gameplayChestScore;

            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserLeaguePlaceComponent userLeaguePlace;

            public UserReputationComponent userReputation;
        }

        public class TutorialStepWithRewardsNode : Node {
            public TutorialGroupComponent tutorialGroup;

            public TutorialRewardDataComponent tutorialRewardData;
            public TutorialStepDataComponent tutorialStepData;
        }

        public class QuestNode : Node {
            public QuestProgressComponent questProgress;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RankNamesNode : Node {
            public RanksNamesComponent ranksNames;
        }
    }
}