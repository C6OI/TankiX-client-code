using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PaintBuilderSystem : ECSSystem {
        [OnEventComplete]
        public void InstantiateAndPreparePaint(NodeAddedEvent e, PaintBattleItemNode paintBattleItem,
            [Context] [JoinByTank] TankNode tank) =>
            tank.Entity.AddComponent(
                new PaintInstanceComponent((GameObject)Object.Instantiate(paintBattleItem.resourceData.Data)));

        public class PaintBattleItemNode : Node {
            public PaintBattleItemComponent paintBattleItem;

            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node {
            public BattleGroupComponent battleGroup;

            public HullInstanceComponent hullInstance;

            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }
    }
}