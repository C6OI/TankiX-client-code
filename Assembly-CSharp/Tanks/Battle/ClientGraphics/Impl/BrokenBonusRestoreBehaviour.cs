using System;
using System.Collections;
using LeopotamGroup.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BrokenBonusRestoreBehaviour : MonoBehaviour {
        readonly FastList<Vector3> _positions = new();

        readonly FastList<Quaternion> _rotations = new();

        void Awake() {
            IEnumerator enumerator = this.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    _positions.Add(transform.localPosition);
                    _rotations.Add(transform.localRotation);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        void OnDisable() {
            int num = 0;
            IEnumerator enumerator = this.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    transform.localPosition = _positions[num];
                    transform.localRotation = _rotations[num];
                    num++;
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }
    }
}