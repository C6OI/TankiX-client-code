using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Tanks.Lobby.ClientControls.API {
    public class LocalizationUtils {
        public static readonly string CONFIG_PATH = "localization";

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        public static string Localize(string uid) {
            YamlNode config = ConfigurationService.GetConfig(CONFIG_PATH);

            try {
                Dictionary<object, object> dictionary = (Dictionary<object, object>)config.GetValue(uid);

                using (Dictionary<object, object>.Enumerator enumerator = dictionary.GetEnumerator()) {
                    if (enumerator.MoveNext()) {
                        return (string)enumerator.Current.Value;
                    }
                }

                return string.Empty;
            } catch (KeyNotFoundException) {
                return string.Empty;
            }
        }
    }
}