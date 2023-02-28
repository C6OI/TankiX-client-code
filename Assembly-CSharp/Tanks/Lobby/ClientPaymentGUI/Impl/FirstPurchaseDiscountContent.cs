using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class FirstPurchaseDiscountContent : DealItemContent {
        public Image bannerImage;

        public Sprite bigBanner;

        public Sprite smallBanner;

        public GameObject bigDesc;

        public GameObject smallDesc;

        public TextMeshProUGUI BigFirstLine;

        public TextMeshProUGUI BigSecondLine;

        public TextMeshProUGUI BigThirdLine;

        public TextMeshProUGUI BigValue;

        public TextMeshProUGUI SmallFirstLine;

        public TextMeshProUGUI SmallSecondLine;

        public string Header { get; set; }

        public string DescriptionFirstLine { get; set; }

        public string DescriptionSecondLine { get; set; }

        public string Footer { get; set; }

        public double Discount {
            set {
                int num = Mathf.RoundToInt((float)value * 100f);
                BigValue.text = num + "%";
                SmallFirstLine.text = Header + " " + num + "%";
            }
        }

        void OnEnable() {
            SmallSecondLine.text = DescriptionFirstLine + " " + DescriptionSecondLine;
            BigFirstLine.text = Header;
            BigSecondLine.text = DescriptionFirstLine;
            BigThirdLine.text = DescriptionSecondLine;
        }

        public override void SetParent(Transform parent) {
            base.SetParent(parent);
            bool flag = parent.name == "Top";
            bannerImage.sprite = !flag ? smallBanner : bigBanner;
            bigDesc.SetActive(flag);
            smallDesc.SetActive(!flag);
        }
    }
}