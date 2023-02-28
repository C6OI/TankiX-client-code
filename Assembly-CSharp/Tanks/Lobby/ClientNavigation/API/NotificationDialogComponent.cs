using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class NotificationDialogComponent : UIBehaviour, Component, ICancelHandler, IEventSystemHandler {
        [SerializeField] TextMeshProUGUI message;

        [SerializeField] Button okButton;

        [Inject] public static EngineService EngineService { get; set; }

        protected override void Start() {
            okButton.onClick.AddListener(OnOk);
        }

        void Update() {
            if (InputMapping.Cancel) {
                OnOk();
            }
        }

        protected override void OnDisable() { }

        public void OnCancel(BaseEventData eventData) {
            Hide();
        }

        public virtual void Show(string message) {
            this.message.text = message;
            gameObject.SetActive(true);
        }

        void OnOk() {
            Hide();

            if (GetComponent<EntityBehaviour>() != null) {
                Entity entity = GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
            }
        }

        public void Hide() {
            GetComponent<Animator>().SetBool("Visible", false);
        }
    }
}