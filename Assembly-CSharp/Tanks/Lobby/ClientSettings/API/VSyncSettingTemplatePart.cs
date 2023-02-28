using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    [TemplatePart]
    public interface VSyncSettingTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        VSyncSettingComponent vSyncSettingComponent();
    }
}