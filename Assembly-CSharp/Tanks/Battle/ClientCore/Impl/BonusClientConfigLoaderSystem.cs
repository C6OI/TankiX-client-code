using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusClientConfigLoaderSystem : ECSSystem {
        [OnEventFire]
        public void LoadBonusClientConfigPrefab(NodeAddedEvent e, BattleNode battle) {
            Entity entity = CreateEntity("BonusClientConfigPrefabLoader");
            entity.AddComponent<BonusClientConfigPrefabLoaderComponent>();
            entity.AddComponent(new BattleGroupComponent(battle.Entity));
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(battle.bonusClientConfigPrefab.AssetGuid)));
            entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CollectComponentToBonusClientConfigEntity(NodeAddedEvent e, BonusClientConfigLoaderNode bonusConfigLoader, [JoinByBattle] SingleNode<BattleComponent> battle) {
            GameObject gameObject = (GameObject)bonusConfigLoader.resourceData.Data;
            gameObject.CollectComponents(battle.Entity);
            DeleteEntity(bonusConfigLoader.Entity);
        }

        [OnEventFire]
        public void DeleteBonusClientConfigLoaderWhenBattleRemoved(NodeRemoveEvent e, SingleNode<BonusClientConfigPrefabComponent> battle,
            [JoinByBattle] BonusClientConfigLoaderForDeleteNode bonusConfigLoader) {
            DeleteEntity(bonusConfigLoader.Entity);
        }

        [Not(typeof(BonusClientConfigComponent))]
        public class BattleNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusClientConfigPrefabComponent bonusClientConfigPrefab;

            public SelfComponent self;
        }

        public class BonusClientConfigLoaderNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusClientConfigPrefabLoaderComponent bonusClientConfigPrefabLoader;

            public ResourceDataComponent resourceData;
        }

        public class BonusClientConfigLoaderForDeleteNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusClientConfigPrefabLoaderComponent bonusClientConfigPrefabLoader;
        }
    }
}