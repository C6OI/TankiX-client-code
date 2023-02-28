using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public abstract class OverrideGoBack : MonoBehaviour {
        public abstract ShowScreenData ScreenData { get; }
    }
}