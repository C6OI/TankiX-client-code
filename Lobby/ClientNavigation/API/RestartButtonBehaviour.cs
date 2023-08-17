using Lobby.ClientNavigation.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientNavigation.API {
    [RequireComponent(typeof(Button))]
    public class RestartButtonBehaviour : MonoBehaviour {
        void Awake() => GetComponent<Button>().onClick.AddListener(SceneSwitcher.CleanAndRestart);
    }
}