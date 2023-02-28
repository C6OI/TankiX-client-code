using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModalConfirmWindowComponent : LocalizedControl, Component {
        [SerializeField] string localizePath;

        [SerializeField] Text confirmText;

        [SerializeField] Text cancelText;

        [SerializeField] Text headerText;

        [SerializeField] Text mainText;

        [SerializeField] Text additionalText;

        [SerializeField] ImageSkin icon;

        [SerializeField] Button confirmButton;

        [SerializeField] Button cancelButton;

        [SerializeField] RectTransform contentRoot;

        bool alive;

        Entity screen;

        [Inject] public new static EngineService EngineService { get; set; }

        public string ConfirmText {
            set => confirmText.text = value;
        }

        public string CancelText {
            set => cancelText.text = value;
        }

        public string HeaderText {
            get => headerText.text;
            set {
                headerText.text = value;
                headerText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string MainText {
            get => mainText.text;
            set {
                mainText.text = value;
                mainText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string AdditionalText {
            get => additionalText.text;
            set {
                additionalText.text = value;
                additionalText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string SpriteUid {
            set => icon.SpriteUid = value;
        }

        public RectTransform ContentRoot => contentRoot;

        void Start() {
            alive = true;
            confirmButton.onClick.AddListener(OnYes);
            cancelButton.onClick.AddListener(OnNo);
        }

        void OnApplicationQuit() {
            alive = false;
        }

        protected override string GetRelativeConfigPath() => localizePath;

        public void Show(Entity screen) {
            this.screen = screen;
            gameObject.SetActive(true);

            if (!screen.HasComponent<LockedScreenComponent>()) {
                screen.AddComponent<LockedScreenComponent>();
            }
        }

        void OnYes() {
            Hide();
            SendEvent<DialogConfirmEvent>();
        }

        void OnNo() {
            Hide();
            SendEvent<DialogDeclineEvent>();
        }

        void SendEvent<T>() where T : Event, new() {
            if (GetComponent<EntityBehaviour>() != null) {
                Entity entity = GetComponent<EntityBehaviour>().Entity;
                ScheduleEvent<T>(entity);
            }
        }

        public void Hide() {
            GetComponent<Animator>().SetBool("Visible", false);

            if (alive && screen.HasComponent<LockedScreenComponent>()) {
                screen.RemoveComponent<LockedScreenComponent>();
            }
        }
    }
}