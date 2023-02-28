using System.Collections;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class DisableFromAnimation : MonoBehaviour {
        public void DisableGameObjectFromAnimation() {
            StartCoroutine(DisableGameObjectOnEndOfFrame());
        }

        IEnumerator DisableGameObjectOnEndOfFrame() {
            yield return new WaitForEndOfFrame();

            gameObject.SetActive(false);
        }
    }
}