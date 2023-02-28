using Tanks.Lobby.ClientHangar.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarLocationBehaviour : MonoBehaviour {
        [SerializeField] HangarLocation hangarLocation;

        public HangarLocation HangarLocation => hangarLocation;
    }
}