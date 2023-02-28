using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class AnimatedValueComponent : BehaviourComponent {
        public float animationTime;

        public AnimationCurve curve;

        [SerializeField] long startValue;

        [SerializeField] long maximum;

        [SerializeField] long price;

        [SerializeField] Slider upgradeSlider;

        [SerializeField] TextMeshProUGUI upgradeCount;

        [SerializeField] GameObject outline;

        [SerializeField] bool canStart;

        bool canUpdate;

        bool isOutline;

        bool isStart;

        float startTime;

        float time;

        public long StartValue {
            set => startValue = value;
        }

        public long Maximum {
            set => maximum = value;
        }

        public long Price {
            set => price = value;
        }

        void Update() {
            if (canStart) {
                startTime = Time.time;
                canUpdate = true;
                canStart = false;
                isStart = true;
            }

            if (canUpdate && price > 0) {
                time = Time.time - startTime;
                float num = curve.Evaluate(time / animationTime) * (maximum - startValue);
                float num2 = curve.Evaluate(time / animationTime) * (maximum - startValue) * 100f;
                long num3 = (long)(startValue + num);
                long num4 = (long)(startValue * 100 + num2);
                upgradeSlider.value = num4;
                upgradeCount.text = string.Empty + num3 + "/" + price;

                if (num3 >= price && outline != null) {
                    outline.GetComponent<Animator>().SetTrigger("Blink");
                }
            }

            if (canUpdate && startValue >= price && outline != null && price > 0) {
                outline.GetComponent<Animator>().SetTrigger("Upgrade");
            }

            if (time >= animationTime) {
                canUpdate = false;
            }

            if (price == 0) {
                upgradeCount.text = string.Empty;
            }
        }
    }
}