using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemAttributesLocalization : LocalizedControl {
        [SerializeField] Text upgradeLevelText;

        [SerializeField] Text experienceToLevelUpText;

        public override string YamlKey => "upgradeInfoText";

        public override string ConfigPath => "ui/screen/garageitempropertyscreen";

        public virtual string UpgradeLevelText {
            get => upgradeLevelText.text;
            set => upgradeLevelText.text = value;
        }

        public virtual string ExperienceToLevelUpText {
            get => experienceToLevelUpText.text;
            set => experienceToLevelUpText.text = value;
        }
    }
}