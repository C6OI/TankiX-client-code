using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatContentGUIComponent : MonoBehaviour, Component {
        [FormerlySerializedAs("messageAsset")] [SerializeField]
        GameObject messagePrefab;

        public GameObject MessagePrefab => messagePrefab;

        public void ClearMessages() {
            IEnumerator enumerator = this.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;

                    if (transform.GetComponent<ChatMessageUIComponent>() != null) {
                        Destroy(transform.gameObject);
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