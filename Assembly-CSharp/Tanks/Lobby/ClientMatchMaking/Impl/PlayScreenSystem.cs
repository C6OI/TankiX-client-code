using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitMode(NodeAddedEvent e, PlayScreenNode screen, RatingModeNode mode) {
            GameObject ratingModeItemPrefab = screen.playScreen.RatingModeItemPrefab;
            CreateActiveModeInstance(mode.Entity, ratingModeItemPrefab, screen.playScreen.RatingModeContainer);
        }

        void CreateActiveModeInstance(Entity mode, GameObject prefab, GameObject container) {
            CreateModeInstance(mode, prefab, container).transform.SetAsFirstSibling();
        }

        GameObject CreateModeInstance(Entity mode, GameObject prefab, GameObject container) {
            GameObject gameObject = Object.Instantiate(prefab);
            gameObject.transform.SetParent(container.transform, false);
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            component.BuildEntity(mode);
            return gameObject;
        }

        [OnEventFire]
        public void RemoveModes(NodeRemoveEvent e, PlayScreenNode screen, [JoinAll] RatingModeGUINode gameMode) {
            Object.Destroy(gameMode.gameModeSelectButton.gameObject);
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, PlayScreenNode screen, SingleNode<PlayScreenSeasonGUIComponent> seasonGUI, [JoinAll] LeaguesConfigNode leaguesConfig) {
            if (string.IsNullOrEmpty(leaguesConfig.currentSeasonName.SeasonName)) {
                seasonGUI.component.SetSeasonNameFromNumber(leaguesConfig.currentSeasonNumber.SeasonNumber - 1);
            } else {
                seasonGUI.component.SetSeasonName(leaguesConfig.currentSeasonName.SeasonName);
            }

            seasonGUI.component.EndDate = leaguesConfig.seasonEndDate.EndDate;
        }

        [OnEventFire]
        public void SetLeagueInfo(NodeAddedEvent e, SingleNode<RankedBattleGUIComponent> uiNode, [JoinAll] SelfUserNode user, [JoinByLeague] [Context] LeagueNode league) {
            uiNode.component.SetLeague(league.leagueName.Name, league.leagueIcon.SpriteUid);
        }

        [OnEventFire]
        public void SetChestProgression(NodeAddedEvent e, PlayScreenNode screen, SingleNode<ChestProgressBarComponent> energygui, [JoinAll] SelfUserNode user,
            [JoinSelf] SingleNode<GameplayChestScoreComponent> chestScore) {
            energygui.component.SetProgress(chestScore.component.Current, chestScore.component.Limit);
        }

        [OnEventFire]
        public void SetChestProgression(NodeAddedEvent e, PlayScreenNode screen, SingleNode<ChestProgressBarComponent> energygui, [JoinAll] SelfUserNode user,
            [JoinSelf] SingleNode<GameplayChestScoreComponent> chestScore, [JoinByLeague] LeaguemNode league) {
            long chestId = league.chestBattleReward.ChestId;
            Entity entityById = GetEntityById(chestId);
            DescriptionItemComponent component = entityById.GetComponent<DescriptionItemComponent>();
            ImageItemComponent component2 = entityById.GetComponent<ImageItemComponent>();
            energygui.component.SetChest(component.Name, component2.SpriteUid);
            energygui.component.SetChestTooltip(chestScore.component.Limit, league.leagueConfig.LeagueIndex > 2);
        }

        public class SelfUserNode : Node {
            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class LeaguemNode : Node {
            public ChestBattleRewardComponent chestBattleReward;
            public LeagueConfigComponent leagueConfig;
        }

        public class LeagueNode : Node {
            public LeagueComponent league;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;

            public LeagueNameComponent leagueName;
        }

        public class LeaguesConfigNode : Node {
            public CurrentSeasonNameComponent currentSeasonName;

            public CurrentSeasonNumberComponent currentSeasonNumber;
            public SeasonEndDateComponent seasonEndDate;
        }

        public class PlayScreenNode : Node {
            public PlayScreenComponent playScreen;
        }

        public class RatingModeNode : Node {
            public MatchMakingRatingModeComponent matchMakingRatingMode;
        }

        public class QuickModeNode : Node {
            public MatchMakingEnergyModeComponent matchMakingEnergyMode;
        }

        public class RatingModeGUINode : RatingModeNode {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }
    }
}