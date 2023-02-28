using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tanks.Battle.ClientMapEditor.Impl {
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class PunitiveGeometryBehaviour : EditorBehavior {
        public bool showGeometry;

        public AnticheatAction anticheatAction;

        [Conditional("UNITY_EDITOR")]
        void Update() {
            BoxCollider component = GetComponent<BoxCollider>();

            if (component.transform.rotation != Quaternion.identity) {
                Debug.LogWarning("Punitive boxes can not be rotated");
                component.transform.rotation = Quaternion.identity;
            }

            if (component.transform.localScale != Vector3.one) {
                Debug.LogWarning("Punitive boxes can not be scaled");
                component.transform.localScale = Vector3.one;
            }
        }

        void OnDrawGizmos() {
            if (showGeometry) {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

                Gizmos.DrawCube(transform.position + new Vector3(GetComponent<BoxCollider>().center.x, GetComponent<BoxCollider>().center.y, GetComponent<BoxCollider>().center.z),
                    new Vector3(GetComponent<BoxCollider>().size.x, GetComponent<BoxCollider>().size.y, GetComponent<BoxCollider>().size.z));
            }
        }

        public void Initialize(AnticheatAction anticheatAction) {
            this.anticheatAction = anticheatAction;
        }
    }
}