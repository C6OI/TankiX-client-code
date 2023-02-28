using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientPayment.API;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class GiftsPromoComponent : BehaviourComponent, ContentWithOrder {
        public int order = 100;

        public int Order => order;

        public bool CanFillBigRow => true;

        public bool CanFillSmallRow => false;

        public void SetParent(Transform parent) {
            transform.SetParent(parent, false);
        }

        public void Show() {
            EngineService.Engine.ScheduleEvent(new GoToXCryShopScreen(), new EntityStub());
        }
    }
}