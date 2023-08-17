using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemPropertySystem : ECSSystem {
        [OnEventFire]
        public void HideAll(HideItemPropertiesEvent e, SingleNode<ItemPropertiesScreenComponent> screen,
            [JoinAll] [Combine] SingleNode<PropertyUIComponent> propertyItem) =>
            propertyItem.component.gameObject.SetActive(false);

        [OnEventFire]
        public void UpdateProficiency(ItemProficiencyUpdatedEvent e,
            SingleNode<ProficiencyLevelItemComponent> proficiency) => proficiency.component.Level = e.Level;
    }
}