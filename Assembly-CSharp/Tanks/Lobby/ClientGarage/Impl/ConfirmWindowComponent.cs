using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ConfirmWindowComponent : UIBehaviour, Component {
        [SerializeField] protected TextMeshProUGUI confirmText;

        [SerializeField] protected TextMeshProUGUI declineText;

        [SerializeField] protected TextMeshProUGUI headerText;

        [SerializeField] protected TextMeshProUGUI mainText;

        [SerializeField] protected Button confirmButton;

        [SerializeField] protected Button declineButton;

        List<Animator> animators;

        [Inject] public static EngineService EngineService { get; set; }

        public string ConfirmText {
            set => confirmText.text = value;
        }

        public string DeclineText {
            set => declineText.text = value;
        }

        public string HeaderText {
            set {
                headerText.text = value;
                headerText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public string MainText {
            set {
                mainText.text = value;
                mainText.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        public bool show { get; set; }

        protected override void Start() {
            confirmButton.onClick.AddListener(OnYes);

            if (declineButton != null) {
                declineButton.onClick.AddListener(OnNo);
            }
        }

        protected override void OnEnable() {
            GetComponentInChildren<CanvasGroup>().alpha = 0f;
            GetComponent<Animator>().SetBool("show", true);

            if (animators == null) {
                return;
            }

            foreach (Animator animator in animators) {
                animator.SetBool("Visible", false);
            }
        }

        protected override void OnDisable() { }

        public void Show(List<Animator> animators) {
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            this.animators = animators;
            show = true;

            if (gameObject.activeInHierarchy) {
                OnEnable();
            } else {
                gameObject.SetActive(true);
            }
        }

        void OnYes() {
            Hide();

            if (GetComponent<EntityBehaviour>() != null) {
                Entity entity = GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogConfirmEvent>(entity);
            }
        }

        void OnNo() {
            Hide();

            if (GetComponent<EntityBehaviour>() != null) {
                Entity entity = GetComponent<EntityBehaviour>().Entity;
                EngineService.Engine.ScheduleEvent<DialogDeclineEvent>(entity);
            }
        }

        public void Hide() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            show = false;
            GetComponent<Animator>().SetBool("show", false);

            if (animators == null) {
                return;
            }

            foreach (Animator animator in animators) {
                animator.SetBool("Visible", true);
            }
        }

        public void HideByBackgorundClick() {
            if (!TutorialCanvas.Instance.IsShow) {
                Hide();
            }
        }

        public void OnHide() {
            if (show) {
                OnEnable();
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}