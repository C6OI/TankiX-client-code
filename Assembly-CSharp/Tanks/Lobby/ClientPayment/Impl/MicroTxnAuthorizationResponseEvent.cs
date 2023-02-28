using Platform.Kernel.ECS.ClientEntitySystem.API;
using Steamworks;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class MicroTxnAuthorizationResponseEvent : Event {
        public MicroTxnAuthorizationResponseEvent(MicroTxnAuthorizationResponse_t response) {
            OrderId = response.m_ulOrderID.ToString();
            AppId = response.m_unAppID;
            Autorized = response.m_bAuthorized != 0;
        }

        public string OrderId { get; set; }

        public long AppId { get; set; }

        public bool Autorized { get; set; }
    }
}