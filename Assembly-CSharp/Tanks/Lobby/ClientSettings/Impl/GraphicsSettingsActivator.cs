using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class GraphicsSettingsActivator : UnityAwareActivator<AutoCompleting> {
        [SerializeField] float defaultSaturationLevel = 0.6f;

        [SerializeField] int defaultVegetationLevel;

        [SerializeField] int defaultGrassLevel;

        [SerializeField] int defaultAntialiasingQuality;

        [SerializeField] int defaultRenderResolutionQuality;

        [SerializeField] int defaultAnisotropicQuality;

        [SerializeField] int defaultTextureQuality;

        [SerializeField] int defaultShadowQuality;

        [SerializeField] int defaultParticleQuality;

        [SerializeField] int defaultCartridgeCaseAmount;

        [SerializeField] int defaultVsyncQuality = 1;

        [SerializeField] bool isWindowedByDefault;

        [SerializeField] int minHeight = 696;

        [SerializeField] int minWidth = 1024;

        [SerializeField] string configPath;

        [SerializeField] GraphicsSettingsAnalyzer graphicsSettingsAnalyzer;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        protected override void Activate() {
            Quality.ValidateQualities();
            graphicsSettingsAnalyzer.Init();
            GraphicsSettings graphicsSettings2 = GraphicsSettings.INSTANCE = new GraphicsSettings();
            graphicsSettings2.InitWindowModeSettings(isWindowedByDefault);
            graphicsSettings2.InitQualitySettings(graphicsSettingsAnalyzer.GetDefaultQuality(), UltraEnabled());
            Quality defaultQuality = graphicsSettingsAnalyzer.GetDefaultQuality();

            if (defaultQuality.Level == 0) {
                defaultGrassLevel = 0;
                defaultShadowQuality = 0;
                defaultParticleQuality = 0;
                defaultAnisotropicQuality = 0;
                defaultTextureQuality = 0;
                defaultVegetationLevel = 0;
                defaultAntialiasingQuality = 0;
                defaultRenderResolutionQuality = 1;
                defaultCartridgeCaseAmount = 0;
                defaultVsyncQuality = 1;
            }

            if (defaultQuality.Level == 1) {
                defaultGrassLevel = 0;
                defaultShadowQuality = 0;
                defaultParticleQuality = 1;
                defaultAnisotropicQuality = 0;
                defaultTextureQuality = 0;
                defaultVegetationLevel = 0;
                defaultAntialiasingQuality = 0;
                defaultRenderResolutionQuality = 0;
                defaultCartridgeCaseAmount = 0;
                defaultVsyncQuality = 1;
            }

            if (defaultQuality.Level == 2) {
                defaultGrassLevel = 1;
                defaultShadowQuality = 1;
                defaultParticleQuality = 2;
                defaultAnisotropicQuality = 0;
                defaultTextureQuality = 1;
                defaultVegetationLevel = 1;
                defaultAntialiasingQuality = 0;
                defaultRenderResolutionQuality = 0;
                defaultCartridgeCaseAmount = 0;
                defaultVsyncQuality = 1;
            }

            if (defaultQuality.Level == 3) {
                defaultGrassLevel = 2;
                defaultShadowQuality = 2;
                defaultParticleQuality = 3;
                defaultAnisotropicQuality = 1;
                defaultTextureQuality = 1;
                defaultVegetationLevel = 2;
                defaultAntialiasingQuality = 0;
                defaultRenderResolutionQuality = 0;
                defaultCartridgeCaseAmount = 1;
                defaultVsyncQuality = 1;
            }

            if (defaultQuality.Level == 4) {
                defaultGrassLevel = 3;
                defaultShadowQuality = 3;
                defaultParticleQuality = 4;
                defaultAnisotropicQuality = 2;
                defaultTextureQuality = 1;
                defaultVegetationLevel = 3;
                defaultAntialiasingQuality = 1;
                defaultRenderResolutionQuality = 0;
                defaultCartridgeCaseAmount = 2;
                defaultVsyncQuality = 1;
            }

            if (defaultQuality.Level == 5) {
                defaultGrassLevel = 4;
                defaultShadowQuality = 4;
                defaultParticleQuality = 5;
                defaultAnisotropicQuality = 2;
                defaultTextureQuality = 1;
                defaultVegetationLevel = 4;
                defaultAntialiasingQuality = 1;
                defaultRenderResolutionQuality = 0;
                defaultCartridgeCaseAmount = 3;
                defaultVsyncQuality = 1;
            }

            DefineScreenResolutionData(graphicsSettings2);
            graphicsSettings2.InitSaturationLevelSettings(defaultSaturationLevel);
            graphicsSettings2.InitAnisotropicQualitySettings(defaultAnisotropicQuality);
            graphicsSettings2.InitRenderResolutionQualitySettings(defaultRenderResolutionQuality);
            graphicsSettings2.InitAntialiasingQualitySettings(defaultAntialiasingQuality);
            graphicsSettings2.InitShadowQualitySettings(defaultShadowQuality);
            graphicsSettings2.InitParticleQualitySettings(defaultParticleQuality);
            graphicsSettings2.InitTextureQualitySettings(defaultTextureQuality);
            graphicsSettings2.InitVegetationLevelSettings(defaultVegetationLevel);
            graphicsSettings2.InitGrassLevelSettings(defaultGrassLevel);
            graphicsSettings2.InitCartridgeCaseAmount(defaultCartridgeCaseAmount);
            graphicsSettings2.InitVSyncQualitySettings(defaultVsyncQuality);

            if (!graphicsSettings2.NeedCompactWindow()) {
                graphicsSettings2.ApplyInitialScreenResolutionData();
                return;
            }

            CompactScreenBehaviour compactScreen = gameObject.AddComponent<CompactScreenBehaviour>();
            graphicsSettings2.EnableCompactScreen(compactScreen);
        }

        bool UltraEnabled() {
            YamlNode config = ConfigurationService.GetConfig(configPath);
            return bool.Parse(config.GetStringValue("ultraenabled"));
        }

        void DefineScreenResolutionData(GraphicsSettings graphicsSettings) {
            List<Resolution> list = FilterSmallResolutions();
            Resolution defaultResolution = graphicsSettingsAnalyzer.GetDefaultResolution(list);
            graphicsSettings.InitScreenResolutionSettings(list, defaultResolution);
        }

        List<Resolution> FilterSmallResolutions() {
            List<Resolution> list = new();
            Resolution[] resolutions = Screen.resolutions;
            int num = resolutions.Length;

            for (int i = 0; i < num; i++) {
                Resolution item = resolutions[i];

                if (item.width >= minWidth && item.height >= minHeight) {
                    list.Add(item);
                }
            }

            if (list.Count == 0) {
                list.Add(new Resolution {
                    height = minHeight,
                    width = minWidth
                });
            }

            return list;
        }
    }
}