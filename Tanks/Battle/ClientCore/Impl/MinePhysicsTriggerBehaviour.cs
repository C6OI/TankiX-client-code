using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class MinePhysicsTriggerBehaviour : TriggerBehaviour<TriggerEnterEvent> {
        void Start() => GetComponent<Collider>().enabled = true;

        void OnTriggerEnter(Collider other) => SendEventByCollision(other);
    }
}