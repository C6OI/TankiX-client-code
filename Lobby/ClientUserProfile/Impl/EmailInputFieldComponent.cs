using Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.Impl {
    [RequireComponent(typeof(InputFieldComponent))]
    public class EmailInputFieldComponent : LocalizedControl, Component {
        [Tooltip("Если true - переводит инпут в валидное состояние если email существует, не валидное - если не существует")]
        [SerializeField]
        bool existsIsValid;

        public bool ExistsIsValid => existsIsValid;

        public string Hint {
            set => GetComponent<InputFieldComponent>().Hint = value;
        }

        public string EmailIsInvalid { get; set; }

        public string EmailIsOccupied { get; set; }

        public string EmailIsNotConfirmed { get; set; }
    }
}