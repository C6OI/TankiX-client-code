using System;
using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ShopBadgeComponent : BehaviourComponent {
        static bool promoAvailable;

        static bool saleAvailable;

        static bool specialOfferAvailable;

        static bool personalDiscountAvailable;

        static bool notificationAvailable = true;

        [SerializeField] Image saleIcon;

        [SerializeField] Image specialIcon;

        [SerializeField] Image promoIcon;

        [SerializeField] List<PromoBadge> promoBadges;

        public bool PromoAvailable => promoAvailable;

        public bool SaleAvailable {
            get => saleAvailable;
            set {
                saleAvailable = value;
                UpdateIcons();
            }
        }

        public bool SpecialOfferAvailable {
            get => specialOfferAvailable;
            set {
                specialOfferAvailable = value;
                UpdateIcons();
            }
        }

        public bool PersonalDiscountAvailable {
            get => personalDiscountAvailable;
            set {
                personalDiscountAvailable = value;
                UpdateIcons();
            }
        }

        public bool NotificationAvailable {
            get => notificationAvailable;
            set {
                notificationAvailable = value;
                UpdateIcons();
            }
        }

        void OnEnable() {
            NotificationAvailable = notificationAvailable;
        }

        public void SetPromoAvailable(string Key, bool available) {
            if (available && promoBadges.Exists(x => x.Key == Key)) {
                promoAvailable = true;
                promoIcon.sprite = promoBadges.Find(x => x.Key == Key).Sprite;
            } else {
                promoAvailable = false;
            }

            UpdateIcons();
        }

        void UpdateIcons() {
            if (specialIcon == null || saleIcon == null || promoIcon == null) {
                return;
            }

            if (promoAvailable) {
                specialIcon.gameObject.SetActive(false);
                saleIcon.gameObject.SetActive(false);
                promoIcon.gameObject.SetActive(true);
                return;
            }

            promoIcon.gameObject.SetActive(false);

            if (personalDiscountAvailable && notificationAvailable) {
                specialIcon.gameObject.SetActive(true);
                saleIcon.gameObject.SetActive(false);
            } else if (saleAvailable && notificationAvailable) {
                specialIcon.gameObject.SetActive(false);
                saleIcon.gameObject.SetActive(true);
            } else {
                specialIcon.gameObject.SetActive(false);
                saleIcon.gameObject.SetActive(false);
            }
        }

        [Serializable]
        class PromoBadge {
            public string Key;

            public Sprite Sprite;
        }
    }
}