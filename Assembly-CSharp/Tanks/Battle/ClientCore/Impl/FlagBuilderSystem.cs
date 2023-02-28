using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FlagBuilderSystem : ECSSystem {
        [OnEventFire]
        public void BuildFlag(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map, [Combine] FlagNode flag, [JoinByTeam] TeamNode teamNode) {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            TeamColor teamColor = teamNode.colorInBattle.TeamColor;
            GameObject original = teamColor != TeamColor.RED ? assetProxyBehaviour.blueFlag : assetProxyBehaviour.redFlag;
            GameObject original2 = teamColor != TeamColor.RED ? assetProxyBehaviour.blueFlagBeam : assetProxyBehaviour.redFlagBeam;
            FlagInstanceComponent flagInstanceComponent = new();
            Vector3 position = flag.flagPosition.Position;
            GameObject gameObject2 = flagInstanceComponent.FlagInstance = Object.Instantiate(original, position, Quaternion.identity);
            GameObject flagBeam = Object.Instantiate(original2, gameObject2.transform, false);
            flagInstanceComponent.FlagBeam = flagBeam;
            flag.Entity.AddComponent(flagInstanceComponent);
            FlagPhysicsBehaviour flagPhysicsBehaviour = gameObject2.AddComponent<FlagPhysicsBehaviour>();
            flagPhysicsBehaviour.TriggerEntity = flag.Entity;
            flag.Entity.AddComponent(new FlagColliderComponent(gameObject2.GetComponent<BoxCollider>()));
        }

        static CTFAssetProxyBehaviour GetAssetProxyBehaviour(BattleNode ctf) => ((GameObject)ctf.resourceData.Data).GetComponent<CTFAssetProxyBehaviour>();

        [OnEventFire]
        public void BuildPedestal(NodeAddedEvent e, BattleNode ctf, SingleNode<MapInstanceComponent> map, [Combine] FlagPedestalNode flagPedestal, [JoinByTeam] TeamNode teamNode) {
            CTFAssetProxyBehaviour assetProxyBehaviour = GetAssetProxyBehaviour(ctf);
            TeamColor teamColor = teamNode.colorInBattle.TeamColor;
            GameObject original = teamColor != TeamColor.RED ? assetProxyBehaviour.bluePedestal : assetProxyBehaviour.redPedestal;
            FlagPedestalInstanceComponent flagPedestalInstanceComponent = new();
            Vector3 position = flagPedestal.flagPedestal.Position;
            flagPedestalInstanceComponent.FlagPedestalInstance = Object.Instantiate(original, position, Quaternion.identity);
            flagPedestal.Entity.AddComponent(flagPedestalInstanceComponent);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagInstanceComponent> flag) {
            Object.Destroy(flag.component.FlagInstance);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, SingleNode<FlagPedestalInstanceComponent> pedestal) {
            Object.Destroy(pedestal.component.FlagPedestalInstance);
        }

        [OnEventFire]
        public void DestroyFlag(NodeRemoveEvent e, TeamNode team, [JoinByTeam] SingleNode<FlagInstanceComponent> flag) {
            Object.Destroy(flag.component.FlagInstance);
        }

        [OnEventFire]
        public void DestroyPedestal(NodeRemoveEvent e, TeamNode team, [JoinByTeam] SingleNode<FlagPedestalInstanceComponent> pedestal) {
            Object.Destroy(pedestal.component.FlagPedestalInstance);
        }

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
            public ColorInBattleComponent colorInBattle;

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