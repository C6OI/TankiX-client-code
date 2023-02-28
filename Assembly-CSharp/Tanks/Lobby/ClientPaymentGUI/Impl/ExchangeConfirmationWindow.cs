using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(Animator))]
    public class ExchangeConfirmationWindow : LocalizedControl {
        [SerializeField] Text questionText;

        [SerializeField] TextMeshProUGUI confirmText;

        [SerializeField] TextMeshProUGUI cancelText;

        [SerializeField] Text forText;

        [SerializeField] Button confirm;

        [SerializeField] Button cancel;

        [SerializeField] Text crystalsText;

        [SerializeField] Text xCrystalsText;

        Entity user;

        long xCrystals;

        public string QuestionText {
            set => questionText.text = value;
        }

        public string ConfirmText {
            set => confirmText.text = value;
        }

        public string CancelText {
            set => cancelText.text = value;
        }

        public string ForText {
            set => forText.text = value;
        }

        protected override void Awake() {
            base.Awake();
            confirm.onClick.AddListener(OnConfirm);
            cancel.onClick.AddListener(OnCancel);
        }

        public void Show(Entity user, long xCrystals, long crystals) {
            this.xCrystals = xCrystals;
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            this.user = user;
            gameObject.SetActive(true);
            crystalsText.text = crystals.ToStringSeparatedByThousands();
            xCrystalsText.text = xCrystals.ToStringSeparatedByThousands();
        }

        void OnConfirm() {
            Hide();

            EngineService.Engine.NewEvent(new ExchangeCrystalsEvent {
                XCrystals = xCrystals
            }).AttachAll(user).Schedule();
        }

        void OnCancel() {
            Hide();
        }

        void Hide() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetBool("Visible", false);
        }
    }
}