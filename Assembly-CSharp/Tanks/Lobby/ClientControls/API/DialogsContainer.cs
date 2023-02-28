using System;
using System.Collections;
using System.Linq;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class DialogsContainer : BehaviourComponent {
        public Transform[] ignoredChilds;

        public T Get<T>() where T : MonoBehaviour => GetComponentInChildren<T>(true);

        public void CloseAll(string ignoredName = "") {
            IEnumerator enumerator = this.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;

                    if (!string.Equals(ignoredName, transform.gameObject.name) && (ignoredChilds == null || !ignoredChilds.Contains(transform))) {
                        transform.gameObject.SetActive(false);
                    }
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