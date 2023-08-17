using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusGraphicsBuilderSystem : ECSSystem {
        [OnEventFire]
        public void AddMaterialComponent(NodeAddedEvent evt, SingleNode<BonusBoxInstanceComponent> bonus) =>
            bonus.Entity.AddComponent(
                new MaterialComponent(MaterialAlphaUtils.GetMaterial(bonus.component.BonusBoxInstance)));

        [OnEventFire]
        public void AddParachuteMaterialComponent(NodeAddedEvent evt, SingleNode<BonusParachuteInstanceComponent> bonus) =>
            bonus.Entity.AddComponent(
                new ParachuteMaterialComponent(MaterialAlphaUtils.GetMaterial(bonus.component.BonusParachuteInstance)));
    }
}