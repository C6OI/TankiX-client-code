using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapReverbZoneAssetComponent : BehaviourComponent {
        [SerializeField] GameObject mapReverbZonesRoot;

        public GameObject MapReverbZonesRoot => mapReverbZonesRoot;
    }
}