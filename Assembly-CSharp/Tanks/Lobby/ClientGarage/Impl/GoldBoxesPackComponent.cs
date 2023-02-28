using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientPaymentGUI.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoldBoxesPackComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _cardNameText;

        [SerializeField] TextMeshProUGUI _boxCountText;

        [SerializeField] ImageSkin _imageSkin;

        [SerializeField] TextMeshProUGUI _priceText;

        [SerializeField] GameObject _hitMarkObject;

        [SerializeField] GameObject _discountMarkObject;

        [SerializeField] TextMeshProUGUI _discountMarkText;

        [SerializeField] PurchaseButtonComponent _purchaseButton;

        public string CardName {
            set => _cardNameText.text = value;
        }

        public long BoxCount {
            set => _boxCountText.text = "x" + value;
        }

        public string SpriteUid {
            set => _imageSkin.SpriteUid = value;
        }

        public string Price {
            set => _priceText.text = value;
        }

        public bool HitMarkEnabled {
            set => _hitMarkObject.SetActive(value);
        }

        public int Discount {
            set {
                if (value > 0) {
                    _discountMarkObject.SetActive(true);
                    _discountMarkText.text = "-" + value + "%";
                } else {
                    _discountMarkObject.SetActive(false);
                }
            }
        }

        public Entity GoodsEntity {
            set => _purchaseButton.GoodsEntity = value;
        }
    }
}