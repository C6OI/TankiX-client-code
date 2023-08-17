using System.Globalization;
using UnityEngine;

namespace Platform.Library.ClientLocale.API {
    public class LocaleUtils {
        public static string LOCALE_SETTING_NAME = "locale";

        public static string GetSavedLocaleCode() => PlayerPrefs.GetString(LOCALE_SETTING_NAME);

        public static void SaveLocaleCode(string code) => PlayerPrefs.SetString(LOCALE_SETTING_NAME, code);

        public static CultureInfo GetCulture() {
            switch (GetSavedLocaleCode()) {
                case "en":
                    return new CultureInfo("en-US");

                default:
                    return new CultureInfo("ru-RU");
            }
        }
    }
}