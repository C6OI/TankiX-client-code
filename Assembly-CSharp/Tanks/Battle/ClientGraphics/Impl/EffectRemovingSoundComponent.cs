using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EffectRemovingSoundComponent : BehaviourComponent {
        [SerializeField] AudioSource sound;

        public AudioSource Sound => sound;
    }
}