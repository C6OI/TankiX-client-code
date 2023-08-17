using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using UnityEngine.UI;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ActivatorScreen : UnityAwareActivator<AutoCompleting> {
        [SerializeField] CanvasGroup backgroundGroup;

        [SerializeField] Text entranceMessage;

        [SerializeField] float fadeOutTimeSec = 1f;

        float fadeOutSpeed;

        State state;

        void Update() {
            float alpha = backgroundGroup.alpha;

            switch (state) {
                case State.FADE_OUT:
                    alpha += fadeOutSpeed * Time.deltaTime;

                    if (alpha <= 0f) {
                        Destroy(gameObject);
                    } else {
                        backgroundGroup.alpha = alpha;
                    }

                    break;

                case State.PREPARED:
                    state = State.PREPARED_IDLE;
                    break;

                case State.PREPARED_IDLE:
                    entranceMessage.gameObject.SetActive(false);
                    state = State.FADE_OUT;
                    break;
            }
        }

        new void OnEnable() {
            fadeOutSpeed = -1f / fadeOutTimeSec;
            backgroundGroup.alpha = 1f;
            state = State.IDLE;
        }

        protected override void Activate() => state = State.PREPARED;

        enum State {
            IDLE = 0,
            PREPARED = 1,
            PREPARED_IDLE = 2,
            FADE_OUT = 3
        }
    }
}