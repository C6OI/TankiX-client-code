using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DetailTargetTeleportView : MonoBehaviour {
        public ImageSkin[] imageSkins;

        public TextMeshProUGUI text;

        public BrokenBackView back;

        public Image fill;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void UpdateView(DailyBonusGarageItemReward detailMarketItem) {
            DetailItem detailItem = GarageItemsRegistry.GetItem<DetailItem>(detailMarketItem.MarketItemId);

            imageSkins.ForEach(delegate(ImageSkin i) {
                i.SpriteUid = detailItem.TargetMarketItem.Preview;
            });

            text.text = detailItem.TargetMarketItem.Name;
            fill.gameObject.SetActive(true);
            fill.fillAmount = 1f;
            back.BreakBack();
            back.enabled = true;
        }
    }
}