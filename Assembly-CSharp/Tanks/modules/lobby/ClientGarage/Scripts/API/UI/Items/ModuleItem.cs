using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items {
    public class ModuleItem : GarageItem {
        public ModuleBehaviourType ModuleBehaviourType => MarketItem.GetComponent<ModuleBehaviourTypeComponent>().Type;

        public int TierNumber => MarketItem.GetComponent<ModuleTierComponent>().TierNumber;

        public TankPartModuleType TankPartModuleType => MarketItem.GetComponent<ModuleTankPartComponent>().TankPart;

        public long Level {
            get {
                if (UserItem == null) {
                    return 0L;
                }

                return UserItem.GetComponent<ModuleUpgradeLevelComponent>().Level;
            }
        }

        public int MaxLevel {
            get {
                if (UserItem == null) {
                    return MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices.Count;
                }

                return UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices.Count;
            }
        }

        public ModulePrice CraftPrice => MarketItem.GetComponent<ModuleCardsCompositionComponent>().CraftPrice;

        public int UpgradePrice {
            get {
                if (UserItem == null) {
                    return 0;
                }

                if (Level < MaxLevel) {
                    return UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int)Level].Cards;
                }

                return 0;
            }
        }

        public int UpgradePriceCRY {
            get {
                if (UserItem == null) {
                    return 0;
                }

                if (Level < MaxLevel) {
                    return UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int)Level].Crystals;
                }

                return 0;
            }
        }

        public int UpgradePriceXCRY {
            get {
                if (UserItem == null) {
                    return 0;
                }

                if (Level < MaxLevel) {
                    return UserItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int)Level].XCrystals;
                }

                return 0;
            }
        }

        public Entity CardItem { get; set; }

        public long UserCardCount {
            get {
                if (CardItem == null) {
                    return 0L;
                }

                return CardItem.GetComponent<UserItemCounterComponent>().Count;
            }
        }

        public string CardSpriteUid {
            get {
                if (MarketCardItem == null) {
                    return string.Empty;
                }

                return MarketCardItem.GetComponent<ImageItemComponent>().SpriteUid;
            }
        }

        public Entity Property { get; set; }

        public Entity MarketCardItem { get; set; }

        public List<ModuleVisualProperty> properties {
            get {
                if (Property == null) {
                    return null;
                }

                return Property.GetComponent<ModuleVisualPropertiesComponent>().Properties;
            }
        }

        public Entity Slot { get; set; }

        public ModulePrice GetUpgradePrice(long level) {
            if (level < MaxLevel) {
                return MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[(int)level];
            }

            return MarketItem.GetComponent<ModuleCardsCompositionComponent>().UpgradePrices[MaxLevel];
        }

        public bool ResearchAvailable() => UserCardCount >= CraftPrice.Cards;

        public bool ImproveAvailable() {
            if (Level < MaxLevel) {
                return UserCardCount >= GetUpgradePrice(Level).Cards;
            }

            return false;
        }

        public bool IsMutable() => !MarketItem.HasComponent<ImmutableModuleItemComponent>();

        public new string Description() {
            DescriptionItemComponent component = MarketItem.GetComponent<DescriptionItemComponent>();
            return component.Description;
        }
    }
}