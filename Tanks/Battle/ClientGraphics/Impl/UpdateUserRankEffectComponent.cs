using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankEffectComponent : BehaviourComponent {
        [SerializeField] GameObject effectPrefab;

        [SerializeField] float finishEventTime = 7f;

        public GameObject EffectPrefab => effectPrefab;

        public float FinishEventTime => finishEventTime;
    }
}