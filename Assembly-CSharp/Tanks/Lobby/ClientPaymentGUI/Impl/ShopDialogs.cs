using System.Collections.Generic;
using System.Linq;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientPayment.main.csharp.Impl.Platbox;
using lobby.modules.ClientPayment.Impl.Payguru;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ShopDialogs : ECSBehaviour {
        const int TrMaxMobilePrice = 120;

        static ShopDialogs instance;

        [SerializeField] PaymentMethodWindow paymentMethod;

        [SerializeField] PlatboxWindow platbox;

        [SerializeField] AdyenWindow adyen;

        [SerializeField] QiwiWindow qiwi;

        [SerializeField] PlatboxCheckoutWindow platboxCheckout;

        [SerializeField] PaymentProcessWindow process;

        [SerializeField] WarningWindowComponent warning;

        [SerializeField] PaymentErrorWindow error;

        [SerializeField] LocalizedField forText;

        [SerializeField] PaletteColorField xCrystalsColor;

        Entity item;

        List<Entity> methods;

        Entity selectedMethod;

        void Awake() {
            instance = this;
        }

        public void Show(Entity item, ICollection<Entity> methods, bool xCryPack, string itemDesc = "") {
            instance = this;
            this.item = item;
            this.methods = methods.ToList();

            if (item.HasComponent<SpecialOfferComponent>()) {
                this.methods.RemoveAll(x => !x.HasComponent<PaymentMethodComponent>() || x.GetComponent<PaymentMethodComponent>().IsTerminal);
            }

            GoodsPriceComponent component = item.GetComponent<GoodsPriceComponent>();

            if (component.Currency.ToLower() == "try" && component.Price > 120.0) {
                this.methods.RemoveAll(delegate(Entity x) {
                    string methodName = x.GetComponent<PaymentMethodComponent>().MethodName;
                    return methodName == "paybymeweb" || methodName == "paybymemobile";
                });
            }

            gameObject.SetActive(true);
            paymentMethod.Show(item, this.methods, Cancel, Proceed, itemDesc);
        }

        public void Cancel() {
            if (!process.gameObject.activeInHierarchy) {
                CloseAll();
            }
        }

        public void ShowCheckout(string transaction) {
            HideAllWindows();
            platboxCheckout.Show(item, selectedMethod, transaction, platbox.EnteredPhoneNumber, Cancel);
        }

        public void ShowQiwiError() {
            HideAllWindows();
            ShowQiwi(selectedMethod, qiwi.Account);
        }

        public void ShowError() {
            HideAllWindows();
            error.Show(item, selectedMethod, Cancel);
        }

        public void CloseAll() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetTrigger("cancel");
            HideAllWindows();
        }

        void HideAllWindows() {
            paymentMethod.gameObject.SetActive(false);
            platbox.gameObject.SetActive(false);
            platboxCheckout.gameObject.SetActive(false);
            process.gameObject.SetActive(false);
            qiwi.gameObject.SetActive(false);
            adyen.gameObject.SetActive(false);
            error.gameObject.SetActive(false);
            warning.gameObject.SetActive(false);
        }

        void ShowQiwi(Entity method, string acc = "") {
            qiwi.Show(item, method, acc, delegate {
                paymentMethod.Show(item, methods, Cancel, Proceed, string.Empty);
            }, delegate {
                SendStat(PaymentStatisticsAction.PROCEED, method);
                MainScreenComponent.Instance.OverrideOnBack(Cancel);
                Engine engine = EngineService.Engine;

                engine.NewEvent(new QiwiProcessPaymentEvent {
                    Account = qiwi.Account
                }).AttachAll(item, method).Schedule();
            });
        }

        void ShowPlatbox(Entity method) {
            platbox.Show(item, method, delegate {
                paymentMethod.Show(item, methods, Cancel, Proceed, string.Empty);
            }, delegate {
                SendStat(PaymentStatisticsAction.PROCEED, method);
                MainScreenComponent.Instance.OverrideOnBack(Cancel);
                Engine engine = EngineService.Engine;

                engine.NewEvent(new PlatBoxBuyGoodsEvent {
                    Phone = platbox.EnteredPhoneNumber
                }).AttachAll(item, method).Schedule();
            });
        }

        void ShowAdyen(Entity method) {
            adyen.Show(item, method, delegate {
                paymentMethod.Show(item, methods, Cancel, Proceed, string.Empty);
            }, delegate {
                SendStat(PaymentStatisticsAction.PROCEED, method);
                MainScreenComponent.Instance.OverrideOnBack(Cancel);
                process.Show(item, method);
            });
        }

        public void Proceed(Entity method) {
            selectedMethod = method;
            SendStat(PaymentStatisticsAction.MODE_SELECT, method);
            PaymentMethodComponent component = method.GetComponent<PaymentMethodComponent>();

            if (component.MethodName == PaymentMethodNames.PAYGURU) {
                NewEvent<PayguruProcessEvent>().AttachAll(method, item).Schedule();
                CloseAll();
            } else if (component.MethodName == PaymentMethodNames.MOBILE) {
                ShowPlatbox(method);
            } else if (component.MethodName == PaymentMethodNames.CREDIT_CARD && component.ProviderName == "adyen") {
                ShowAdyen(method);
            } else if (component.MethodName == PaymentMethodNames.QIWI_WALLET && component.ProviderName == "qiwi") {
                ShowQiwi(method, string.Empty);
            } else {
                NewEvent<ProceedToExternalPaymentEvent>().AttachAll(method, item).Schedule();
                SendStat(PaymentStatisticsAction.PROCEED, method);
                process.Show(item, method);
            }
        }

        void SendStat(PaymentStatisticsAction action, Entity method) {
            Engine engine = EngineService.Engine;

            engine.ScheduleEvent(new PaymentStatisticsEvent {
                Action = action,
                Item = item.Id,
                Screen = gameObject.name,
                Method = method.Id
            }, SelfUserComponent.SelfUser);
        }

        public static string FormatItem(Entity item, Entity method = null) {
            string text = string.Empty;
            GoodsComponent component = item.GetComponent<GoodsComponent>();

            if (item.HasComponent<SpecialOfferContentLocalizationComponent>()) {
                SpecialOfferContentLocalizationComponent component2 = item.GetComponent<SpecialOfferContentLocalizationComponent>();
                text = component2.Title + " ";
            } else if (item.HasComponent<XCrystalsPackComponent>()) {
                XCrystalsPackComponent component3 = item.GetComponent<XCrystalsPackComponent>();
                text = "<#" + instance.xCrystalsColor.Color.ToHexString() + ">";
                long num = (long)(component.SaleState.AmountMultiplier * component3.Amount);
                string text2 = (num + component3.Bonus).ToStringSeparatedByThousands();
                text += text2;
                text += "<sprite=9></color>";
            }

            GoodsPriceComponent component4 = item.GetComponent<GoodsPriceComponent>();
            double value = component4.Price;

            if (item.HasComponent<SpecialOfferComponent>()) {
                if (item.HasComponent<CustomOfferPriceForUIComponent>()) {
                    value = item.GetComponent<CustomOfferPriceForUIComponent>().Price;
                }
            } else {
                value = component4.Round(component.SaleState.PriceMultiplier * component4.Price);
            }

            return text + string.Format(" {0} {1} {2}", instance.forText.Value, value.ToStringSeparatedByThousands(), component4.Currency);
        }
    }
}