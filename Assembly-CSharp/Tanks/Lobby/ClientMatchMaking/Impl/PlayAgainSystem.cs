using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientHUD.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientMatchMaking.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayAgainSystem : ECSSystem {
        [OnEventFire]
        public void HideAfterReturn(NodeAddedEvent e, SingleNode<PlayAgainButtonComponent> button, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) {
            button.component.gameObject.SetActive(chosenMode.IsPresent());
        }

        [OnEventFire]
        public void PlayAgain(ButtonClickEvent e, SingleNode<PlayAgainButtonComponent> button, [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) {
            if (chosenMode.IsPresent()) {
                NewEvent<PlayAgainEvent>().Attach(chosenMode.Get()).Schedule();
            }
        }

        [OnEventFire]
        public void CheckModeRestrictions(PlayAgainEvent e, SingleNode<ChosenMatchMackingModeComponent> chosenMode, [JoinAll] ICollection<MatchMakingModeNode> modes,
            [JoinAll] UserNode user, [JoinByUser] MountedHullNode hull, [JoinAll] ButtonNode button) {
            int level = hull.upgradeLevelItem.Level;
            Entity modeEntity = chosenMode.component.ModeEntity;

            if (modeEntity != null) {
                MatchMakingModeRestrictionsComponent component = modeEntity.GetComponent<MatchMakingModeRestrictionsComponent>();
                int rank = user.userRank.Rank;
                e.MatchMackingMode = modeEntity;
                e.ModeIsAvailable = rank <= component.MaximalShowRank && rank >= component.MinimalShowRank;
                MainScreenComponent.Instance.ShowHome();

                if (e.ModeIsAvailable) {
                    button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
                    MainScreenComponent.Instance.ShowMatchSearching(modeEntity.GetComponent<DescriptionItemComponent>().Name);
                } else {
                    MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.PlayScreen);
                }
            }
        }

        [OnEventFire]
        public void SaveChosenMode(SaveBattleModeEvent e, MatchMakingModeNode mode, [JoinAll] ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes) {
            foreach (SingleNode<ChosenMatchMackingModeComponent> mode2 in modes) {
                DeleteEntity(mode2.Entity);
            }

            Entity entity = CreateEntity("ChosenMode");
            ChosenMatchMackingModeComponent chosenMatchMackingModeComponent = new();
            chosenMatchMackingModeComponent.ModeEntity = mode.Entity;
            ChosenMatchMackingModeComponent component = chosenMatchMackingModeComponent;
            entity.AddComponent(component);
        }

        [OnEventFire]
        public void RemoveChosenModes(ExitFromMatchMakingEvent e, Node any, ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes) {
            foreach (SingleNode<ChosenMatchMackingModeComponent> mode in modes) {
                DeleteEntity(mode.Entity);
            }
        }

        [OnEventFire]
        public void RemoveChosenModes(NodeAddedEvent e, SingleNode<CustomBattleLobbyComponent> customBattle, ICollection<SingleNode<ChosenMatchMackingModeComponent>> modes) {
            foreach (SingleNode<ChosenMatchMackingModeComponent> mode in modes) {
                DeleteEntity(mode.Entity);
            }
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserRankComponent userRank;
        }

        public class MountedHullNode : Node {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class MatchMakingModeNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MatchMakingModeComponent matchMakingMode;

            public MatchMakingModeRestrictionsComponent matchMakingModeRestrictions;
        }

        public class ButtonNode : Node {
            public ESMComponent esm;
            public PlayButtonComponent playButton;
        }
    }
}