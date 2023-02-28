using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AvatarScreenSystem : ECSSystem {
        [OnEventFire]
        public void RemoveMainScreen(NodeRemoveEvent e, SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<AvatarDialogComponent> avatarDialog) {
            avatarDialog.component.Hide();
        }

        [OnEventFire]
        public void LogTutorialStep(NodeAddedEvent e, SingleNode<TutorialScreenComponent> tutorial, SingleNode<AvatarDialogComponent> dialog) {
            dialog.component.Hide();
        }

        [OnEventFire]
        public void ShowSelfUserTooltip(RightMouseButtonClickEvent e, SelfUserLabelNode selfUserButton, [JoinAll] SelfUserNode selfUser) {
            ShowTooltip(selfUser, selfUser, selfUserButton.squadTeammateInteractionButton);
        }

        public void ShowTooltip(UserNode user, SelfUserNode selfUser, SquadTeammateInteractionButtonComponent squadTeammateInteractionButton) {
            TooltipController.Instance.ShowTooltip(Input.mousePosition, new SquadTeammateInteractionTooltipContentData(), squadTeammateInteractionButton.tooltipPrefab, false);
        }

        [Not(typeof(UserLabelComponent))]
        public class SelfUserLabelNode : Node {
            public SquadTeammateInteractionButtonComponent squadTeammateInteractionButton;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SquadGroupComponent))]
        public class SelfUserNode : UserNode {
            public SelfUserComponent selfUser;
        }
    }
}