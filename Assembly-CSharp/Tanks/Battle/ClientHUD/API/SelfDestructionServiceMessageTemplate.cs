using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1442307333986L)]
    public interface SelfDestructionServiceMessageTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        SelfDestructionMessageComponent selfDestructionMessage();
    }
}