using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1436343663226L)]
    public interface UpgradableUserItemTemplate : UserItemTemplate, Template {
        ExperienceToLevelUpItemComponent experienceToLevelUpItem();

        UpgradeLevelItemComponent upgradeLevelItem();

        UpgradeMaxLevelItemComponent upgradeMaxLevelItem();
    }
}