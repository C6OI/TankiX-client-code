using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class UnitMoveSmootherTestBehaviour : MonoBehaviour {
        void Update() {
            Rigidbody componentInChildren = GetComponentInChildren<Rigidbody>();
            UnitMoveSmootherComponent componentInChildren2 = GetComponentInChildren<UnitMoveSmootherComponent>();

            if (Input.GetMouseButtonUp(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo)) {
                    Vector3 point = hitInfo.point;
                    point.y += 0.5f;
                    componentInChildren2.BeforeSetMovement();
                    Vector3 position = point;
                    componentInChildren.transform.position = position;
                    componentInChildren.position = position;
                    Quaternion rotation = Quaternion.LookRotation(Vector3.left);
                    componentInChildren.transform.rotation = rotation;
                    componentInChildren.rotation = rotation;
                    componentInChildren.velocity = Vector3.zero;
                    componentInChildren.angularVelocity = Vector3.zero;
                    componentInChildren.ResetInertiaTensor();
                    componentInChildren2.AfterSetMovement();
                }
            }
        }
    }
}