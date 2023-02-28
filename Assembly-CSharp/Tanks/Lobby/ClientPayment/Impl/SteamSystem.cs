using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SteamSystem : ECSSystem {
        [OnEventFire]
        public void Steam(GoToPaymentRequestEvent e, SingleNode<UserComponent> user, [JoinAll] SingleNode<SteamComponent> steam) {
            e.SteamIsActive = true;

            if (string.IsNullOrEmpty(SteamComponent.Ticket) || string.IsNullOrEmpty(steam.component.SteamID)) {
                ScheduleEvent<OpenGameCurrencyPaymentSectionEvent>(user);
                return;
            }

            e.SteamId = steam.component.SteamID;
            e.Ticket = SteamComponent.Ticket;
        }

        [OnEventFire]
        public void Steam(RetryGoToPaymentRequestEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent(new GoToPaymentRequestEvent(), user);
        }

        [OnEventFire]
        public void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponseEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user) {
            NewEvent(new SteamFinalizeTransactionEvent(e.OrderId, e.AppId, e.Autorized)).Attach(user).Schedule();
        }

        [OnEventFire]
        public void OnSteam(RequestRetrySteamAuthTicketEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<SteamComponent> steam) {
            steam.component.GetTicket(e.GoToPayment);
        }

        [OnEventFire]
        public void OnAuthSessionRecieved(SteamAuthSessionRecievedEvent @event, SingleNode<SteamComponent> steam) {
            if (@event.GoToPayment) {
                RetryGoToPaymentRequestEvent eventInstance = new();
                ScheduleEvent(eventInstance, steam);
            } else {
                RequestCheckSteamDlcInstalledEvent eventInstance2 = new();
                ScheduleEvent(eventInstance2, steam);
            }
        }

        [OnEventFire]
        public void OnSteam(NodeAddedEvent e, SingleNode<SteamComponent> steam, SingleNode<SelfUserComponent> user) {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket)) {
                CheckTicketRequestEvent eventInstance = new(steam.component.SteamID, SteamComponent.Ticket);
                NewEvent(eventInstance).Attach(user).Schedule();
            }
        }

        [OnEventFire]
        public void CheckDLC(RequestCheckSteamDlcInstalledEvent e, SingleNode<SteamComponent> steam, [JoinAll] SingleNode<SelfUserComponent> user) {
            CheckSteamDlcEvent eventInstance = new(steam.component.SteamID, SteamComponent.Ticket);
            NewEvent(eventInstance).Attach(user).Schedule();
        }

        [OnEventFire]
        public void CheckDLC(NodeAddedEvent e, SingleNode<SelfUserComponent> user, [JoinAll] SingleNode<SteamComponent> steam) {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket)) {
                CheckSteamDlcEvent eventInstance = new(steam.component.SteamID, SteamComponent.Ticket);
                NewEvent(eventInstance).Attach(user).Schedule();
            }
        }
    }
}