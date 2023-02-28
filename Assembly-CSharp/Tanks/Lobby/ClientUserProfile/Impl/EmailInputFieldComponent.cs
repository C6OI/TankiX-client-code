using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [RequireComponent(typeof(InputFieldComponent))]
    public class EmailInputFieldComponent : LocalizedControl, Component {
        [SerializeField] [Tooltip("Если true - переводит инпут в валидное состояние если email существует, не валидное - если не существует")]
        bool existsIsValid;

        [SerializeField] [Tooltip("Если true - дополнительно проверяет в неподтверждённых")]
        bool includeUnconfirmed;

        public bool ExistsIsValid => existsIsValid;

        public bool IncludeUnconfirmed => includeUnconfirmed;

        public string Hint {
            set => GetComponent<InputFieldComponent>().Hint = value;
        }

        public string EmailIsInvalid { get; set; }

        public string EmailIsOccupied { get; set; }

        public string EmailIsNotConfirmed { get; set; }
    }
}