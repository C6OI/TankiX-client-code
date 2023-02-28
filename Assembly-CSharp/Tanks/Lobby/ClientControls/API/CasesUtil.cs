using System;
using System.Collections.Generic;
using Platform.Library.ClientLocale.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public static class CasesUtil {
        public static CaseType GetCase(int count) {
            switch (LocaleUtils.GetCulture().TwoLetterISOLanguageName) {
                case "ru":
                    return GetRUCase(count);

                case "en":
                    return GetENCase(count);

                default:
                    return CaseType.DEFAULT;
            }
        }

        public static CaseType GetRUCase(int count) {
            if (count > 10 && count < 20) {
                return CaseType.DEFAULT;
            }

            if (count % 10 == 1) {
                return CaseType.ONE;
            }

            if (count % 10 == 2 || count % 10 == 3 || count % 10 == 4) {
                return CaseType.TWO;
            }

            return CaseType.DEFAULT;
        }

        public static CaseType GetENCase(int count) => count == 1 ? CaseType.ONE : CaseType.DEFAULT;

        public static string GetLocalizedCase(string localizedVariants, int count) {
            string result = localizedVariants;

            try {
                Dictionary<CaseType, string> dictionary = GetDictionary(localizedVariants);
                result = dictionary[GetCase(count)];
                return result;
            } catch (Exception) {
                Debug.LogError("Check string: " + localizedVariants);
                return result;
            }
        }

        public static Dictionary<CaseType, string> GetDictionary(string localizedVariants) {
            Dictionary<CaseType, string> dictionary = new();
            string[] array = localizedVariants.Split(' ');
            string[] array2 = array;

            foreach (string text in array2) {
                CaseType key = (CaseType)Enum.Parse(typeof(CaseType), text.Split(':')[0]);
                string value = text.Split(':')[1];
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}