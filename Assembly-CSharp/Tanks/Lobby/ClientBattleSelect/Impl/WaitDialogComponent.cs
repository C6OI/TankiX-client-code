using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class WaitDialogComponent : UIBehaviour, Component {
        public float maxTimerValue = 5f;

        [SerializeField] Slider timerSlider;

        [SerializeField] TextMeshProUGUI message;

        float _timer;

        bool isShow;

        bool savedCursorVisible;

        CursorLockMode savedLockMode;

        float timer {
            get => _timer;
            set {
                _timer = value;
                timerSlider.value = 1f - timer / maxTimerValue;
            }
        }

        bool IsShow {
            get => isShow;
            set {
                GetComponent<Animator>().SetBool("show", value);
                isShow = value;
            }
        }

        void Update() {
            timer += Time.deltaTime;

            if (timer > maxTimerValue) {
                Hide();
            }
        }

        public virtual void Show(string messageText) {
            timer = 0f;
            MainScreenComponent.Instance.Lock();
            message.text = messageText;
            gameObject.SetActive(true);
            IsShow = true;
        }

        public void Hide() {
            IsShow = false;
            MainScreenComponent.Instance.Unlock();
            Destroy(gameObject, 3f);
        }

        void OnHideAnimationEvent() {
            if (!IsShow) {
                gameObject.SetActive(false);
            }
        }
    }
}