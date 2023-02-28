using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainerBonusView : MonoBehaviour {
        public ImageSkin imageSkin;

        public TextMeshProUGUI text;

        public GameObject back;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public void UpdateView(DailyBonusGarageItemReward containerItem) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(containerItem.MarketItemId);
            imageSkin.SpriteUid = item.Preview;
            text.text = item.Name.ToUpper();
            back.SetActive(true);
        }

        public void UpdateViewByMarketItem(long marketItem) {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(marketItem);
            imageSkin.SpriteUid = item.Preview;
            text.text = item.Name.ToUpper();
            back.SetActive(true);
        }
    }
}