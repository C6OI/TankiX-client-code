using UnityEngine;

namespace Tanks.Battle.ClientMapEditor.Impl {
    public class ShowCollider : MonoBehaviour {
        public bool showGeometry;

        public Color edgeColor;

        public Color faceColor;

        Vector3 position;

        Quaternion rotation;

        Vector3 scale;

        void OnDrawGizmos() {
            if (showGeometry) {
                if (GetComponent<BoxCollider>() != null) {
                    position = transform.position + new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z);
                    scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    Gizmos.color = faceColor;
                    Gizmos.DrawCube(position, scale);
                    Gizmos.color = edgeColor;
                    Gizmos.DrawWireCube(position, scale);
                }

                if (GetComponent<SphereCollider>() != null) {
                    SphereCollider component = GetComponent<SphereCollider>();
                    position = transform.position + new Vector3(component.center.x, component.center.y, component.center.z);
                    float radius = component.radius;
                    Gizmos.color = faceColor;
                    Gizmos.DrawSphere(position, radius);
                    Gizmos.color = edgeColor;
                    Gizmos.DrawWireSphere(position, radius);
                }
            }
        }
    }
}