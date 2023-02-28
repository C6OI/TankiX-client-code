using UnityEngine;

namespace Tanks.Battle.ClientHUD.API {
    public abstract class BaseUserNotificationMessageBehaviour : MonoBehaviour {
        [SerializeField] protected Animator animator;

        [SerializeField] float lifeTime = 3f;

        bool destroyTriggerSend;

        bool isDestroying;

        float timer;

        void Update() {
            if (isDestroying && !destroyTriggerSend) {
                if (timer <= 0f) {
                    animator.SetTrigger("FadeOut");
                    destroyTriggerSend = true;
                } else {
                    timer -= Time.deltaTime;
                }
            }
        }

        void OnEnable() {
            isDestroying = false;
            destroyTriggerSend = false;
            timer = lifeTime;
        }

        void OnNotificationFadeIn() {
            isDestroying = true;
        }

        void OnNotificationFadeOut() {
            SendMessageUpwards("OnUserNotificationFadeOut", SendMessageOptions.RequireReceiver);
            DestroyObject(gameObject);
        }

        public void Play() {
            gameObject.SetActive(true);
        }
    }
}