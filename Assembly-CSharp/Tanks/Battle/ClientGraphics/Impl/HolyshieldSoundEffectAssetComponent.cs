using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HolyshieldSoundEffectAssetComponent : BehaviourComponent {
        [SerializeField] SoundController asset;

        public SoundController Asset => asset;
    }
}