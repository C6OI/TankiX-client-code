using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class TargetBehaviour : MonoBehaviour {
        public Entity TargetEntity { get; protected set; }

        public Entity TargetIcarnationEntity { get; protected set; }

        public void Init(Entity targetEntity, Entity targetIcarnationEntity = null) {
            TargetEntity = targetEntity;
            TargetIcarnationEntity = targetIcarnationEntity ?? targetEntity;
        }

        public virtual bool CanSkip(Entity targetingOwner) => false;

        public virtual bool AcceptAsTarget(Entity targetingOwner) => true;
    }
}