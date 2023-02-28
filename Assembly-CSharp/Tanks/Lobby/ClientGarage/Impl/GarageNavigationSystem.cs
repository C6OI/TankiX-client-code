using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageNavigationSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void ParseItemLink(ParseLinkEvent e, Node node) {
            long result;

            if (!e.Link.StartsWith("item/") || !long.TryParse(e.Link.Substring("item/".Length), out result)) {
                return;
            }

            if (!EngineService.EntityRegistry.ContainsEntity(result)) {
                e.ParseMessage = "Entity not found: " + result;
                return;
            }

            Entity entity = EngineService.EntityRegistry.GetEntity(result);

            if (!entity.HasComponent<MarketItemComponent>()) {
                e.ParseMessage = "Entity is not market item: " + entity;
                return;
            }

            e.CustomNavigationEvent = NewEvent(new ShowGarageItemEvent {
                Item = entity
            }).Attach(node);
        }

        [OnEventFire]
        public void ParseCategoryLink(ParseLinkEvent e, Node node) {
            if (e.Link.StartsWith("garage/")) {
                string text = e.Link.Substring("garage/".Length);
                GarageCategory garageCategory = ParseCategory(text);

                if (garageCategory == null) {
                    e.ParseMessage = "Category not found: " + text;
                    return;
                }

                e.CustomNavigationEvent = NewEvent(new ShowGarageCategoryEvent {
                    Category = garageCategory
                }).Attach(node);
            }
        }

        [OnEventFire]
        public void ShowGarageItem(ShowGarageItemEvent e, Node node) {
            GarageCategory categoryByItem = GetCategoryByItem(e.Item);
            Entity parentItem = GetParentItem(e.Item, categoryByItem);

            ScheduleEvent(new ShowGarageCategoryEvent {
                Category = categoryByItem,
                ParentItem = parentItem,
                SelectedItem = e.Item
            }, node);
        }

        [OnEventFire]
        public void ShowGarageCategory(ShowGarageCategoryEvent e, Node node, [JoinAll] ICollection<ItemNode> items) {
            Log.InfoFormat("ShowGarageCategory {0} ParentItem={1} SelectedItem={2}", e.Category, e.ParentItem, e.SelectedItem);
            MainScreenComponent instance = MainScreenComponent.Instance;

            if (!instance.gameObject.activeSelf) {
                instance.ShowHome();
            }

            CustomizationUIComponent componentInChildrenIncludeInactive = instance.GetComponentInChildrenIncludeInactive<CustomizationUIComponent>();

            if (e.ParentItem == null && e.Category.NeedParent) {
                e.ParentItem = e.Category != GarageCategory.MODULES ? instance.MountedTurret.UserItem : instance.MountedHull.UserItem;
            }

            if (e.Category == GarageCategory.CONTAINERS || e.Category == GarageCategory.BLUEPRINTS) {
                ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);

                if (!e.SelectedItem.HasComponent<HangarItemPreviewComponent>()) {
                    e.SelectedItem.AddComponent<HangarItemPreviewComponent>();
                }

                instance.ShowShopIfNotVisible();
                int num = e.Category == GarageCategory.BLUEPRINTS ? 1 : 2;

                if (ShopTabManager.Instance == null) {
                    ShopTabManager.shopTabIndex = num;
                } else {
                    ShopTabManager.Instance.Show(num);
                }
            } else if (e.Category == GarageCategory.GRAFFITI) {
                ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                componentInChildrenIncludeInactive.HullVisual(3);
            } else if (e.Category == GarageCategory.HULLS) {
                instance.ShowHulls(GarageItemsRegistry.GetItem<TankPartItem>(e.SelectedItem));
            } else if (e.Category == GarageCategory.WEAPONS) {
                instance.ShowTurrets(GarageItemsRegistry.GetItem<TankPartItem>(e.SelectedItem));
            } else if (e.Category == GarageCategory.MODULES) {
                componentInChildrenIncludeInactive.HullModules();
            } else if (e.Category == GarageCategory.SHELLS) {
                ScheduleEvent<ListItemSelectedEvent>(e.ParentItem);
                ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);
                componentInChildrenIncludeInactive.TurretVisualNoSwitch(4);
            } else if (e.Category == GarageCategory.PAINTS) {
                ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);

                if (e.SelectedItem.HasComponent<TankPaintItemComponent>()) {
                    componentInChildrenIncludeInactive.HullVisualNoSwitch(1);
                } else {
                    componentInChildrenIncludeInactive.TurretVisualNoSwitch(2);
                }
            } else if (e.Category == GarageCategory.SKINS) {
                ScheduleEvent<ListItemSelectedEvent>(e.ParentItem);
                ScheduleEvent<ListItemSelectedEvent>(e.SelectedItem);

                if (e.ParentItem.HasComponent<TankItemComponent>()) {
                    componentInChildrenIncludeInactive.HullVisualNoSwitch(0);
                } else {
                    componentInChildrenIncludeInactive.TurretVisualNoSwitch(0);
                }
            }
        }

        IEnumerable<ItemNode> FilterByParentItem(IEnumerable<ItemNode> itemsByCategory, GarageCategory garageCategory, Entity parentItem) {
            if (garageCategory == GarageCategory.MODULES) {
                return Enumerable.Empty<ItemNode>();
            }

            return itemsByCategory.Where(item => IsParentAndChild(parentItem, item.Entity));
        }

        public GarageCategory ParseCategory(string str) {
            for (int i = 0; i < GarageCategory.Values.Length; i++) {
                if (GarageCategory.Values[i].LinkPart.Equals(str)) {
                    return GarageCategory.Values[i];
                }
            }

            return null;
        }

        GarageCategory GetCategoryByItem(Entity item) {
            for (int i = 0; i < GarageCategory.Values.Length; i++) {
                if (item.HasComponent(GarageCategory.Values[i].ItemMarkerComponentType)) {
                    return GarageCategory.Values[i];
                }
            }

            throw new Exception("Category by item not found: " + item);
        }

        bool IsParentAndChild(Entity parent, Entity item) {
            long key = item.GetComponent<ParentGroupComponent>().Key;
            return key == parent.Id;
        }

        Entity GetParentItem(Entity item, GarageCategory category) {
            if (!category.NeedParent) {
                return null;
            }

            long key = item.GetComponent<ParentGroupComponent>().Key;
            return EngineService.EntityRegistry.GetEntity(key);
        }

        Entity FindMountedWeapon(ICollection<ItemNode> items) {
            ItemNode itemNode = items.FirstOrDefault(item => item.Entity.HasComponent<MountedItemComponent>() && item.Entity.HasComponent<WeaponItemComponent>());

            if (itemNode == null) {
                throw new Exception("Mounted weapon not found");
            }

            return itemNode.Entity;
        }

        Entity FindMountedTank(ICollection<ItemNode> items) {
            ItemNode itemNode = items.FirstOrDefault(item => item.Entity.HasComponent<MountedItemComponent>() && item.Entity.HasComponent<TankItemComponent>());

            if (itemNode == null) {
                throw new Exception("Mounted tank not found");
            }

            return itemNode.Entity;
        }

        public class ItemNode : Node {
            public GarageItemComponent garageItem;
        }
    }
}