using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientLocale.API {
    public class LocaleComponent : Component {
        public string Code { get; set; }

        public string Caption { get; set; }

        public string LocalizedCaption { get; set; }
    }
}