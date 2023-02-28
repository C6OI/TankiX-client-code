using System.Collections;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PresetNameEditorComponent : UIBehaviour, Component, IPointerClickHandler, IEventSystemHandler {
        [SerializeField] MainScreenComponent mainScreen;

        [SerializeField] EntityBehaviour entityBehaviour;

        [SerializeField] TMP_InputField inputField;

        [SerializeField] Button editButton;

        string nameBeforeEdit;

        [Inject] public static EngineService EngineService { get; set; }

        public string PresetName {
            get => inputField.text;
            set => inputField.text = value;
        }

        protected override void Awake() {
            editButton.onClick.AddListener(OnBeginEdit);
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.clickCount > 1) {
                OnBeginEdit();
            }
        }

        void OnBeginEdit() {
            nameBeforeEdit = inputField.text;
            editButton.gameObject.SetActive(false);
            inputField.enabled = true;
            inputField.ActivateInputField();
        }

        public void DisableInput() {
            inputField.interactable = false;
        }

        public void EnableInput() {
            inputField.interactable = true;
        }

        void OnEndEdit(string value) {
            editButton.gameObject.SetActive(true);
            StartCoroutine(LateEndEdit());

            if (string.IsNullOrEmpty(value) || value.Contains('\n') || value.All(char.IsWhiteSpace)) {
                inputField.text = nameBeforeEdit;
            } else if (!nameBeforeEdit.Equals(value)) {
                EngineService.Engine.ScheduleEvent<PresetNameChangedEvent>(entityBehaviour.Entity);
            }
        }

        IEnumerator LateEndEdit() {
            yield return new WaitForEndOfFrame();

            inputField.enabled = false;
        }
    }
}