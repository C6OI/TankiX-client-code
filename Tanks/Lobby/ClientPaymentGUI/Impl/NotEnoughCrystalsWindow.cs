using System.Collections.Generic;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(Animator))]
    public class NotEnoughCrystalsWindow : LocalizedControl {
        [SerializeField] Text needMoreText;

        [SerializeField] Text needMoreAfterText;

        [SerializeField] Text offerCryText;

        [SerializeField] Text offerXCryText;

        [SerializeField] Text yesText;

        [SerializeField] Text noText;

        [SerializeField] List<GameObject> cryIcons = new();

        [SerializeField] List<GameObject> xCryIcons = new();

        [SerializeField] Button yes;

        [SerializeField] Button no;

        [SerializeField] Text amountText;

        Entity screen;

        Entity user;

        [Inject] public static EngineService EngineService { get; set; }

        public string NeedMoreText {
            set => needMoreText.text = value;
        }

        public string NeedMoreAfterText {
            set => needMoreAfterText.text = value;
        }

        public string OfferCryText {
            set => offerCryText.text = value;
        }

        public string OfferXCryText {
            set => offerXCryText.text = value;
        }

        public string YesText {
            set => yesText.text = value;
        }

        public string NoText {
            set => noText.text = value;
        }

        protected override void Awake() {
            base.Awake();
            yes.onClick.AddListener(OnYes);
            no.onClick.AddListener(OnNo);
        }

        void Update() {
            if (InputMapping.Cancel) {
                OnNo();
            }
        }

        public void ShowForXCrystals(Entity user, Entity screen, long amount) {
            offerCryText.gameObject.SetActive(false);
            offerXCryText.gameObject.SetActive(true);

            foreach (GameObject cryIcon in cryIcons) {
                cryIcon.SetActive(false);
            }

            foreach (GameObject xCryIcon in xCryIcons) {
                xCryIcon.SetActive(true);
            }

            Show(user, screen, amount);
        }

        public void ShowForCrystals(Entity user, Entity screen, long amount) {
            offerCryText.gameObject.SetActive(true);
            offerXCryText.gameObject.SetActive(false);

            foreach (GameObject cryIcon in cryIcons) {
                cryIcon.SetActive(true);
            }

            foreach (GameObject xCryIcon in xCryIcons) {
                xCryIcon.SetActive(false);
            }

            Show(user, screen, amount);
        }

        void Show(Entity user, Entity screen, long amount) {
            this.user = user;
            this.screen = screen;
            gameObject.SetActive(true);
            amountText.text = amount.ToStringSeparatedByThousands() + " ";
            yes.Select();

            if (!screen.HasComponent<LockedScreenComponent>()) {
                screen.AddComponent<LockedScreenComponent>();
            }
        }

        void OnYes() {
            Hide();

            EngineService.ExecuteInFlow(delegate(Engine e) {
                if (offerXCryText.gameObject.activeSelf) {
                    e.ScheduleEvent<GoToPaymentRequestEvent>(user);
                } else {
                    e.ScheduleEvent<GoToExchangeEvent>(user);
                }
            });
        }

        void OnNo() => Hide();

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