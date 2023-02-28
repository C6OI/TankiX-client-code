using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EffectActivationSoundComponent : BehaviourComponent {
        [SerializeField] AudioSource sound;

        public AudioSource Sound => sound;
    }
}