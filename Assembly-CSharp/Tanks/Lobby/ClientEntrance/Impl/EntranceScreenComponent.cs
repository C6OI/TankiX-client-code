using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [SerialVersionUID(635824350867325226L)]
    public class EntranceScreenComponent : LocalizedScreenComponent, NoScaleScreen {
        [SerializeField] TMP_InputField loginField;

        [SerializeField] TMP_InputField passwordField;

        [SerializeField] TMP_InputField captchaField;

        [SerializeField] GameObject captcha;

        [SerializeField] Toggle rememberMeCheckbox;

        [SerializeField] TextMeshProUGUI serverStatusLabel;

        public GameObject loginText;

        public GameObject loginWaitIndicator;

        public GameObject locker;

        [SerializeField] TextMeshProUGUI enterNameOrEmail;

        [SerializeField] TextMeshProUGUI enterPassword;

        [SerializeField] TextMeshProUGUI rememberMe;

        [SerializeField] TextMeshProUGUI play;

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

        public virtual string serverStatus {
            get => serverStatusLabel.text;
            set => serverStatusLabel.text = value;
        }

        public string EnterNameOrEmail {
            set => enterNameOrEmail.text = value;
        }

        public string EnterPassword {
            set => enterPassword.text = value;
        }

        public string RememberMeCheckbox {
            set => rememberMe.text = value;
        }

        public string Play {
            set => play.text = value;
        }

        public string IncorrectPassword { get; set; }

        public string IncorrectCaptcha { get; set; }

        public string IncorrectLogin { get; set; }

        public string UnconfirmedEmail { get; set; }

        protected void OnEnable() {
            LockScreen(false);
            captcha.SetActive(false);
            captchaField.gameObject.SetActive(false);
        }

        public virtual void ActivateCaptcha() {
            captchaField.gameObject.SetActive(true);
            captcha.SetActive(true);
            GetComponent<Animator>().SetBool("captcha", true);
        }

        public void SetOfflineStatus() {
            SetServerStatus("Offline", "#E93A3AFF");
            Debug.Log("Set OFFLINE");
        }

        public void SetOnlineStatus() {
            SetServerStatus("Online", "#B6FF19FF");
        }

        void SetServerStatus(string text, string color) {
            string text2 = LocalizationUtils.Localize("d2788af7-8f66-4445-8154-d1e9c04af353");
            serverStatus = text2 + ": <color=" + color + ">" + text + "</color>";
        }

        public void SetWaitIndicator(bool wait) {
            loginText.SetActive(!wait);
            loginWaitIndicator.SetActive(wait);
        }

        public void LockScreen(bool value) {
            locker.SetActive(value);
        }
    }
}