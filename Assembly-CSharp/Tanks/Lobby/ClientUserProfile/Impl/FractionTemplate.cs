using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1544501900637L)]
    public interface FractionTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        FractionInfoComponent fractionInfo();
    }
}