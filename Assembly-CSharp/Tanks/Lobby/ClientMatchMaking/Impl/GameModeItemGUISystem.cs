using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class GameModeItemGUISystem : ECSSystem {
        [OnEventFire]
        public void InitName(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode) {
            gameModeItemNode.gameModeSelectButton.GameModeTitle = gameModeItemNode.descriptionItem.Name;
        }

        [OnEventFire]
        public void InitDescription(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode) {
            gameModeItemNode.gameModeSelectButton.ModeDescription = gameModeItemNode.descriptionItem.Description;
        }

        [OnEventFire]
        public void InitImage(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode) {
            gameModeItemNode.gameModeSelectButton.SetImage(gameModeItemNode.imageItem.SpriteUid);
        }

        [OnEventFire]
        public void BlockMode(NodeAddedEvent e, MatchMakingModeNode gameModeItemNode) {
            if (!gameModeItemNode.matchMakingModeActivation.Active) {
                gameModeItemNode.gameModeSelectButton.SetInactive();
            }
        }

        void DelayUpdate(Node modeItem) {
            NewEvent<UpdateMatchMakingModeEvent>().Attach(modeItem).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateGameModeItem(UpdateMatchMakingModeEvent e, MatchMakingModeNode matchMakingModeNode, [JoinAll] SelfUserNode user, [JoinAll] RanksNamesNode ranksNames,
            [JoinAll] Optional<SelfUserSquadNode> selfUserSquadNode, [JoinBySquad] ICollection<UserInSquadNode> allTeammates,
            [JoinAll] Optional<SelfUserLeaderSquadNode> selfUserLeaderNode) {
            bool userInSquadNow = selfUserSquadNode.IsPresent() && allTeammates.Count > 0;
            bool userIsSquadLeader = selfUserLeaderNode.IsPresent();
            bool modeIsDefault = matchMakingModeNode.Entity.HasComponent<MatchMakingTrainingModeComponent>();

            if (matchMakingModeNode.Entity.HasComponent<RankRestrictionGUIComponent>() && user.userRank.Rank < matchMakingModeNode.matchMakingModeRestrictions.MinimalRank) {
                RankRestrictionGUIComponent component = matchMakingModeNode.Entity.GetComponent<RankRestrictionGUIComponent>();
                matchMakingModeNode.gameModeSelectButton.SetRestricted(true);
                component.SetRank(matchMakingModeNode.matchMakingModeRestrictions.MinimalRank);
                component.RankName = ranksNames.ranksNames.Names[matchMakingModeNode.matchMakingModeRestrictions.MinimalRank];
            }

            matchMakingModeNode.gameModeSelectButton.SetAvailableForSquadMode(userInSquadNow, userIsSquadLeader, modeIsDefault);
        }

        [OnEventFire]
        public void RestrictMode(NodeAddedEvent e, MatchMakingModeWithRankRestrictionNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void RestrictMode(NodeAddedEvent e, MatchMakingModeWithEnergyRestrictionNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void TeammateAdded(NodeAddedEvent e, [Combine] UserInSquadNode teammate, [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void TeammateRemoved(NodeRemoveEvent e, UserInSquadNode teammate, [JoinAll] [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserInSuad(NodeAddedEvent e, SelfUserSquadNode selfUserSquad, [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SquadRemoved(NodeRemoveEvent e, SelfUserSquadNode selfUserSquad, [JoinAll] [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserIsSquadLeader(NodeAddedEvent e, SelfUserLeaderSquadNode selfUserLeaderSquadNode, [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void SelfUserIsNotSquadLeader(NodeRemoveEvent e, SelfUserLeaderSquadNode selfUserLeaderSquadNode, [JoinAll] [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        [OnEventFire]
        public void DefaultModeInited(NodeAddedEvent e, MatchMakingTrainingModeNode matchMakingTrainingMode) {
            DelayUpdate(matchMakingTrainingMode);
        }

        [OnEventFire]
        public void TutorialComplete(NodeAddedEvent e, TutorialCompleteNode tutorialComplete, [JoinAll] [Combine] MatchMakingModeNode gameModeItemNode) {
            DelayUpdate(gameModeItemNode);
        }

        public class SelfUserNode : Node {
            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class GameModeNode : Node {
            public GameModeSelectButtonComponent gameModeSelectButton;
        }

        public class MatchMakingModeNode : GameModeNode {
            public DescriptionItemComponent descriptionItem;

            public ImageItemComponent imageItem;
            public MatchMakingModeComponent matchMakingMode;

            public MatchMakingModeActivationComponent matchMakingModeActivation;

            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
        }

        public class MatchMakingModeWithRankRestrictionNode : MatchMakingModeNode {
            public RankRestrictionGUIComponent rankRestrictionGUI;
        }

        public class MatchMakingModeWithEnergyRestrictionNode : MatchMakingModeNode {
            public EnergyRestrictionGUIComponent energyRestrictionGUI;
        }

        public class MatchMakingTrainingModeNode : MatchMakingModeNode {
            public MatchMakingTrainingModeComponent matchMakingTrainingMode;
        }

        [Not(typeof(SelfUserComponent))]
        public class UserInSquadNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfUserSquadNode : Node {
            public SelfUserComponent selfUser;

            public SquadGroupComponent squadGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfUserLeaderSquadNode : Node {
            public SelfUserComponent selfUser;

            public SquadGroupComponent squadGroup;

            public SquadLeaderComponent squadLeader;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class TutorialCompleteNode : Node {
            public TutorialCompleteComponent tutorialComplete;
            public TutorialDataComponent tutorialData;
        }

        [Not(typeof(UserNotificatorRankNamesComponent))]
        public class RanksNamesNode : Node {
            public RanksNamesComponent ranksNames;
        }

        public class UpdateMatchMakingModeEvent : Event { }
    }
}