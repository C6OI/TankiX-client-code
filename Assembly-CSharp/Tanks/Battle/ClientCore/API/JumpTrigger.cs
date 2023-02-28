using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class JumpTrigger : ECSBehaviour {
        public Transform targetPoint;

        public float angle = 45f;

        readonly float sqrActivateDistance = 4f;

        void OnTriggerStay(Collider other) {
            Rigidbody attachedRigidbody = other.attachedRigidbody;
            float sqrMagnitude = (attachedRigidbody.position - transform.position).sqrMagnitude;

            if (sqrMagnitude > sqrActivateDistance) {
                return;
            }

            TargetBehaviour componentInParent = attachedRigidbody.GetComponentInParent<TargetBehaviour>();

            if ((bool)componentInParent && componentInParent.TargetEntity.HasComponent<TankSyncComponent>()) {
                Vector3 velocity = CalculateJumpVelocity(attachedRigidbody.position, targetPoint.position);

                if (componentInParent.TargetEntity.HasComponent<TankJumpComponent>()) {
                    TankJumpComponent component = componentInParent.TargetEntity.GetComponent<TankJumpComponent>();
                    component.StartJump(velocity);
                } else {
                    TankJumpComponent tankJumpComponent = new();
                    tankJumpComponent.StartJump(velocity);
                    componentInParent.TargetEntity.AddComponent(tankJumpComponent);
                }
            }
        }

        Vector3 CalculateJumpVelocity(Vector3 startPosition, Vector3 targetPosition) {
            targetPosition.y = startPosition.y;
            Vector3 vector = targetPosition - startPosition;
            float magnitude = vector.magnitude;
            float num = angle * ((float)Math.PI / 180f);
            float num2 = Mathf.Sqrt(Physics.gravity.magnitude * magnitude / Mathf.Sin(2f * num));
            vector.Normalize();
            vector += Vector3.up * Mathf.Tan(num);
            vector.Normalize();
            return vector * num2;
        }
    }
}