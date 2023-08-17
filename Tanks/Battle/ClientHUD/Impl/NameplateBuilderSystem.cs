using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateBuilderSystem : ECSSystem {
        [OnEventFire]
        public void CreateNameplate(NodeAddedEvent e, [Combine] TankNode tank,
            [Context] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) {
            GameObject gameObject = Object.Instantiate(worldSpaceHUD.component.nameplatePrefab);
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            tank.tankGroup.Attach(entity);
            tank.userGroup.Attach(entity);
            entity.AddComponent<NameplatePositionComponent>();
            NameplateESMComponent nameplateESMComponent = new();
            entity.AddComponent(nameplateESMComponent);
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateDeletionState>();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateAppearanceState>();
            nameplateESMComponent.Esm.AddState<NameplateStates.NameplateConcealmentState>();
            gameObject.transform.SetParent(worldSpaceHUD.component.transform, false);
        }

        [OnEventFire]
        public void ColorizeTeamNameplate(NodeAddedEvent e, [Combine] NameplateNode nameplate,
            [JoinByUser] [Context] UserNode user, [Context] [JoinByTeam] TeamColorNode teamColor,
            SingleNode<NameplateTeamColorComponent> namaplateTeamColor) {
            switch (teamColor.teamColor.TeamColor) {
                case TeamColor.BLUE:
                    nameplate.nameplate.Color = namaplateTeamColor.component.blueTeamColor;
                    break;

                case TeamColor.RED:
                    nameplate.nameplate.Color = namaplateTeamColor.component.redTeamColor;
                    break;
            }
        }

        [OnEventFire]
        public void AddHealthBarIfNeeded(NodeAddedEvent e, NameplateWithoutHealthNode nameplate,
            [Context] [JoinByUser] UserNode user, [Context] [JoinByTeam] TeamColorNode team,
            [Context] [JoinByTeam] SelfTankNode selfTank) {
            switch (team.teamColor.TeamColor) {
                case TeamColor.BLUE:
                    nameplate.nameplate.AddBlueHealthBar(nameplate.Entity);
                    break;

                case TeamColor.RED:
                    nameplate.nameplate.AddRedHealthBar(nameplate.Entity);
                    break;
            }

            nameplate.nameplate.alwaysVisible = true;
        }

        [OnEventFire]
        public void DeleteNameplate(NodeRemoveEvent e, SingleNode<TankMovableComponent> tank,
            [JoinByTank] NameplateNode nameplate) =>
            nameplate.nameplateESM.Esm.ChangeState<NameplateStates.NameplateDeletionState>();

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

            public TankMovableComponent tankMovable;

            public UserGroupComponent userGroup;
        }

        public class UserNode : Node {
            public RoundUserComponent roundUser;
            public TeamGroupComponent teamGroup;

            public UserGroupComponent userGroup;
        }

        public class SelfTankNode : Node {
            public SelfTankComponent selfTank;

            public TeamGroupComponent teamGroup;
        }

        public class TeamColorNode : Node {
            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;
        }
    }
}