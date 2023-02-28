using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PaintBuilderSystem : ECSSystem {
        [OnEventComplete]
        public void InstantiateAndPreparePaint(NodeAddedEvent e, TankPaintBattleItemNode paintBattleItem, [Context] [JoinByTank] TankNode tank) {
            Transform transform = tank.hullInstance.HullInstance.transform;
            GameObject paintInstance = Object.Instantiate(paintBattleItem.resourceData.Data, transform) as GameObject;
            tank.Entity.AddComponent(new TankPartPaintInstanceComponent(paintInstance));
        }

        [OnEventComplete]
        public void InstantiateAndPreparePaint(NodeAddedEvent e, WeaponPaintBattleItemNode paintBattleItem, [Context] [JoinByTank] WeaponNode weapon) {
            Transform transform = weapon.weaponInstance.WeaponInstance.transform;
            GameObject paintInstance = Object.Instantiate(paintBattleItem.resourceData.Data, transform) as GameObject;
            weapon.Entity.AddComponent(new TankPartPaintInstanceComponent(paintInstance));
        }

        public class TankPaintBattleItemNode : Node {
            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
            public TankPaintBattleItemComponent tankPaintBattleItem;
        }

        public class WeaponPaintBattleItemNode : Node {
            public ResourceDataComponent resourceData;

            public TankGroupComponent tankGroup;
            public WeaponPaintBattleItemComponent weaponPaintBattleItem;
        }

        public class TankNode : Node {
            public BattleGroupComponent battleGroup;

            public HullInstanceComponent hullInstance;

            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node {
            public BattleGroupComponent battleGroup;

            public TankGroupComponent tankGroup;

            public WeaponComponent weapon;

            public WeaponInstanceComponent weaponInstance;
        }
    }
}