using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapNativeSoundsAssetComponent : BehaviourComponent {
        [SerializeField] MapNativeSoundsBehaviour asset;

        public MapNativeSoundsBehaviour Asset => asset;
    }
}