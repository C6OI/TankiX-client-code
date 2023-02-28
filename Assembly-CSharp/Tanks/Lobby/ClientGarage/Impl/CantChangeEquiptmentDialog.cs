using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CantChangeEquiptmentDialog : UIBehaviour, Component, ICancelHandler, IEventSystemHandler {
        [SerializeField] TextMeshProUGUI message;

        [SerializeField] Button okButton;

        public LocalizedField messageLocalizedField;

        protected override void Start() {
            okButton.onClick.AddListener(OnOk);
        }

        protected override void OnDisable() {
            gameObject.SetActive(false);
            GetComponent<Animator>().SetBool("Visible", false);
        }

        public void OnCancel(BaseEventData eventData) {
            Hide();
        }

        public void Show() {
            MainScreenComponent.Instance.Lock();
            message.text = messageLocalizedField.Value;
            gameObject.SetActive(true);
        }

        void OnOk() {
            Hide();
        }

        public void Hide() {
            MainScreenComponent.Instance.Unlock();
            GetComponent<Animator>().SetBool("Visible", false);
        }
    }
}