using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateBuilderSystem : ECSSystem {
        [OnEventFire]
        public void SetTankAsReadyForTemplateComponent(NodeAddedEvent e, SingleNode<TankSemiActiveStateComponent> tank) {
            tank.Entity.AddComponentIfAbsent<TankReadyForNameplateComponent>();
        }

        [OnEventFire]
        public void SetTankAsReadyForTemplateComponent(NodeAddedEvent e, SingleNode<TankActiveStateComponent> tank) {
            tank.Entity.AddComponentIfAbsent<TankReadyForNameplateComponent>();
        }

        [OnEventFire]
        public void SetTankAsNotReadyForTemplateComponent(NodeAddedEvent e, SingleNode<TankDeadStateComponent> tank) {
            tank.Entity.RemoveComponentIfPresent<TankReadyForNameplateComponent>();
        }

        [OnEventFire]
        public void SetTankAsNotReadyForTemplateComponent(NodeAddedEvent e, SingleNode<TankSpawnStateComponent> tank) {
            tank.Entity.RemoveComponentIfPresent<TankReadyForNameplateComponent>();
        }

        [OnEventFire]
        public void SetTankAsNotReadyForTemplateComponent(NodeAddedEvent e, SingleNode<TankNewStateComponent> tank) {
            tank.Entity.RemoveComponentIfPresent<TankReadyForNameplateComponent>();
        }

        [OnEventFire]
        public void CreateNameplate(NodeAddedEvent e, [Combine] TankNode tank, [Context] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) {
            GameObject gameObject = Object.Instantiate(worldSpaceHUD.component.nameplatePrefab);
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent<NameplatePositionComponent>();
            tank.tankGroup.Attach(entity);
            tank.userGroup.Attach(entity);
            NameplateESMComponent nameplateESMComponent = new();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateDeletionState>();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateAppearanceState>();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateConcealmentState>();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateInvisibilityEffectState>();
            entity.AddComponent(nameplateESMComponent);
            gameObject.transform.SetParent(worldSpaceHUD.component.transform, false);
        }

        [OnEventFire]
        public void ColorizeTeamNameplate(NodeAddedEvent e, [Combine] NameplateNode nameplate, [Context] [JoinByUser] TeamUserNode teamUser,
            [Context] [JoinByTeam] TeamColorNode teamColor, SingleNode<NameplateTeamColorComponent> nameplateTeamColor) {
            switch (teamColor.colorInBattle.TeamColor) {
                case TeamColor.BLUE:
                    nameplate.nameplate.Color = nameplateTeamColor.component.blueTeamColor;
                    nameplate.nameplate.alwaysVisible = true;
                    nameplate.nameplate.AddBlueHealthBar(nameplate.Entity);
                    break;

                case TeamColor.RED:
                    nameplate.nameplate.Color = nameplateTeamColor.component.redTeamColor;
                    nameplate.nameplate.alwaysVisible = false;
                    nameplate.nameplate.AddRedHealthBar(nameplate.Entity);
                    break;
            }
        }

        [OnEventFire]
        public void ColorizeDMNameplate(NodeAddedEvent e, [Combine] NameplateNode nameplate, [Context] [JoinByUser] UserNode user, [JoinByUser] SingleNode<BattleUserComponent> battleUser,
            [JoinByBattle] SingleNode<DMComponent> battle, SingleNode<NameplateTeamColorComponent> nameplateTeamColor) {
            nameplate.nameplate.Color = !battleUser.Entity.HasComponent<SelfBattleUserComponent>() ? nameplateTeamColor.component.redTeamColor
                                            : nameplateTeamColor.component.blueTeamColor;

            nameplate.nameplate.alwaysVisible = false;
            nameplate.nameplate.AddRedHealthBar(nameplate.Entity);
        }

        [OnEventFire]
        public void DeleteNameplate(NodeRemoveEvent e, TankReadyForNameplateNode tank, [JoinByTank] [Combine] NameplateNode nameplate) {
            nameplate.nameplateESM.Esm.ChangeState<NameplateStates.NameplateDeletionState>();
        }

        public class NameplateNode : Node {
            public NameplateComponent nameplate;

            public NameplateESMComponent nameplateESM;

            public NameplatePositionComponent nameplatePosition;

            public TankGroupComponent tankGroup;
            public UserGroupComponent userGroup;
        }

        [Not(typeof(HealthBarComponent))]
        public class NameplateWithoutHealthNode : Node {
            public NameplateComponent nameplate;

            public NameplateESMComponent nameplateESM;

            public NameplatePositionComponent nameplatePosition;

            public TankGroupComponent tankGroup;
            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public AssembledTankComponent assembledTank;

            public HullInstanceComponent hullInstance;

            public RemoteTankComponent remoteTank;

            public TankGroupComponent tankGroup;

            public TankReadyForNameplateComponent tankReadyForNameplate;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserComponent roundUser;

            public UserGroupComponent userGroup;
        }

        public class TeamUserNode : UserNode {
            public TeamGroupComponent teamGroup;
        }

        public class SelfTankNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfTankComponent selfTank;

            public TeamGroupComponent teamGroup;
        }

        public class TeamColorNode : Node {
            public ColorInBattleComponent colorInBattle;

            public TeamGroupComponent teamGroup;
        }

        public class TankReadyForNameplateNode : Node {
            public TankGroupComponent tankGroup;
            public TankReadyForNameplateComponent tankReadyForNameplate;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class TankReadyForNameplateComponent : Component { }
    }
}