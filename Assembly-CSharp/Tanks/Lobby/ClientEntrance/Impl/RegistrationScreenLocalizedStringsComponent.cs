using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class RegistrationScreenLocalizedStringsComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI finishButtonText;

        [SerializeField] TextMeshProUGUI skipButtonText;

        [SerializeField] TextMeshProUGUI nicknameHintText;

        [SerializeField] TextMeshProUGUI passwordHintText;

        [SerializeField] TextMeshProUGUI repeatPasswordHintText;

        [SerializeField] TextMeshProUGUI iAcceptTermsPart1Text;

        [SerializeField] TextMeshProUGUI iAcceptTermsPart2EULAText;

        [SerializeField] TextMeshProUGUI iAcceptTermsPart4RulesText;

        [SerializeField] TextMeshProUGUI iAcceptTermsPart5PrivacyPolicyText;

        public string Finish {
            set => finishButtonText.text = value;
        }

        public string Skip {
            set => skipButtonText.text = value;
        }

        public string Nickname {
            set => nicknameHintText.text = value;
        }

        public string Password {
            set => passwordHintText.text = value;
        }

        public string RepeatPassword {
            set => repeatPasswordHintText.text = value;
        }

        public string IAcceptTermsPart1 {
            set => iAcceptTermsPart1Text.text = value;
        }

        public string IAcceptTermsPart2EULA {
            set => iAcceptTermsPart2EULAText.text = value;
        }

        public string IAcceptTermsPart4Rules {
            set => iAcceptTermsPart4RulesText.text = value;
        }

        public string IAcceptTermsPart5PrivacyPolicy {
            set => iAcceptTermsPart5PrivacyPolicyText.text = value;
        }

        public string LicenseAgreementUrl { get; set; }

        public string GameRulesUrl { get; set; }

        public string PrivacyPolicyUrl { get; set; }
    }
}