using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SpectatorHUDModesSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitSpectator(NodeAddedEvent e, SpectatorNode spectator, SingleNode<BattleScreenComponent> screen, SingleNode<SpectatorBattleScreenComponent> specScreen) {
            spectator.Entity.AddComponent(new SpectatorHUDModeComponent(SpectatorHUDMode.Full));
            ScheduleEvent<ChangeHUDModeEvent>(spectator);
        }

        [OnEventFire]
        public void ChangeMode(UpdateEvent evt, ActiveSpectatorNode spectator) {
            if (InputManager.GetKeyDown(KeyCode.Backslash)) {
                SpectatorHUDMode hUDMode = spectator.spectatorHUDMode.HUDMode;
                ChangeHUDModeEvent changeHUDModeEvent = new();
                changeHUDModeEvent.Mode = GetNextMode(hUDMode);
                ChangeHUDModeEvent eventInstance = changeHUDModeEvent;
                ScheduleEvent(eventInstance, spectator);
            }
        }

        [OnEventFire]
        public void ActuateHUDMode(ChangeHUDModeEvent e, ActiveSpectatorNode spectator, [JoinAll] SingleNode<SpectatorBattleScreenComponent> battleSpectatorScreen,
            [JoinAll] SingleNode<BattleScreenComponent> battleScreen, [JoinAll] Optional<SingleNode<HUDWorldSpaceCanvas>> hudWorldspaceCanvas) {
            spectator.spectatorHUDMode.HUDMode = e.Mode;

            if (hudWorldspaceCanvas.IsPresent()) {
                SetGameObjectVisibleByAlpha(hudWorldspaceCanvas.Get().component.gameObject, e.Mode == SpectatorHUDMode.Full);
            }

            SetGameObjectVisibleByAlpha(battleScreen.component.topPanel,
                e.Mode == SpectatorHUDMode.Full || e.Mode == SpectatorHUDMode.WithoutNameplates || e.Mode == SpectatorHUDMode.WithoutMessagesAndChat);

            SetGameObjectVisible(battleScreen.component.combatEventLog, e.Mode == SpectatorHUDMode.Full || e.Mode == SpectatorHUDMode.WithoutNameplates);
            SetGameObjectVisible(battleSpectatorScreen.component.spectatorChat, e.Mode == SpectatorHUDMode.Full || e.Mode == SpectatorHUDMode.WithoutNameplates);

            SetGameObjectVisible(battleSpectatorScreen.component.scoreTable,
                e.Mode == SpectatorHUDMode.Full || e.Mode == SpectatorHUDMode.WithoutNameplates || e.Mode == SpectatorHUDMode.OnlyScoreTable);

            SetGameObjectVisible(battleSpectatorScreen.component.scoreTableShadow,
                e.Mode == SpectatorHUDMode.Full || e.Mode == SpectatorHUDMode.WithoutNameplates || e.Mode == SpectatorHUDMode.OnlyScoreTable);
        }

        SpectatorHUDMode GetNextMode(SpectatorHUDMode current) {
            if (current == SpectatorHUDMode.NoHUD) {
                return SpectatorHUDMode.Full;
            }

            return current + 1;
        }

        void SetGameObjectVisibleByAlpha(GameObject go, bool visible) {
            go.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        }

        void SetGameObjectVisible(GameObject go, bool visible) {
            go.SetActive(visible);
        }

        void SetVisible(CanvasGroup cg, bool visible) {
            cg.alpha = visible ? 1 : 0;
        }

        public class SpectatorNode : Node {
            public SelfBattleUserComponent selfBattleUser;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ActiveSpectatorNode : Node {
            public SpectatorHUDModeComponent spectatorHUDMode;
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
        }
    }
}