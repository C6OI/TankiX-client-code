using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    [TemplatePart]
    public interface CartridgeCaseSettingTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        CartridgeCaseSettingComponent cartridgeCaseSettingComponent();
    }
}