using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class TriggerBehaviour<T> : MonoBehaviour where T : Event, new() {
        GameObject collisionGameObject;

        public Entity TriggerEntity { get; set; }

        void SendEvent(Engine engine) {
            TargetBehaviour componentInParent = collisionGameObject.GetComponentInParent<TargetBehaviour>();

            engine.NewEvent<T>().Attach(TriggerEntity).Attach(componentInParent.Entity)
                .Schedule();
        }

        protected void SendEventByCollision(Collider other) {
            collisionGameObject = other.gameObject;
            ClientUnityIntegrationUtils.ExecuteInFlow(SendEvent);
        }
    }
}