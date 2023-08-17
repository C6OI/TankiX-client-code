using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1436343663226L)]
    public interface UpgradableUserItemTemplate : Template, UserItemTemplate {
        [AutoAdded]
        UpgradableItemComponent upgradableItem();

        ExperienceToLevelUpItemComponent experienceToLevelUpItem();

        ProficiencyLevelItemComponent proficiencyLevelItem();

        UpgradeLevelItemComponent upgradeLevelItem();

        NextUpgradePriceComponent nextUpgradePrice();

        ProficiencyMaxLevelItemComponent proficiencyMaxLevelItem();

        UpgradeMaxLevelItemComponent upgradeMaxLevelItem();
    }
}