using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.API {
    [SerialVersionUID(1453800151787L)]
    public interface PaymentMethodTemplate : Template {
        PaymentMethodComponent PaymentMethod();
    }
}