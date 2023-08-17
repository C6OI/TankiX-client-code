using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1443003315944L)]
    public interface AutokickServiceMessageTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        AutokickMessageComponent autokickMessage();
    }
}