using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientSettings.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class TanksGraphicsSettingsAnalyzer : GraphicsSettingsAnalyzer {
        const string SPACE = " ";

        const string UNSUPPORTED_QUALITY_NAME = "Unsupported";

        [SerializeField] string configPath;

        [SerializeField] readonly Quality defaultQualityForUnknownDevice = Quality.medium;

        [SerializeField] readonly Quality maxDefaultQuality = Quality.maximum;

        [SerializeField] readonly Quality minQuality = Quality.mininum;

        Quality defaultQuality;

        bool unsupportedDevice;

        [Inject] public static ConfigurationService Configuration { get; set; }

        public override void Init() {
            Dictionary<string, string> configData = GetConfigData();
            string text = PrepareDeviceName(SystemInfo.graphicsDeviceName);

            if (!configData.ContainsKey(text)) {
                Console.WriteLine("Unknown device {0}! Default Quality Level = {1}",
                    text,
                    defaultQualityForUnknownDevice.Name);

                defaultQuality = defaultQualityForUnknownDevice;
                return;
            }

            string text2 = configData[text];

            if (text2.Equals("Unsupported")) {
                unsupportedDevice = true;
                defaultQuality = minQuality;
                Console.WriteLine("Unsupported device! Default Quality Level = " + minQuality.Name);
            } else {
                Quality qualityByName = Quality.GetQualityByName(text2);
                defaultQuality = qualityByName.Level <= maxDefaultQuality.Level ? qualityByName : maxDefaultQuality;
            }
        }

        public override Quality GetDefaultQuality() => defaultQuality;

        public override Resolution GetDefaultResolution(List<Resolution> resolutions) {
            Func<Resolution, int> keySelector = r => r.width + r.height;

            return !unsupportedDevice ? resolutions.OrderByDescending(keySelector).First()
                       : resolutions.OrderBy(keySelector).First();
        }

        Dictionary<string, string> GetConfigData() {
            YamlNode config = Configuration.GetConfig(configPath);
            Dictionary<string, string> source = config.ConvertTo<Dictionary<string, string>>();
            return source.ToDictionary(k => PrepareDeviceName(k.Key), k => k.Value);
        }

        static string PrepareDeviceName(string currentDeviceName) =>
            currentDeviceName.Replace(" ", string.Empty).Trim().ToUpperInvariant();
    }
}