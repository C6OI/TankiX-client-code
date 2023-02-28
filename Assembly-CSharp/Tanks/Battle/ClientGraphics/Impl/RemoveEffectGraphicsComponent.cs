using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RemoveEffectGraphicsComponent : BehaviourComponent {
        [SerializeField] GameObject effectPrefab;

        [SerializeField] float effectLifeTime = 2f;

        [SerializeField] Vector3 origin = Vector3.up;

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }

        public float EffectLifeTime => effectLifeTime;

        public Vector3 Origin => origin;
    }
}