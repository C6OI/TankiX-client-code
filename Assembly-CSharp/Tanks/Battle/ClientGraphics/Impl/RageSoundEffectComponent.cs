using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RageSoundEffectComponent : BehaviourComponent {
        [SerializeField] RageSoundEffectBehaviour asset;

        public RageSoundEffectBehaviour Asset => asset;
    }
}