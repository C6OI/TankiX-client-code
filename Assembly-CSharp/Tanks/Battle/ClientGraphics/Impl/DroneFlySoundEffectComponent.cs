using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class DroneFlySoundEffectComponent : BehaviourComponent {
        [SerializeField] SoundController sound;

        public SoundController Sound => sound;
    }
}