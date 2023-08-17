using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    [SerialVersionUID(1429771189777L)]
    public interface ClientSessionTemplate : Template {
        ClientSessionComponent clientSession();
    }
}