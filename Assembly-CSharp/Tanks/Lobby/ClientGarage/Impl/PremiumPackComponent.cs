using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumPackComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _daysText;

        [SerializeField] TextMeshProUGUI _daysDescriptionText;

        [SerializeField] TextMeshProUGUI _priceText;

        [SerializeField] GameObject _xCrystals;

        [SerializeField] GameObject _saleContainer;

        [SerializeField] TextMeshProUGUI _salePercentText;

        [SerializeField] PremiumLearnMoreButtonComponent _learnMoreButton;

        [SerializeField] PurchaseButtonComponent _premiumPurchaseButton;

        public string DaysText {
            set => _daysText.text = value;
        }

        public string DaysDescription {
            set => _daysDescriptionText.text = value;
        }

        public string Price {
            set => _priceText.text = value;
        }

        public bool HasXCrystals {
            set => _xCrystals.SetActive(value);
        }

        public float Discount {
            set {
                if (value > 0f) {
                    _saleContainer.SetActive(true);
                    _salePercentText.text = string.Format("-{0:0}%", value * 100f);
                } else {
                    _saleContainer.SetActive(false);
                }
            }
        }

        public int LearnMoreIndex {
            set => _learnMoreButton.idx = value;
        }

        public Entity GoodsEntity {
            set => _premiumPurchaseButton.GoodsEntity = value;
        }
    }
}