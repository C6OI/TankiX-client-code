using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentSectionSystem : ECSSystem {
        [OnEventFire]
        public void ClosePaymentSection(NodeRemoveEvent e, SingleNode<SectionComponent> section,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent<ClosePaymentSectionEvent>(user);

        [OnEventFire]
        public void OpenUrl(GoToUrlToPayEvent e, Node node) =>
            ScheduleEvent<ShowScreenLeftEvent<GoodsSelectionScreenComponent>>(node);

        [OnEventFire]
        public void LeavePayment(NodeAddedEvent e, SingleNode<ScreenComponent> screen,
            [JoinAll] SingleNode<UserInPaymentSectionComponent> session) {
            if (screen.component.GetComponent<PaymentScreen>() == null) {
                session.Entity.RemoveComponent<UserInPaymentSectionComponent>();
            }
        }
    }
}