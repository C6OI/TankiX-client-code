using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(Animator))]
    public class Timer : MonoBehaviour {
        public static float battleTime;

        Animator animator;

        bool autoUpdate;

        bool firstUpdateTime = true;

        float intLastTime;

        float lastTime;

        TextMeshProUGUI text;

        Animator Animator {
            get {
                if (animator == null) {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }

        TextMeshProUGUI Text {
            get {
                if (text == null) {
                    text = GetComponent<TextMeshProUGUI>();
                }

                return text;
            }
        }

        void Update() {
            if (autoUpdate && lastTime > 0f) {
                Set(lastTime - Time.deltaTime);
            }
        }

        public void Set(float time) {
            if (!Mathf.Approximately(time, lastTime)) {
                Animator.SetFloat("Speed", time < 10f ? 1 : 0);
                Animator.SetBool("Grow", time < 60f);
                int num = (int)time;

                if (firstUpdateTime) {
                    UpdateTextTime(num);
                } else if (num != intLastTime) {
                    UpdateTextTime(num);
                }

                lastTime = time;
                intLastTime = num;
                battleTime = intLastTime;
            }
        }

        public void Set(float time, bool autoUpdate) {
            this.autoUpdate = autoUpdate;
            Set(time);
        }

        void UpdateTextTime(int time) {
            firstUpdateTime = false;
            Text.text = FormatTime(time);
        }

        string FormatTime(int time) => string.Format("{00}:{1:00}", time / 60, time % 60);
    }
}