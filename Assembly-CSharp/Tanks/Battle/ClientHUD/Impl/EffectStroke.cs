using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class EffectStroke : MonoBehaviour {
        void OnEnable() {
            RectTransform component = GetComponent<RectTransform>();
            component.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 4) * 90);
        }
    }
}