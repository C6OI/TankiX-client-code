using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Battle.ClientCore.Impl {
    public class CursorStateSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void SetCursor(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser) {
            CursorStateComponent component = new();
            selfBattleUser.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void HideCursor(BattleFullyLoadedEvent e, UserAsTank selfBattleUserAsTank) {
            if (selfBattleUserAsTank.mouseControlStateHolder.MouseControlAllowed) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [OnEventFire]
        public void ShowCursorOnExitFromBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        [OnEventFire]
        public void SwitchCursorState(UpdateEvent evt, Spectator spectator) {
            if (InputManager.GetActionKeyDown(BattleActions.CURSOR)) {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = !Cursor.visible && spectator.mouseControlStateHolder.MouseControlAllowed ? CursorLockMode.Locked : CursorLockMode.None;
                spectator.cursorState.CursorVisible = Cursor.visible;
                spectator.cursorState.CursorLockState = Cursor.lockState;
            }
        }

        [OnEventFire]
        public void SetBattleChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }

        [OnEventFire]
        public void SetCursorWhenCloseChat(NodeRemoveEvent e, SingleNode<BattleChatStateComponent> battleChatState, UserAsTank userAsTank) {
            if (userAsTank.mouseControlStateHolder.MouseControlAllowed) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        [OnEventFire]
        public void SetCursorWhenCloseChat(NodeRemoveEvent e, SingleNode<BattleChatStateComponent> battleChatState, Spectator spectator) {
            Cursor.visible = spectator.cursorState.CursorVisible;
            Cursor.lockState = spectator.cursorState.CursorLockState;
        }

        [OnEventFire]
        public void Update(UpdateEvent e, Spectator spectator) {
            if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE)) {
                Cursor.lockState = spectator.cursorState.CursorLockState;
                Cursor.visible = spectator.cursorState.CursorVisible;
            }
        }

        [OnEventFire]
        public void Update(UpdateEvent e, UserAsTank userAsTank) {
            if (userAsTank.mouseControlStateHolder.MouseControlAllowed) {
                if (InputManager.GetActionKeyDown(BattleActions.SHOW_SCORE)) {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }

                if (InputManager.GetActionKeyUp(BattleActions.SHOW_SCORE)) {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        bool IsErrorScreen() => SceneManager.GetActiveScene() == SceneManager.GetSceneByName(SceneNames.FATAL_ERROR);

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, UserAsTank userAsTank) {
            if (e.IsFocused && userAsTank.mouseControlStateHolder.MouseControlAllowed && !IsErrorScreen()) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [OnEventFire]
        public void OnFocus(ApplicationFocusEvent e, Spectator spectator) {
            if (e.IsFocused && !IsErrorScreen()) {
                Cursor.visible = spectator.cursorState.CursorVisible;
                Cursor.lockState = spectator.cursorState.CursorLockState;
            }
        }

        public class SelfBattleUserNode : Node {
            public MouseControlStateHolderComponent mouseControlStateHolder;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class UserAsTank : SelfBattleUserNode {
            public new SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class Spectator : SelfBattleUserNode {
            public CursorStateComponent cursorState;
            public new SelfBattleUserComponent selfBattleUser;

            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }
    }
}