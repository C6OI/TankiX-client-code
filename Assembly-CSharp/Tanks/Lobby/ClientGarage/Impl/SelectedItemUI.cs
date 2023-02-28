using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelectedItemUI : MonoBehaviour {
        [SerializeField] TextMeshProUGUI itemName;

        [SerializeField] TextMeshProUGUI feature;

        [SerializeField] MainVisualPropertyUI[] props;

        [SerializeField] AnimatedNumber mastery;

        [SerializeField] TextMeshProUGUI currentSkin;

        [SerializeField] UpgradeStars upgradeStars;

        [SerializeField] LocalizedField currentSkinLocalizedField;

        public TankPartItem TankPartItem { get; set; }

        public void SetStars(float coef) {
            upgradeStars.SetPower(coef);
        }

        public void Set(TankPartItem item, string skinName, int level) {
            TankPartItem = item;
            this.SendEvent<ListItemSelectedEvent>(item.UserItem);
            itemName.text = item.Name;
            mastery.Value = level;
            feature.text = item.Feature;
            currentSkin.text = currentSkinLocalizedField.Value + ": " + skinName;
            List<MainVisualProperty> mainProperties = item.MainProperties;

            for (int i = 0; i < mainProperties.Count; i++) {
                props[i].gameObject.SetActive(true);
                props[i].Set(mainProperties[i].Name, mainProperties[i].NormalizedValue);
            }

            for (int j = mainProperties.Count; j < props.Length; j++) {
                props[j].gameObject.SetActive(false);
            }
        }

        public void DisableMasteryElement() {
            mastery.gameObject.SetActive(false);
        }

        public void EnableMasteryElement() {
            mastery.gameObject.SetActive(true);
        }
    }
}