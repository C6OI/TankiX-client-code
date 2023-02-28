using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NormalizedAnimatedValue : MonoBehaviour {
        [Range(0f, 1f)] public float value;
    }
}