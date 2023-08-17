using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(1433752208915L)]
    public interface UserTemplate : Template {
        SelfComponent self();
    }
}