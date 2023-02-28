using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class TutorialKeymapComponent : UIBehaviour, Component {
        public GameObject prefab;

        public float showDelay = 30f;

        public float destroyDelay = 1f / 3f;

        GameObject content;

        float hideTime = float.MaxValue;

        float showTime = float.MaxValue;

        bool visible;

        public bool Visible {
            get => content != null && visible;
            set {
                if (value) {
                    if (content != null) {
                        Destroy(content);
                    }

                    content = Instantiate(prefab, transform, false);
                }

                if (content != null) {
                    content.GetComponent<Animator>().SetTrigger(!value ? "HIDE" : "SHOW");
                }

                if (value) {
                    showTime = Time.time;
                } else {
                    hideTime = Time.time;
                }

                visible = value;
            }
        }

        void Update() {
            if (Visible && Time.time > showTime + showDelay) {
                Visible = false;
            }

            if (!visible && Time.time > hideTime + destroyDelay && content != null) {
                Destroy(content);
                content = null;
            }
        }

        public void ResetState() {
            showTime = float.MaxValue;
            hideTime = float.MaxValue;
            visible = false;

            if (content != null) {
                Destroy(content);
                content = null;
            }
        }
    }
}