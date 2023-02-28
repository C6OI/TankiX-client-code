using System.Text.RegularExpressions;
using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UserEmailUIComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI text;

        string email = string.Empty;

        bool emailIsVisible;

        public string FormatText { get; set; } = string.Empty;

        public string UnconfirmedEmail { get; set; } = string.Empty;

        public string Email {
            get => email;
            set {
                email = value;
                EmailIsVisible = emailIsVisible;
            }
        }

        public bool EmailIsVisible {
            get => emailIsVisible;
            set {
                emailIsVisible = value;
                string newValue = !emailIsVisible ? Regex.Replace(email, "[A-Za-z0-9]", "*") : email;
                string newValue2 = !emailIsVisible ? Regex.Replace(UnconfirmedEmail, "[A-Za-z0-9]", "*") : UnconfirmedEmail;
                text.text = FormatText.Replace("%EMAIL%", newValue).Replace("%UNCEMAIL%", newValue2);
            }
        }
    }
}