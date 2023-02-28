using System;
using Assets.platform.library.ClientResources.Scripts.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;

namespace Platform.Library.ClientResources.Impl {
    public class DiskCachingActivator : UnityAwareActivator<AutoCompleting> {
        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        protected override void Activate() {
            YamlNode config = ConfigurationService.GetConfig(ConfigPath.CLIENT_RESOURCES);
            string stringValue = config.GetStringValue("caching");
            string stringValue2 = config.GetStringValue("maximumAvailableDiskSpace");
            string stringValue3 = config.GetStringValue("expirationDelay");
            string stringValue4 = config.GetStringValue("compressionEnabled");
            DiskCaching.Enabled = Convert.ToBoolean(stringValue);
            DiskCaching.MaximumAvailableDiskSpace = Convert.ToInt64(stringValue2);
            DiskCaching.ExpirationDelay = Convert.ToInt32(stringValue3);
            DiskCaching.CompressionEnambled = Convert.ToBoolean(stringValue4);
        }
    }
}