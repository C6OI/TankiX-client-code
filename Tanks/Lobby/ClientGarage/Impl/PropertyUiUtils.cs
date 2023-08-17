using System.Linq;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public static class PropertyUiUtils {
        public static void ShowProperty<T>(this ItemPropertiesScreenComponent propertiesScreen) where T : MonoBehaviour =>
            ShowPropertyAndGetPropertyObject<T>(propertiesScreen);

        public static void ShowPropertyAndSetValue<T>(this ItemPropertiesScreenComponent propertiesScreen, float value)
            where T : MonoBehaviour {
            GameObject gameObject = ShowPropertyAndGetPropertyObject<T>(propertiesScreen);
            gameObject.GetComponent<PropertyUIComponent>().SetValue(value);
            gameObject.GetComponent<PropertyUIComponent>().SetNextValue(value);
        }

        public static void ShowPropertyAndSetValue<T>(this ItemPropertiesScreenComponent propertiesScreen, float from,
            float to) where T : MonoBehaviour {
            GameObject gameObject = ShowPropertyAndGetPropertyObject<T>(propertiesScreen);
            gameObject.GetComponent<PropertyUIComponent>().SetValue(from, to);
            gameObject.GetComponent<PropertyUIComponent>().SetNextValue(from, to);
        }

        static GameObject ShowPropertyAndGetPropertyObject<T>(ItemPropertiesScreenComponent propertiesScreen)
            where T : MonoBehaviour {
            T firstComponentInChildren = GetFirstComponentInChildren<T>(propertiesScreen);
            GameObject gameObject = firstComponentInChildren.gameObject;
            gameObject.SetActive(true);
            return gameObject;
        }

        static T GetFirstComponentInChildren<T>(MonoBehaviour monoBehaviour) where T : MonoBehaviour =>
            monoBehaviour.GetComponentsInChildren<T>(true).Single();

        public static bool IsOverUpgradeItem(UpgradeLevelItemComponent upgradeLevel,
            ProficiencyLevelItemComponent proficiencyLevel) => upgradeLevel.Level >= proficiencyLevel.Level;
    }
}