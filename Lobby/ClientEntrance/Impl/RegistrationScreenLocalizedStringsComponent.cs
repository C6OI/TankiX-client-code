using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class RegistrationScreenLocalizedStringsComponent : LocalizedScreenComponent {
        [SerializeField] Text finishButtonText;

        [SerializeField] Text nicknameHintText;

        [SerializeField] Text passwordHintText;

        [SerializeField] Text repeatPasswordHintText;

        [SerializeField] Text iAcceptTermsPart1Text;

        [SerializeField] Text iAcceptTermsPart2EULAText;

        [SerializeField] Text iAcceptTermsPart3ConfidentialityText;

        [SerializeField] Text iAcceptTermsPart4RulesText;

        [SerializeField] Text iAcceptTermsPart5PrivacyPolicyText;

        public string Finish {
            set => finishButtonText.text = value;
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

        public string IAcceptTermsPart3Confidentiality {
            set => iAcceptTermsPart3ConfidentialityText.text = value;
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