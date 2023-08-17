using Lobby.ClientControls.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentMethodContent : ListItemEntityContent {
        [SerializeField] ImageListSkin skin;

        void SetProviderName(string name) => skin.SelectSprite(name);

        protected override void FillFromEntity(Entity entity) =>
            SetProviderName(entity.GetComponent<PaymentMethodComponent>().ProviderName);
    }
}