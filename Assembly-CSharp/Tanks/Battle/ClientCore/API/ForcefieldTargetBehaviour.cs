using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class ForcefieldTargetBehaviour : TargetBehaviour {
        public bool OwnerTeamCanShootThrough;

        public override bool CanSkip(Entity targetingOwner) => CheckCanSkip(targetingOwner);

        public override bool AcceptAsTarget(Entity targetingOwner) => false;

        bool CheckCanSkip(Entity targetingOwner) => OwnerTeamCanShootThrough &&
                                                    (TargetEntity.IsSameGroup<TankGroupComponent>(targetingOwner) || TargetEntity.IsSameGroup<TeamGroupComponent>(targetingOwner));
    }
}