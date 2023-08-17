using System.Collections;
using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class ReConnectBehaviour : MonoBehaviour {
        public int ReConnectTime { get; set; }

        public void Start() => StartCoroutine(LoadState());

        IEnumerator LoadState() {
            yield return new WaitForSeconds(ReConnectTime);

            SceneSwitcher.CleanAndSwitch(SceneNames.ENTRANCE);
        }
    }
}