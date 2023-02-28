using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientLocale.Impl {
    [SerialVersionUID(635824351162635226L)]
    public class LocaleListComponent : Component {
        public List<string> Locales { get; set; }
    }
}