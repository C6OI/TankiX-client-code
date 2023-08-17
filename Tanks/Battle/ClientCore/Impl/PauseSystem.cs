using System;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PauseSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, UnpausedUserNode user, [JoinByUser] ActiveTankNode tank,
            [JoinByTank] WeaponNode weaponNode) {
            if (InputManager.GetActionKeyDown(BattleActions.PAUSE) &&
                IsNearZero(weaponNode.weaponRotationControl.Control) &&
                IsNearZero(tank.chassis.MoveAxis) &&
                IsNearZero(tank.chassis.TurnAxis)) {
                EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent e, PausedUserNode user) {
            if (IsAnyKeyPressed() || IsMouseMovement()) {
                LeavePause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnApplicationFocusLost(ApplicationFocusEvent e, UnpausedUserNode user,
            [JoinByUser] ActiveTankNode tank) {
            if (!e.IsFocused && ShouldPauseOnFocusLoss()) {
                EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void MarkUserOnApplicationFocusLost(ApplicationFocusEvent e, FocusedUserNode user) {
            if (!e.IsFocused) {
                user.Entity.AddComponent<UnfocusedUserComponent>();
            }
        }

        [OnEventFire]
        public void UnmarkUserOnApplicationFocusReturns(ApplicationFocusEvent e, SingleNode<UnfocusedUserComponent> user) {
            if (e.IsFocused) {
                user.Entity.RemoveComponent<UnfocusedUserComponent>();
            }
        }

        [OnEventFire]
        public void ForcePauseSemiActiveTankUser(NodeAddedEvent e, SemiActiveSelfTankNode tank,
            [JoinByUser] UnpausedUnfocusedUserNode user) {
            if (ShouldPauseOnFocusLoss()) {
                EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void ForcePauseActiveTankUserUser(NodeAddedEvent e, ActiveTankNode tank,
            [JoinByUser] UnpausedUnfocusedUserNode user) {
            if (ShouldPauseOnFocusLoss()) {
                EnterPause(user.Entity);
            }
        }

        [OnEventFire]
        public void OnApplicationFocusEvent(ApplicationFocusEvent e, PausedUserNode user) {
            if (e.IsFocused) {
                LeavePause(user.Entity);
            }
        }

        void LeavePause(Entity user) {
            Log.Info("LeavePause");
            ScheduleEvent<UnpauseEvent>(user);
        }

        void EnterPause(Entity user) {
            Log.Info("EnterPause");
            ScheduleEvent<PauseEvent>(user);
        }

        bool IsMouseMovement() => InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE) != 0f ||
                                  InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE) != 0f ||
                                  InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM) != 0f;

        bool IsAnyKeyPressed() => InputManager.IsAnyKey();

        bool IsNearZero(float value) => Math.Abs(value) < 0.001;

        bool ShouldPauseOnFocusLoss() => !Application.isEditor;

        [Not(typeof(PauseComponent))]
        public class UnpausedUserNode : Node {
            public BattleUserComponent battleUser;

            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        public class PausedUserNode : Node {
            public BattleUserComponent battleUser;

            public PauseComponent pause;

            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SelfDestructionComponent))]
        public class ActiveTankNode : Node {
            public ChassisComponent chassis;

            public SelfTankComponent selfTank;
            public TankComponent tank;

            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;

            public UserGroupComponent userGroup;
        }

        public class WeaponNode : Node {
            public TankGroupComponent tankGroup;

            public WeaponRotationControlComponent weaponRotationControl;
        }

        [Not(typeof(PauseComponent))]
        public class UnpausedUnfocusedUserNode : Node {
            public BattleUserComponent battleUser;

            public UnfocusedUserComponent unfocusedUser;
        }

        [Not(typeof(UnfocusedUserComponent))]
        public class FocusedUserNode : Node {
            public BattleUserComponent battleUser;
        }

        public class SemiActiveSelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;

            public TankSemiActiveStateComponent tankSemiActiveState;
        }
    }
}