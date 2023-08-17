using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.API {
    [TemplatePart]
    public interface MouseSettingsTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        GameMouseSettingsComponent gameMouseSettings();
    }
}