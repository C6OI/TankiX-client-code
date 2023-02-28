using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class UpgradeModuleBaseButtonComponent : BehaviourComponent {
        [SerializeField] protected TextMeshProUGUI titleText;

        [SerializeField] protected LocalizedField activate;

        [SerializeField] protected LocalizedField upgrade;

        [SerializeField] protected LocalizedField fullUpgraded;

        [SerializeField] protected Image border;

        [SerializeField] protected Image fill;

        [SerializeField] protected Color notEnoughColor;

        [SerializeField] protected Color notEnoughFillColor;

        [SerializeField] protected Color enoughColor;

        [SerializeField] protected Color notEnoughTextColor;

        [SerializeField] protected Color enoughTextColor;

        [SerializeField] protected GameObject notEnoughText;

        [SerializeField] protected LocalizedField notEnoughTextStart;

        public string TitleTextUpgrade {
            get => titleText.text;
            set => titleText.text = upgrade.Value + " " + value;
        }

        public string BuyCrystal {
            set => titleText.text = value;
        }

        public bool NotEnoughTextEnable {
            set => notEnoughText.SetActive(value);
        }

        public long NotEnoughText {
            set => notEnoughText.GetComponent<TextMeshProUGUI>().text = string.Format(notEnoughTextStart, value);
        }

        public string TitleTextActivate {
            get => titleText.text;
            set => titleText.text = activate.Value + " " + value;
        }

        public virtual void Setup(int moduleLevel, int cardsCount, int maxCardCount, int price, int priceXCry, int userCryCount, int userXCryCount) {
            throw new NotImplementedException();
        }

        public void FullUpgraded() {
            titleText.text = fullUpgraded.Value;
            Image image = border;
            Color color = notEnoughColor;
            titleText.color = color;
            color = color;
            fill.color = color;
            image.color = color;
        }

        public void Activate() { }
    }
}