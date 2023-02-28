using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class CTFAssetProxyBehaviour : MonoBehaviour {
        [Header("Red flag")] public GameObject redPedestal;

        public GameObject redFlag;

        public GameObject redFlagBeam;

        [Header("Blue flag")] public GameObject bluePedestal;

        public GameObject blueFlag;

        public GameObject blueFlagBeam;

        [Header("Sounds")] public GameObject flagLostSound;

        public GameObject flagReturnSound;

        public GameObject flagStoleSound;

        public GameObject flagWinSound;
    }
}