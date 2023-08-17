using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(Animator))]
    public class ExchangeConfirmationWindow : LocalizedControl {
        [SerializeField] Text questionText;

        [SerializeField] Text confirmText;

        [SerializeField] Text cancelText;

        [SerializeField] Text forText;

        [SerializeField] Button confirm;

        [SerializeField] Button cancel;

        [SerializeField] Text crystalsText;

        [SerializeField] Text xCrystalsText;

        Entity package;

        Entity screen;

        Entity user;

        [Inject] public static EngineService EngineService { get; set; }

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

        void Update() {
            if (InputMapping.Cancel) {
                OnCancel();
            }
        }

        public void Show(Entity user, Entity package, Entity screen) {
            this.user = user;
            this.package = package;
            this.screen = screen;
            gameObject.SetActive(true);
            GameCurrencyPackComponent component = package.GetComponent<GameCurrencyPackComponent>();
            GoodsXPriceComponent component2 = package.GetComponent<GoodsXPriceComponent>();
            crystalsText.text = (component.Amount + component.Bonus).ToStringSeparatedByThousands();
            xCrystalsText.text = component2.Price.ToStringSeparatedByThousands();
            confirm.Select();

            if (!screen.HasComponent<LockedScreenComponent>()) {
                screen.AddComponent<LockedScreenComponent>();
            }
        }

        void OnConfirm() {
            Hide();

            EngineService.ExecuteInFlow(delegate(Engine e) {
                e.NewEvent<ExchangeCrystalsEvent>().AttachAll(user, package).Schedule();
            });
        }

        void OnCancel() => Hide();

        void Hide() {
            EngineService.ExecuteInFlow(delegate {
                if (screen.HasComponent<LockedScreenComponent>()) {
                    screen.RemoveComponent<LockedScreenComponent>();
                }
            });

            GetComponent<Animator>().SetBool("Visible", false);
        }
    }
}