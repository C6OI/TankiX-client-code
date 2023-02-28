using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1443003216475L)]
    public interface UpsideDownServiceMessageTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        UpsideDownMessageComponent upsideDownMessage();
    }
}