using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CommonMapEffectBehaviour : MonoBehaviour {
        [SerializeField] GameObject commonMapEffectPrefab;

        public GameObject CommonMapEffectPrefab => commonMapEffectPrefab;
    }
}