using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class TankCollisionComponent : MonoBehaviour, Component {
        [SerializeField] bool hasCollision;

        public bool HasCollision => hasCollision;

        public Collision Collision { get; private set; }

        void OnCollisionEnter(Collision collision) {
            hasCollision = true;
            Collision = collision;
        }

        void OnCollisionExit(Collision collision) {
            hasCollision = false;
            Collision = null;
        }
    }
}