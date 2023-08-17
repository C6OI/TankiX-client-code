using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientLocale.API {
    [SerialVersionUID(635718871765800507L)]
    public interface LocaleTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LocaleComponent locale();
    }
}