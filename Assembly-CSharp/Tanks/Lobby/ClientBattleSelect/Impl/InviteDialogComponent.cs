using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.Impl;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class InviteDialogComponent : UIBehaviour, Component {
        [SerializeField] GameObject buttons;

        [SerializeField] GameObject keys;

        public float maxTimerValue = 5f;

        [SerializeField] Slider timerSlider;

        [SerializeField] TextMeshProUGUI message;

        [SerializeField] Button acceptButton;

        [SerializeField] Button declineButton;

        [SerializeField] AudioSource sound;

        float _timer;

        bool inBattle;

        bool intractable;

        bool isShow;

        bool savedCursorVisible;

        CursorLockMode savedLockMode;

        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static InputManager InputManager { get; set; }

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

        protected override void Start() {
            acceptButton.onClick.AddListener(OnYes);
            declineButton.onClick.AddListener(OnNo);
        }

        void Update() {
            timer += Time.deltaTime;

            if (timer > maxTimerValue) {
                OnNo();
            }

            if (InputMapping.Cancel) {
                OnNo();
            } else if (Input.GetKeyDown(KeyCode.Y) && inBattle && !ChatIsFocused()) {
                OnYes();
            } else if (Input.GetKeyDown(KeyCode.N) && inBattle && !ChatIsFocused()) {
                OnNo();
            }
        }

        public virtual void Show(string messageText, bool inBattle) {
            intractable = true;
            timer = 0f;
            MainScreenComponent.Instance.Lock();
            message.text = messageText;
            gameObject.SetActive(true);
            IsShow = true;
            this.inBattle = inBattle;
            buttons.SetActive(!inBattle);
            keys.SetActive(inBattle);

            if (sound != null) {
                sound.Play();
            }
        }

        void OnYes() {
            if (intractable) {
                Hide();

                if (GetComponent<EntityBehaviour>() != null) {
                    Entity entity = GetComponent<EntityBehaviour>().Entity;
                    EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
                }
            }
        }

        public void OnNo() {
            if (intractable) {
                Hide();

                if (GetComponent<EntityBehaviour>() != null) {
                    Entity entity = GetComponent<EntityBehaviour>().Entity;
                    EngineService.Engine.ScheduleEvent<DialogDeclineEvent>(entity);
                }
            }
        }

        public void Hide() {
            intractable = false;
            IsShow = false;
            MainScreenComponent.Instance.Unlock();
            Destroy(gameObject, 3f);
        }

        bool ChatIsFocused() {
            BattleChatFocusCheckEvent battleChatFocusCheckEvent = new();
            EngineService.Engine.ScheduleEvent(battleChatFocusCheckEvent, new EntityStub());
            return battleChatFocusCheckEvent.InputIsFocused;
        }

        void OnHideAnimationEvent() {
            if (!IsShow) {
                gameObject.SetActive(false);
            }
        }
    }
}