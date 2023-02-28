using Platform.Library.ClientDataStructures.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MinePhysicsTriggerBehaviour : TriggerBehaviour<TriggerEnterEvent> {
        void Start() {
            GetComponentsInChildren<Collider>(true).ForEach(delegate(Collider c) {
                c.enabled = true;
            });
        }

        void OnTriggerEnter(Collider other) {
            SendEventByCollision(other);
        }
    }
}