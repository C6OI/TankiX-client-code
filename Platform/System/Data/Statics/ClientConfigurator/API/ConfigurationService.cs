using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public interface ConfigurationService {
        bool HasConfig(string path);

        YamlNode GetConfig(string path);
    }
}