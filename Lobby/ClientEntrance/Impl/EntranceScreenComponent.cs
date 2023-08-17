using Lobby.ClientNavigation.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(635824350867325226L)]
    public class EntranceScreenComponent : LocalizedScreenComponent {
        [SerializeField] InputField loginField;

        [SerializeField] InputField passwordField;

        [SerializeField] InputField captchaField;

        [SerializeField] GameObject captcha;

        [SerializeField] Toggle rememberMeCheckbox;

        [SerializeField] Text enterNameOrEmail;

        [SerializeField] Text enterPassword;

        [SerializeField] Text enterSymbolsFromCaptcha;

        [SerializeField] Text rememberMe;

        [SerializeField] Text play;

        [SerializeField] Text forgetNickOrPassword;

        [SerializeField] Text registration;

        public virtual string Login {
            get => loginField.text;
            set => loginField.text = value;
        }

        public virtual string Password {
            get => passwordField.text;
            set => passwordField.text = value;
        }

        public virtual string Captcha {
            get => captchaField.text;
            set => captchaField.text = value;
        }

        public virtual bool RememberMe {
            get => rememberMeCheckbox.isOn;
            set => rememberMeCheckbox.isOn = value;
        }

        public string EnterNameOrEmail {
            set => enterNameOrEmail.text = value;
        }

        public string EnterPassword {
            set => enterPassword.text = value;
        }

        public string EnterSymbolsFromCaptcha {
            set => enterSymbolsFromCaptcha.text = value;
        }

        public string RememberMeCheckbox {
            set => rememberMe.text = value;
        }

        public string Play {
            set => play.text = value;
        }

        public string ForgetNickOrPassword {
            set => forgetNickOrPassword.text = value;
        }

        public string Registration {
            set => registration.text = value;
        }

        public string IncorrectPassword { get; set; }

        public string IncorrectCaptcha { get; set; }

        public string IncorrectLogin { get; set; }

        public string UnconfirmedEmail { get; set; }

        protected void OnEnable() {
            captcha.SetActive(false);
            captchaField.gameObject.SetActive(false);
        }

        public virtual void ActivateCaptcha() {
            captchaField.gameObject.SetActive(true);
            captcha.SetActive(true);
        }
    }
}