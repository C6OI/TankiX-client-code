using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFrictionSoundBehaviour : ECSBehaviour {
        Entity tankEntity;

        public bool TriggerStay { get; set; }

        public Collider FrictionCollider { get; set; }

        void Awake() {
            enabled = false;
        }

        void OnTriggerEnter(Collider other) {
            UpdateCollisionStay(other);
        }

        void OnTriggerExit(Collider other) {
            DisableCollisionStay();
            SendTankFrictionExitEvent();
        }

        void OnTriggerStay(Collider other) {
            UpdateCollisionStay(other);
        }

        void SendTankFrictionExitEvent() {
            TankFrictionExitEvent eventInstance = new();
            NewEvent(eventInstance).Attach(tankEntity).Schedule();
        }

        void UpdateCollisionStay(Collider collider) {
            TriggerStay = true;
            FrictionCollider = collider;
        }

        void DisableCollisionStay() {
            TriggerStay = false;
        }

        public void Init(Entity tankEntity) {
            this.tankEntity = tankEntity;
        }
    }
}