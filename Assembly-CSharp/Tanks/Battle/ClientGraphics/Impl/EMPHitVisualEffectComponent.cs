using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EMPHitVisualEffectComponent : BehaviourComponent {
        [SerializeField] ParticleSystem HitPrefab;

        public ParticleSystem EmpHitPrefab => HitPrefab;
    }
}