using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tanks.Battle.ClientMapEditor.Impl {
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class PunitiveGeometryBehaviour : EditorBehavior {
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

        public void Initialize(AnticheatAction anticheatAction) => this.anticheatAction = anticheatAction;
    }
}