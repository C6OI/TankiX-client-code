using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class QuantumShopContent : DealItemContent {
        [SerializeField] Button button;

        public virtual string Title { get; set; }

        public virtual string Price { get; set; }

        public virtual string Description { get; set; }

        protected override void FillFromEntity(Entity entity) {
            string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
            banner.SpriteUid = spriteUid;
            Dictionary<int, int> packXPrice = entity.GetComponent<PackPriceComponent>().PackXPrice;
            title.text = string.Format(Title, packXPrice.Keys.First());
            description.text = Description;
            price.text = string.Format(Price, packXPrice.Values.First().ToString(), "<sprite=9>");
            base.FillFromEntity(entity);
        }
    }
}