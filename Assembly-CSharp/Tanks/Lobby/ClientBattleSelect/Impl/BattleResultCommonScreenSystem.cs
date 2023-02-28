using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientQuests.API;
using Tanks.Lobby.ClientQuests.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultCommonScreenSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void DelayShowBackgroundAndLoadHangar(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [Mandatory] [JoinAll] SelfUserNode user) {
            NewEvent<GoBackFromBattleWithResultsEvent>().Attach(user).ScheduleDelayed(4f);
            NewEvent<LoadHangarEvent>().Attach(user).ScheduleDelayed(3.5f);
        }

        [OnEventFire]
        public void RemoveResults(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] ICollection<SingleNode<BattleResultsComponent>> otherResults) {
            foreach (SingleNode<BattleResultsComponent> otherResult in otherResults) {
                DeleteEntity(otherResult.Entity);
            }
        }

        [OnEventFire]
        public void ShowResultsScreen(GoBackFromBattleWithResultsEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<ShowScreenNoAnimationEvent<BattleResultScreenComponent>>(EngineService.EntityStub);
        }

        [OnEventFire]
        public void CreateResultsEntity(BattleResultForClientEvent e, SelfUserNode user, [JoinAll] ICollection<SingleNode<BattleResultsComponent>> otherResults) {
            UpdateResultEntity(e.UserResultForClient, otherResults);
        }

        void UpdateResultEntity(BattleResultForClient result, ICollection<SingleNode<BattleResultsComponent>> otherResults) {
            foreach (SingleNode<BattleResultsComponent> otherResult in otherResults) {
                DeleteEntity(otherResult.Entity);
            }

            Entity entity = CreateEntity("BattleResults");

            entity.AddComponent(new BattleResultsComponent {
                ResultForClient = result
            });
        }

        [OnEventFire]
        public void ToMainScreen(ButtonClickEvent e, SingleNode<ToMainScreenBattleButtonComponent> button) {
            MainScreenComponent.Instance.ShowHome();
            ScheduleEvent<ShowQuestProgressIfNeedEvent>(button);
        }

        [OnEventFire]
        public void OnContinue(ButtonClickEvent e, SingleNode<ContinueBattleButtonComponent> button) {
            ScheduleEvent<GoBackRequestEvent>(button);
        }

        [OnEventFire]
        public void ShowQuestWindowAfterBattleFinishScreen(ShowQuestProgressIfNeedEvent e, Node any, [JoinAll] SingleNode<MainScreenComponent> mainScreen,
            [JoinAll] SingleNode<WindowsSpaceComponent> screens, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] ICollection<QuestNode> quests) {
            int num = quests.Count(q => !q.questProgress.PrevValue.Equals(q.questProgress.CurrentValue));

            if (num > 0) {
                QuestWindowComponent questWindowComponent = dialogs.component.Get<QuestWindowComponent>();
                questWindowComponent.ShowOnMainScreen = false;
                questWindowComponent.ShowProgress = true;
                questWindowComponent.Show(screens.component.Animators);
            }
        }

        [OnEventFire]
        public void ScreenInit(NodeAddedEvent e, SingleNode<BattleResultCommonUIComponent> battleResultScreenUI, [JoinAll] SelfUserNode selfUserNode,
            [JoinAll] SingleNode<BattleResultsComponent> results) {
            BattleResultForClient resultForClient = results.component.ResultForClient;

            if (resultForClient.Spectator) {
                battleResultScreenUI.component.ShowScreen(resultForClient.Custom, true, false, false, false);
                return;
            }

            bool flag = resultForClient.PersonalResult.MaxEnergySource == EnergySource.MVP_BONUS;
            bool flag2 = resultForClient.PersonalResult.MaxEnergySource == EnergySource.UNFAIR_MM || resultForClient.PersonalResult.MaxEnergySource == EnergySource.DISBALANCE_BONUS;
            GetBattleTypeEvent getBattleTypeEvent = new();
            getBattleTypeEvent.WithCashback = flag || flag2;
            GetBattleTypeEvent getBattleTypeEvent2 = getBattleTypeEvent;
            ScheduleEvent(getBattleTypeEvent2, battleResultScreenUI);

            bool tutor = selfUserNode.userStatistics.Statistics["ALL_BATTLES_PARTICIPATED"] <= 4 ||
                         getBattleTypeEvent2.BattleType == BattleResultsAwardsScreenComponent.BattleTypes.Tutorial;

            bool squad = selfUserNode.Entity.HasComponent<SquadGroupComponent>();
            battleResultScreenUI.component.ShowScreen(resultForClient.Custom, false, tutor, squad, true);
        }

        [OnEventFire]
        public void FillMVPScreen(NodeAddedEvent e, SingleNode<MVPScreenUIComponent> screen, SingleNode<BattleResultsComponent> battleResults,
            [JoinAll] ModuleUpgradeConfigNode moduleUpgradeConfig, [JoinAll] SelfUserNode user) {
            screen.component.SetModuleConfig(moduleUpgradeConfig.moduleUpgradablePowerConfig);
            UserResult userResult = FindMostValuablePlayer(battleResults.component.ResultForClient);
            bool mvpIsPlayer = false;

            if (!battleResults.component.ResultForClient.Spectator) {
                UserResult userResult2 = battleResults.component.ResultForClient.FindUserResultByUserId(user.Entity.Id);
                mvpIsPlayer = userResult.UserId == userResult2.UserId;
            }

            screen.component.SetResults(userResult, battleResults.component.ResultForClient, mvpIsPlayer);
        }

        UserResult FindMostValuablePlayer(BattleResultForClient battleResults) {
            if (battleResults.DmUsers.Count > 0) {
                return battleResults.DmUsers[0];
            }

            if (battleResults.RedUsers.Count == 0) {
                return battleResults.BlueUsers[0];
            }

            if (battleResults.BlueUsers.Count == 0) {
                return battleResults.RedUsers[0];
            }

            if (battleResults.RedUsers[0].ScoreWithoutPremium > battleResults.BlueUsers[0].ScoreWithoutPremium) {
                return battleResults.RedUsers[0];
            }

            return battleResults.BlueUsers[0];
        }

        [OnEventFire]
        public void BuildBestPlayerTank(BuildBestPlayerTankEvent e, Node any, [JoinAll] SingleNode<BattleResultsComponent> battleResults,
            [JoinAll] SingleNode<BattleResultCommonUIComponent> screen) {
            UserResult mvp = FindMostValuablePlayer(battleResults.component.ResultForClient);
            BuildTank(mvp, true, screen.component.tankPreviewImage1);
        }

        [OnEventFire]
        public void BuildSelfTank(BuildSelfPlayerTankEvent e, Node any, [JoinAll] SelfUserNode user, [JoinAll] SingleNode<BattleResultsComponent> battleResults,
            [JoinAll] SingleNode<BattleResultCommonUIComponent> screen) {
            UserResult mvp = battleResults.component.ResultForClient.FindUserResultByUserId(user.Entity.Id);
            BuildTank(mvp, false, screen.component.tankPreviewImage2);
        }

        void BuildTank(UserResult mvp, bool bestPlayerScreen, Image image) {
            string hullGuid = GetHullGuid(mvp);
            string turretGuid = GetTurretGuid(mvp);
            string paintGuid = GetPaintGuid(mvp);
            string coverGuid = GetCoverGuid(mvp);
            BuildBattleResultTankEvent buildBattleResultTankEvent = new();
            buildBattleResultTankEvent.HullGuid = hullGuid;
            buildBattleResultTankEvent.WeaponGuid = turretGuid;
            buildBattleResultTankEvent.PaintGuid = paintGuid;
            buildBattleResultTankEvent.CoverGuid = coverGuid;
            buildBattleResultTankEvent.BestPlayerScreen = bestPlayerScreen;
            BuildBattleResultTankEvent buildBattleResultTankEvent2 = buildBattleResultTankEvent;
            ScheduleEvent(buildBattleResultTankEvent2, EngineService.EntityStub);
            SetImage(image, buildBattleResultTankEvent2);
        }

        static void SetImage(Image image, BuildBattleResultTankEvent buildEvent) {
            image.material.SetTexture("_MainTex", buildEvent.tankPreviewRenderTexture);
            image.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
        }

        static string GetCoverGuid(UserResult mvp) => GetItemGuid(mvp.CoatingId);

        static string GetPaintGuid(UserResult mvp) => GetItemGuid(mvp.PaintId);

        static string GetTurretGuid(UserResult mvp) => GetItemGuid(mvp.WeaponSkinId);

        static string GetHullGuid(UserResult mvp) => GetItemGuid(mvp.HullSkinId);

        static string GetItemGuid(long marketItemId) => GarageItemsRegistry.GetItem<GarageItem>(marketItemId).AssertGuid;

        [OnEventFire]
        public void BattleResultRemoved(NodeRemoveEvent e, SingleNode<BattleResultCommonUIComponent> battleResultUI) {
            ScheduleEvent<ClearBattleResultTankEvent>(EngineService.EntityStub);
        }

        public class ShowQuestProgressIfNeedEvent : Event { }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserStatisticsComponent userStatistics;
        }

        public class ModuleUpgradeConfigNode : Node {
            public ModuleUpgradablePowerConfigComponent moduleUpgradablePowerConfig;
        }

        public class LeagueNode : Node {
            public ChestBattleRewardComponent chestBattleReward;
            public LeagueComponent league;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;
        }

        public class QuestNode : Node {
            public QuestProgressComponent questProgress;
        }
    }
}