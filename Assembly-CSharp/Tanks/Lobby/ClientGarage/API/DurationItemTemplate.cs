using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513586038487L)]
    public interface DurationItemTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        DurationItemComponent durationItem();
    }
}