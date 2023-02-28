using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class PenetrationTargetValidator : TargetValidator {
        public PenetrationTargetValidator(Entity ownerEntity)
            : base(ownerEntity) { }

        public override bool BreakOnTargetHit(Entity target) => false;
    }
}