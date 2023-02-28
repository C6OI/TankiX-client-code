using System;
using System.Globalization;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent {
    public class TankPurchaseScreenComponent : PurchaseItemComponent {
        public TextMeshProUGUI actualPrice;

        public TextMeshProUGUI priceWithoutDiscount;

        public TextMeshProUGUI discount;

        public GameObject discountExplanationBlock;

        public Image tankImage;

        public Image backgroundImage;

        public Image[] modules;

        [Header("Support")] public Sprite supportTank;

        public Sprite supportTankBackgroundImage;

        public Sprite[] supportModules;

        [Header("Offensive")] public Sprite offensiveTank;

        public Sprite[] offensiveModules;

        public Sprite offensiveTankBackgroundImage;

        [Header("Annihilation")] public Sprite annihilationTank;

        public Sprite[] annihilationModules;

        public Sprite annihilationTankBackgroundImage;

        void Update() {
            if (InputMapping.Cancel) {
                CloseScreen();
            }
        }

        void OnDisable() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.Main);
        }

        public void InitiateScreen(GoodsPriceComponent offerGoodsPrice, DiscountComponent personalOfferDiscount, RentTankRole tankRole, ShopDialogs shopDialogs) {
            this.shopDialogs = shopDialogs;

            if (personalOfferDiscount.DiscountCoeff > 0f) {
                float num = RoundPrice(offerGoodsPrice.Price * (1f - personalOfferDiscount.DiscountCoeff));
                actualPrice.text = num + " " + offerGoodsPrice.Currency;
                priceWithoutDiscount.text = offerGoodsPrice.Price.ToString(CultureInfo.InvariantCulture);
                discount.text = string.Format("-{0}%", personalOfferDiscount.DiscountCoeff * 100f);
                SetDiscountObjects(true);
            } else {
                actualPrice.text = offerGoodsPrice.Price + " " + offerGoodsPrice.Currency;
                SetDiscountObjects(false);
            }

            SetWindowContent(tankRole);
        }

        float RoundPrice(double price) => (float)(Math.Round(price * 100.0) / 100.0);

        void SetDiscountObjects(bool state) {
            priceWithoutDiscount.gameObject.SetActive(state);
            discount.transform.parent.gameObject.SetActive(state);
            discountExplanationBlock.SetActive(state);
        }

        void SetWindowContent(RentTankRole role) {
            MainScreenComponent.Instance.OverrideOnBack(CloseScreen);
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.TankRent);

            switch (role) {
                case RentTankRole.ANNIHILATION: {
                    tankImage.sprite = annihilationTank;
                    backgroundImage.sprite = annihilationTankBackgroundImage;

                    for (int j = 0; j < modules.Length; j++) {
                        modules[j].sprite = annihilationModules[j];
                    }

                    break;
                }

                case RentTankRole.OFFENSIVE: {
                    tankImage.sprite = offensiveTank;
                    backgroundImage.sprite = offensiveTankBackgroundImage;

                    for (int k = 0; k < modules.Length; k++) {
                        modules[k].sprite = offensiveModules[k];
                    }

                    break;
                }

                case RentTankRole.SUPPORT: {
                    tankImage.sprite = supportTank;
                    backgroundImage.sprite = supportTankBackgroundImage;

                    for (int i = 0; i < modules.Length; i++) {
                        modules[i].sprite = supportModules[i];
                    }

                    break;
                }
            }
        }

        void CloseScreen() {
            gameObject.SetActive(false);
        }

        public void OpenPurchaseWindow(Entity entity, ShopDialogs dialogs = null) {
            if (dialogs != null) {
                shopDialogs = dialogs;
            }

            if (shopDialogs != null) {
                OnPackClick(entity);
            }
        }
    }
}