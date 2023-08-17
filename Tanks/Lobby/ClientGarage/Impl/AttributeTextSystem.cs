using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AttributeTextSystem : ECSSystem {
        [OnEventFire]
        public void ShowUpgradeLevelIfItemSelected(ListItemSelectedEvent e, ItemNode item,
            [JoinAll] SingleNode<ActiveScreenComponent> screen, [JoinByScreen] UpgradeInfoUINode upgradeInfoUi) =>
            ShowUpgrade(item, upgradeInfoUi);

        [OnEventFire]
        public void ShowUpgradeLevelIfItemBought(NodeAddedEvent e, ItemWithLevelNode item,
            [JoinAll] SingleNode<ActiveScreenComponent> screen, [JoinByScreen] UpgradeInfoUINode upgradeInfoUi) =>
            ShowUpgrade(item, upgradeInfoUi);

        void ShowUpgrade(ItemNode item, UpgradeInfoUINode upgradeInfoUi) {
            UpdateTexts(item, upgradeInfoUi);
            upgradeInfoUi.itemAttributes.Show();
        }

        [OnEventFire]
        public void HideExperienceIfSelectItemMaxLevelProficiency(ListItemSelectedEvent e,
            SingleNode<ProficiencyMaxLevelItemComponent> item, [JoinAll] SingleNode<ActiveScreenComponent> screen,
            [JoinByScreen] SingleNode<ItemAttributesComponent> experience) => experience.component.HideExperience();

        [OnEventFire]
        public void HideUpgradeInfoIfNotUpgradableItem(ListItemSelectedEvent e, NotUpgradableItemNode item,
            [JoinAll] GarageItemsScreenNode screen, [JoinByScreen] UpgradeInfoUINode upgradeInfo) =>
            upgradeInfo.itemAttributes.Hide();

        [OnEventComplete]
        public void UpdateProficiency(ItemProficiencyUpdatedEvent e, ItemNode item,
            [JoinAll] UpgradeInfoUINode upgradeInfoUi) => UpdateTexts(item, upgradeInfoUi);

        [OnEventFire]
        public void ShowUpgradeLevel(NodeAddedEvent e, UpgradeInfoUINode upgradeInfo,
            [JoinByScreen] [Context] UpgradableItemScreenNode item) => ShowUpgrade(item, upgradeInfo);

        [OnEventFire]
        public void HideUpgradeLevelIfNotUpgradable(NodeAddedEvent e, UpgradeInfoUINode upgradeInfo,
            [JoinByScreen] [Context] NotUpgradableItemScreenNode item) => upgradeInfo.itemAttributes.Hide();

        [OnEventFire]
        public void HideExperienceIfMaxLevelProficiency(NodeAddedEvent e, UpgradeInfoUINode experience,
            [JoinByScreen] [Context] MaxLevelItemNode item) => experience.itemAttributes.HideExperience();

        void UpdateTexts(ItemNode item, UpgradeInfoUINode upgradeInfoUi) {
            upgradeInfoUi.itemAttributes.SetUpgradeLevel(item.upgradeLevelItem.Level);
            upgradeInfoUi.itemAttributes.SetProficiencyLevel(item.proficiencyLevelItem.Level);

            if (item.Entity.HasComponent<ExperienceToLevelUpItemComponent>()) {
                ExperienceToLevelUpItemComponent component = item.Entity.GetComponent<ExperienceToLevelUpItemComponent>();

                upgradeInfoUi.itemAttributes.SetExperience(component.RemainingExperience,
                    component.InitLevelExperience,
                    component.FinalLevelExperience);
            } else if (item.Entity.HasComponent<ProficiencyMaxLevelItemComponent>()) {
                upgradeInfoUi.itemAttributes.HideExperience();
            }

            ScheduleEvent(new UpdateItemPropertiesEvent(item.upgradeLevelItem.Level, item.proficiencyLevelItem.Level), item);
        }

        public class ItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;

            public ProficiencyLevelItemComponent proficiencyLevelItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }

        public class SkinNode : Node {
            public SkinItemComponent skinItem;
        }

        [Not(typeof(UpgradeLevelItemComponent))]
        public class NotUpgradableItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public GarageItemComponent garageItem;
        }

        public class UpgradableItemScreenNode : ItemNode {
            public ScreenGroupComponent screenGroup;
        }

        public class ItemWithLevelNode : ItemNode {
            public ExperienceToLevelUpItemComponent experienceToLevelUpItem;
        }

        public class NotUpgradableItemScreenNode : NotUpgradableItemNode {
            public ScreenGroupComponent screenGroup;
        }

        public class MaxLevelItemNode : UpgradableItemScreenNode {
            public ProficiencyMaxLevelItemComponent proficiencyMaxLevelItem;
        }

        public class UpgradeInfoUINode : Node {
            public ItemAttributesComponent itemAttributes;

            public ScreenGroupComponent screenGroup;
        }

        public class GarageItemsScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public GarageItemsScreenComponent garageItemsScreen;
        }
    }
}