using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(NormalizedAnimatedValue))]
    public class CooldownAnimation : MonoBehaviour {
        NormalizedAnimatedValue _animatedValue;
        Text _text;

        float cooldown;

        Text text {
            get {
                if (_text == null) {
                    _text = GetComponent<Text>();
                }

                return _text;
            }
        }

        NormalizedAnimatedValue animatedValue {
            get {
                if (_animatedValue == null) {
                    _animatedValue = GetComponent<NormalizedAnimatedValue>();
                }

                return _animatedValue;
            }
        }

        public float Cooldown {
            get => cooldown;
            set {
                cooldown = value;
                text.text = string.Format("{0:0}", value);
            }
        }

        void Awake() {
            text.text = string.Empty;
        }

        void Update() {
            float num = animatedValue.value * cooldown;

            if (!(num <= 0f)) {
                text.text = string.Format("{0:0}", num);
            }
        }

        void OnDisable() {
            text.text = string.Empty;
        }
    }
}