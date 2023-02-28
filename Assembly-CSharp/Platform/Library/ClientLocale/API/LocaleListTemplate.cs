using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLocale.Impl;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientLocale.API {
    [SerialVersionUID(635718872080731997L)]
    public interface LocaleListTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        LocaleListComponent localeList();
    }
}