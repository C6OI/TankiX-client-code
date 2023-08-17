using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankSoundEffectAssetComponent : BehaviourComponent {
        [SerializeField] AudioSource selfUserRankSource;

        [SerializeField] AudioSource remoteUserRankSource;

        public AudioSource SelfUserRankSource => selfUserRankSource;

        public AudioSource RemoteUserRankSource => remoteUserRankSource;
    }
}