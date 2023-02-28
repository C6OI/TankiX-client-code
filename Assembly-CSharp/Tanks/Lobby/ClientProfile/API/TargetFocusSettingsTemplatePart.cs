using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Lobby.ClientProfile.API {
    [TemplatePart]
    public interface TargetFocusSettingsTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        TargetFocusSettingsComponent targetFocusSettings();

        [AutoAdded]
        LaserSightSettingsComponent laserSightSettings();
    }
}