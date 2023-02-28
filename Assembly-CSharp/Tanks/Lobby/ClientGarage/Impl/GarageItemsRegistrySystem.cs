using System;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageItemsRegistrySystem : ECSSystem, GarageItemsRegistry {
        readonly HashSet<Avatar> avatars = new();

        readonly Dictionary<long, GarageItem> cache = new();

        readonly HashSet<VisualItem> coatings = new();

        readonly HashSet<ContainerBoxItem> containers = new();

        readonly HashSet<DetailItem> details = new();

        readonly HashSet<TankPartItem> hulls = new();

        readonly HashSet<ModuleItem> modules = new();

        readonly HashSet<VisualItem> paints = new();

        readonly HashSet<PremiumItem> premiumBoost = new();

        readonly HashSet<PremiumItem> premiumQuests = new();

        readonly HashSet<TankPartItem> turrets = new();

        public ICollection<TankPartItem> Hulls => hulls;

        public ICollection<TankPartItem> Turrets => turrets;

        public ICollection<VisualItem> Paints => paints;

        public ICollection<VisualItem> Coatings => coatings;

        public ICollection<ContainerBoxItem> Containers => containers;

        public ICollection<DetailItem> Details => details;

        public ICollection<ModuleItem> Modules => modules;

        public T GetItem<T>(Entity marketEntity) where T : GarageItem, new() => GetItem<T>(marketEntity.Id);

        public T GetItem<T>(long marketId) where T : GarageItem, new() {
            if (!cache.ContainsKey(marketId)) {
                cache.Add(marketId, new T());
            }

            return cache[marketId] as T;
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<AvatarItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(Avatar avatarItem) {
                avatarItem.MarketItem = item.Entity;
            }, avatars);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<AvatarItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(Avatar avatarItem) {
                avatarItem.UserItem = item.Entity;
            }, avatars);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PremiumBoostItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(PremiumItem premiumBoostItem) {
                premiumBoostItem.MarketItem = item.Entity;
            }, premiumBoost);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PremiumQuestItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(PremiumItem premiumQuestItem) {
                premiumQuestItem.MarketItem = item.Entity;
            }, premiumQuests);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ContainerMarkerComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(ContainerBoxItem containerItem) {
                containerItem.MarketItem = item.Entity;
            }, containers);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<ContainerMarkerComponent> item,
            [JoinByUser] [Context] UserNode self) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(ContainerBoxItem containerItem) {
                containerItem.UserItem = item.Entity;
            }, containers);
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<ItemsContainerItemComponent> itemNode) {
            ContainerBoxItem item = GetItem<ContainerBoxItem>(itemNode.marketItemGroup.Key);
            containers.Remove(item);
        }

        [OnEventFire]
        public void ItemCreated(ItemsCountChangedEvent e, UserItemNode<ContainerMarkerComponent> item, [JoinByUser] UserNode self, UserItemNode<ContainerMarkerComponent> item1,
            [JoinByMarketItem] BaseMarketItemNode marketItem) {
            if (e.Delta > 0) {
                AddOrUpdate(item.marketItemGroup.Key, delegate(ContainerBoxItem containerItem) {
                    containerItem.UserItem = item.Entity;
                });
            } else {
                AddOrUpdate(item.marketItemGroup.Key, delegate(ContainerBoxItem containerItem) {
                    containerItem.Opend();
                });
            }
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<TankItemComponent> item, [Context] [JoinByParentGroup] DefaultSkinNode skin) {
            AddOrUpdate(item.Entity.Id, delegate(TankPartItem partItem) {
                partItem.MarketItem = item.Entity;
                partItem.Preview = skin.imageItem.SpriteUid;
            }, hulls);
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<TankItemComponent> itemNode) {
            TankPartItem item = GetItem<TankPartItem>(itemNode.marketItemGroup.Key);
            hulls.Remove(item);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<TankItemComponent> item,
            [JoinByUser] [Context] UserNode self) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(TankPartItem partItem) {
                partItem.UserItem = item.Entity;
            }, hulls);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<WeaponItemComponent> item, [Context] [JoinByParentGroup] DefaultSkinNode skin) {
            AddOrUpdate(item.Entity.Id, delegate(TankPartItem partItem) {
                partItem.MarketItem = item.Entity;
                partItem.Preview = skin.imageItem.SpriteUid;
            }, turrets);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<WeaponItemComponent> item,
            [JoinByUser] [Context] UserNode self) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(TankPartItem partItem) {
                partItem.UserItem = item.Entity;
            }, turrets);
        }

        [OnEventFire]
        public void ItemRemoved(NodeRemoveEvent e, UserItemNode<WeaponItemComponent> itemNode) {
            TankPartItem item = GetItem<TankPartItem>(itemNode.marketItemGroup.Key);
            turrets.Remove(item);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketParentItemNode<SkinItemComponent> item) {
            TankPartItem item2 = GetItem<TankPartItem>(item.parentGroup.Key);

            VisualItem visualItem2 = AddOrUpdate(item.Entity.Id, delegate(VisualItem visualItem) {
                visualItem.MarketItem = item.Entity;
            });

            if (visualItem2 == null) {
                Console.WriteLine("GarageItemsRegistrySystem.ItemCreated error. Skin created error with id: " + item.Entity.Id);
            }

            item2.Skins.Add(visualItem2);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<SkinItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(VisualItem visualItem) {
                visualItem.UserItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<TankPaintItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(VisualItem visualItem) {
                visualItem.MarketItem = item.Entity;
            }, paints);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<TankPaintItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(VisualItem visualItem) {
                visualItem.UserItem = item.Entity;
            }, paints);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<WeaponPaintItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(VisualItem visualItem) {
                visualItem.MarketItem = item.Entity;
            }, coatings);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<WeaponPaintItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(VisualItem visualItem) {
                visualItem.UserItem = item.Entity;
            }, coatings);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<GraffitiItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(VisualItem visualItem) {
                visualItem.MarketItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<GraffitiItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(VisualItem visualItem) {
                visualItem.UserItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ShellItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(VisualItem visualItem) {
                visualItem.MarketItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<ShellItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(VisualItem visualItem) {
                visualItem.UserItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<PresetItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(GarageItem garageItem) {
                garageItem.MarketItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<DetailItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(DetailItem garageItem) {
                garageItem.MarketItem = item.Entity;
            }, details);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<DetailItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(DetailItem garageItem) {
                garageItem.UserItem = item.Entity;
            }, details);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<GoldBonusItemComponent> item) {
            AddOrUpdate(item.Entity.Id, delegate(GarageItem garageItem) {
                garageItem.MarketItem = item.Entity;
            });
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context] [JoinByParentGroup] Optional<UserItemNode<ModuleCardItemComponent>> card) {
            AddOrUpdate(item.Entity.Id, delegate(ModuleItem garageItem) {
                garageItem.MarketItem = item.Entity;

                if (card.IsPresent()) {
                    garageItem.CardItem = card.Get().Entity;
                }
            }, modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context] [JoinByParentGroup] Optional<MarketItemNode<ModuleCardItemComponent>> card) {
            AddOrUpdate(item.Entity.Id, delegate(ModuleItem garageItem) {
                garageItem.MarketItem = item.Entity;

                if (card.IsPresent()) {
                    garageItem.MarketCardItem = card.Get().Entity;
                }
            }, modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, MarketItemNode<ModuleItemComponent> item, [Context] [JoinByMarketItem] ModuleEffectUpgradablePropertyNode property) {
            AddOrUpdate(item.Entity.Id, delegate(ModuleItem garageItem) {
                garageItem.MarketItem = item.Entity;
                garageItem.Property = property.Entity;
            }, modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, BaseMarketItemNode marketItem, [Context] [JoinByMarketItem] [Combine] UserItemNode<ModuleItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(ModuleItem garageItem) {
                garageItem.UserItem = item.Entity;
            }, modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeAddedEvent e, UserItemNode<ModuleItemComponent> item, [Context] [JoinByModule] SlotNode slot) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(ModuleItem garageItem) {
                garageItem.UserItem = item.Entity;
                garageItem.Slot = slot.Entity;
            }, modules);
        }

        [OnEventFire]
        public void ItemCreated(NodeRemoveEvent e, SlotNode slot, [Context] [JoinByModule] UserItemNode<ModuleItemComponent> item) {
            AddOrUpdate(item.marketItemGroup.Key, delegate(ModuleItem garageItem) {
                garageItem.UserItem = item.Entity;
                garageItem.Slot = null;
            }, modules);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<TankPaintBattleItemComponent> item) {
            CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<WeaponPaintBattleItemComponent> item) {
            CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<WeaponSkinBattleItemComponent> item) {
            CacheBattleItem(item);
        }

        [OnEventFire]
        public void BattleItemCreated(NodeAddedEvent e, BattleItemNode<HullSkinBattleItemComponent> item) {
            CacheBattleItem(item);
        }

        void CacheBattleItem(BattleItemNode item) {
            GetItem<GarageItem>(item.marketItemGroup.Key).ConfigPath = ((EntityImpl)item.Entity).TemplateAccessor.Get().ConfigPath;
        }

        [OnEventFire]
        public void SendMounted(NodeAddedEvent e, UserItemNode<MountedItemComponent> item) {
            if (cache.ContainsKey(item.marketItemGroup.Key)) {
                cache[item.marketItemGroup.Key].Mounted();
            }
        }

        T AddOrUpdate<T>(long id, Action<T> update, HashSet<T> set = null) where T : GarageItem, new() {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(id);
            T item = GetItem<T>(id);

            if (item == null) {
                return null;
            }

            if (set != null) {
                set.Add(item);
            }

            update(item);
            entity.AddComponentIfAbsent<GarageMarketItemRegisteredComponent>();
            return item;
        }

        public class BaseMarketItemNode : Node {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MarketItemNode<T> : BaseMarketItemNode {
            public T marker;
        }

        public class MarketParentItemNode<T> : MarketItemNode<T> {
            public ParentGroupComponent parentGroup;
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public SelfComponent self;

            public UserGroupComponent userGroup;

            public UserItemComponent userItem;
        }

        public class UserItemNode<T> : UserItemNode {
            public T marker;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class BattleItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
        }

        public class BattleItemNode<T> : BattleItemNode {
            public T marker;
        }

        public class DefaultSkinNode : Node {
            public DefaultSkinItemComponent defaultSkinItem;

            public ImageItemComponent imageItem;

            public ParentGroupComponent parentGroup;
            public SkinItemComponent skinItem;
        }

        public class ModuleEffectUpgradablePropertyNode : Node {
            public MarketItemGroupComponent marketItemGroup;

            public ModuleVisualPropertiesComponent moduleVisualProperties;
        }

        public class SlotNode : Node {
            public ModuleGroupComponent moduleGroup;

            public SlotTankPartComponent slotTankPart;

            public SlotUserItemInfoComponent slotUserItemInfo;
            public UserItemComponent userItem;
        }
    }
}