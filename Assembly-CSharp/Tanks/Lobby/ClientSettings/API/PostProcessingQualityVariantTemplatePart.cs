using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientSettings.API {
    [TemplatePart]
    public interface PostProcessingQualityVariantTemplatePart : SettingsTemplate, Template {
        [AutoAdded]
        PostProcessingQualityVariantComponent postProcessingQualitySettings();
    }
}