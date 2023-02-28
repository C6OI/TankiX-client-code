using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class TankPartItem : GarageItem {
        public enum TankPartItemType {
            Turret = 0,
            Hull = 1
        }

        readonly List<VisualItem> skins = new();

        public int UpgradeLevel {
            get {
                if (UserItem == null) {
                    return 0;
                }

                return UserItem.GetComponent<UpgradeLevelItemComponent>().Level;
            }
        }

        public int AbsExperience {
            get {
                ExperienceToLevelUpItemComponent component = UserItem.GetComponent<ExperienceToLevelUpItemComponent>();
                return component.FinalLevelExperience - component.RemainingExperience;
            }
        }

        public ExperienceToLevelUpItemComponent Experience => UserItem.GetComponent<ExperienceToLevelUpItemComponent>();

        public string Feature => MarketItem.GetComponent<VisualPropertiesComponent>().Feature;

        public List<MainVisualProperty> MainProperties => MarketItem.GetComponent<VisualPropertiesComponent>().MainProperties;

        public List<VisualProperty> Properties => MarketItem.GetComponent<VisualPropertiesComponent>().Properties;

        public TankPartItemType Type { get; private set; }

        public override Entity MarketItem {
            get => base.MarketItem;
            set {
                base.MarketItem = value;

                if (value.HasComponent<WeaponItemComponent>()) {
                    Type = TankPartItemType.Turret;
                    return;
                }

                if (value.HasComponent<TankItemComponent>()) {
                    Type = TankPartItemType.Hull;
                    return;
                }

                throw new Exception("Invalid tank part type");
            }
        }

        public ICollection<VisualItem> Skins => skins;
    }
}