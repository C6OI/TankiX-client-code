using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Lobby.ClientProfile.API {
    [TemplatePart]
    public interface MouseSettingsTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        GameMouseSettingsComponent gameMouseSettings();
    }
}