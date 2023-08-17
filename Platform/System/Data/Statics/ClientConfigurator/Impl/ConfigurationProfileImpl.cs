using System.Linq;
using System.Text.RegularExpressions;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigurationProfileImpl : ConfigurationProfile {
        readonly string PROFILE_PATTERN = "_([A-Za-z0-9]+)";
        readonly string[] profileElements;

        readonly string PUBLIC_CONFIG_PATTERN = "public(_[A-Za-z0-9]+)*\\.yml";

        readonly Regex regexConfig;

        readonly Regex regexProfile;

        public ConfigurationProfileImpl(string[] profileElements = null) {
            this.profileElements = profileElements;
            regexProfile = new Regex(PROFILE_PATTERN);
            regexConfig = new Regex(PUBLIC_CONFIG_PATTERN);
        }

        public bool Match(string configName) {
            if (profileElements == null) {
                return configName.Equals("public.yml");
            }

            if (!regexConfig.IsMatch(configName)) {
                return false;
            }

            foreach (Match item in regexProfile.Matches(configName)) {
                if (!profileElements.Contains(item.Groups[1].Value)) {
                    return false;
                }
            }

            return true;
        }
    }
}