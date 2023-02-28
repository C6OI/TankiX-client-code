using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SynthUIComponent : BehaviourComponent {
        [SerializeField] TMP_InputField crystals;

        [SerializeField] TMP_InputField xCrystals;

        [SerializeField] long defaultXCrystalsAmount = 100L;

        [SerializeField] Animator synthButtonAnimator;

        [SerializeField] ExchangeConfirmationWindow exchangeConfirmation;

        bool calculating;

        void Awake() {
            crystals.onValueChanged.AddListener(CalculateXCrystals);
            crystals.onEndEdit.AddListener(RoundCrystals);
            xCrystals.onValueChanged.AddListener(CalculateCrystals);
        }

        void Start() {
            ValidateButton(long.Parse(crystals.text), long.Parse(xCrystals.text));
        }

        void OnEnable() {
            if (string.IsNullOrEmpty(xCrystals.text)) {
                xCrystals.text = defaultXCrystalsAmount.ToString();
                CalculateCrystals(xCrystals.text);
            }
        }

        void OnDisable() {
            crystals.text = string.Empty;
            xCrystals.text = string.Empty;
        }

        void RoundCrystals(string value) {
            CalculateCrystals(xCrystals.text);
        }

        public void SetCrystals(long cry) {
            crystals.text = cry.ToString();
            CalculateXCrystals(crystals.text);
        }

        public void SetXCrystals(long xcry) {
            xCrystals.text = xcry.ToString();
            CalculateCrystals(xCrystals.text);
        }

        void CalculateXCrystals(string value) {
            if (!calculating) {
                calculating = true;
                long result = 0L;
                long.TryParse(value, out result);
                long num = (long)(result / ExchangeRateComponent.ExhchageRate);
                ValidateButton(result, num);
                xCrystals.text = num.ToString();
                calculating = false;
            }
        }

        void CalculateCrystals(string value) {
            if (!calculating) {
                calculating = true;
                long result = 0L;
                long.TryParse(value, out result);
                long num = (long)(result * ExchangeRateComponent.ExhchageRate);
                ValidateButton(num, result);
                crystals.text = num.ToString();
                calculating = false;
            }
        }

        void ValidateButton(long crystals, long xCrystals) {
            if (gameObject.activeInHierarchy) {
                synthButtonAnimator.SetBool("Visible", crystals > 0 && xCrystals > 0 && xCrystals <= SelfUserComponent.SelfUser.GetComponent<UserXCrystalsComponent>().Money);
            }
        }

        public void OnSynth() {
            long num = long.Parse(xCrystals.text);
            exchangeConfirmation.Show(SelfUserComponent.SelfUser, num, (long)(ExchangeRateComponent.ExhchageRate * num));
        }

        public void OnXCrystalsChanged() {
            CalculateCrystals(xCrystals.text);
        }
    }
}