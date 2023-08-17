using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildFlag(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map,
            [Combine] FlagNode flag, [JoinByTeam] TeamNode teamNode) {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            TeamColor teamColor = teamNode.teamColor.TeamColor;
            GameObject original = teamColor != TeamColor.RED ? assetProxyBehaviour.blueFlag : assetProxyBehaviour.redFlag;
            FlagInstanceComponent flagInstanceComponent = new();
            Vector3 position = flag.flagPosition.Position;

            GameObject gameObject2 = flagInstanceComponent.FlagInstance =
                                         Object.Instantiate(original, position, Quaternion.identity);

            flag.Entity.AddComponent(flagInstanceComponent);
            FlagPhysicsBehaviour flagPhysicsBehaviour = gameObject2.AddComponent<FlagPhysicsBehaviour>();
            flagPhysicsBehaviour.TriggerEntity = flag.Entity;
            flag.Entity.AddComponent(new FlagColliderComponent(gameObject2.GetComponent<BoxCollider>()));
        }

        static CTFAssetProxyBehaviour GetAssetProxyBehaviour(BattleNode ctf) =>
            ((GameObject)ctf.resourceData.Data).GetComponent<CTFAssetProxyBehaviour>();

        [OnEventFire]
        public void BuildPedestal(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map,
            [Combine] FlagPedestalNode flagPedestal, [JoinByTeam] TeamNode teamNode) {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            TeamColor teamColor = teamNode.teamColor.TeamColor;

            GameObject original = teamColor != TeamColor.RED ? assetProxyBehaviour.bluePedestal
                                      : assetProxyBehaviour.redPedestal;

            FlagPedestalInstanceComponent flagPedestalInstanceComponent = new();
            Vector3 position = flagPedestal.flagPedestal.Position;
            flagPedestalInstanceComponent.FlagPedestalInstance = Object.Instantiate(original, position, Quaternion.identity);
            flagPedestal.Entity.AddComponent(flagPedestalInstanceComponent);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagInstanceComponent> flag) =>
            Object.Destroy(flag.component.FlagInstance);

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagPedestalInstanceComponent> pedestal) =>
            Object.Destroy(pedestal.component.FlagPedestalInstance);

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, TeamNode team, [JoinByTeam] SingleNode<FlagInstanceComponent> flag) =>
            Object.Destroy(flag.component.FlagInstance);

        [OnEventFire]
        public void DestroyPedestal(NodeRemoveEvent e, TeamNode team,
            [JoinByTeam] SingleNode<FlagPedestalInstanceComponent> pedestal) =>
            Object.Destroy(pedestal.component.FlagPedestalInstance);

        public class FlagNode : Node {
            public BattleGroupComponent battleGroup;
            public FlagPositionComponent flagPosition;

            public TeamGroupComponent teamGroup;
        }

        public class FlagPedestalNode : Node {
            public BattleGroupComponent battleGroup;
            public FlagPedestalComponent flagPedestal;

            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
        }

        public class BattleNode : Node {
            public BattleGroupComponent battleGroup;
            public CTFComponent ctf;

            public ResourceDataComponent resourceData;

            public SelfComponent self;
        }
    }
}