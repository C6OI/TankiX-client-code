using System.Collections.Generic;
using Lobby.ClientSettings.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;

namespace Lobby.ClientSettings.Impl {
    public class GraphicsSettingsActivator : UnityAwareActivator<AutoCompleting> {
        [SerializeField] float defaultSaturationLevel = 0.6f;

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
            DefineScreenResolutionData(graphicsSettings2);
            graphicsSettings2.InitSaturationLevelSettings(defaultSaturationLevel);

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