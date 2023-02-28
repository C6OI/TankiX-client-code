using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientMapSoundEffectMarkerComponent : BehaviourComponent {
        [SerializeField] AmbientSoundFilter ambientSoundFilter;

        public AmbientSoundFilter AmbientSoundFilter => ambientSoundFilter;
    }
}