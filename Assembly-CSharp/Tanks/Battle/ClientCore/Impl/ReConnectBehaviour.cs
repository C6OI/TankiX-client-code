using System.Collections;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ReConnectBehaviour : MonoBehaviour {
        public int ReConnectTime { get; set; }

        public void Start() {
            StartCoroutine(LoadState());
        }

        IEnumerator LoadState() {
            yield return new WaitForSeconds(ReConnectTime);

            SceneSwitcher.CleanAndRestart();
        }
    }
}