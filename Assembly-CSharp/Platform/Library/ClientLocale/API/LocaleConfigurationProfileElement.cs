using System;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLocale.Impl;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using UnityEngine;

namespace Platform.Library.ClientLocale.API {
    public class LocaleConfigurationProfileElement : MonoBehaviour, ConfigurationProfileElement {
        public string language;

        [Inject] public static ConfigurationService configurationService { get; set; }

        public string ProfileElement {
            get {
                language = LocaleUtils.GetSavedLocaleCode();

                if (string.IsNullOrEmpty(language)) {
                    DefaultLocaleConfiguration defaultLocaleConfiguration = null;

                    try {
                        defaultLocaleConfiguration = configurationService.GetConfig(ConfigPath.DEFAULT_LOCALE).ConvertTo<DefaultLocaleConfiguration>();
                    } catch (Exception ex) {
                        LoggerProvider.GetLogger(this).WarnFormat("Unable to read default lcoale  {0}. {1}", ex.Message, ex);
                    }

                    if (defaultLocaleConfiguration != null && !string.IsNullOrEmpty(defaultLocaleConfiguration.DefaultLocale)) {
                        language = defaultLocaleConfiguration.DefaultLocale;
                    } else {
                        switch (Application.systemLanguage) {
                            case SystemLanguage.Russian:
                                language = "ru";
                                break;

                            case SystemLanguage.English:
                                language = "en";
                                break;

                            case SystemLanguage.Turkish:
                                language = "tr";
                                break;

                            default:
                                language = "en";
                                break;
                        }
                    }
                }

                LocaleUtils.SaveLocaleCode(language);
                return language;
            }
        }
    }
}