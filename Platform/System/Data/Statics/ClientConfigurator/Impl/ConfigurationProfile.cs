namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public interface ConfigurationProfile {
        bool Match(string configName);
    }
}