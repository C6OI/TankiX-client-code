using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PhysicTestBehaviour : MonoBehaviour {
        public Rigidbody body1;

        public Rigidbody body2;

        Vector3 lastPosition1 = Vector3.zero;

        Vector3 lastPosition2 = Vector3.zero;

        public void FixedUpdate() {
            Vector3 position = body1.position;

            if (lastPosition1 != Vector3.zero) {
                body1.velocity = (position - lastPosition1) / Time.fixedDeltaTime;
            }

            lastPosition1 = position;
            BoxCollider component = body1.GetComponent<BoxCollider>();
            BoxCollider component2 = body2.GetComponent<BoxCollider>();
            DepenetrationForce.ApplyDepenetrationForce(body1, component, body2, component2);
            DepenetrationForce.ApplyDepenetrationForce(body2, component2, body1, component);
        }
    }
}