using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesBadgesSystem : ECSSystem {
        [OnEventFire]
        public void NotificationBadgeInit(NodeAddedEvent e, [Combine] SingleNode<ModulesNotificationBadgeComponent> modulesNotificationBadge, SelfUserNode selfUser,
            SelectedPresetNode selectedPreset, [JoinAll] ICollection<MarketModuleNode> marketModules) {
            if (selectedPreset.userGroup.Key != selfUser.userGroup.Key) {
                modulesNotificationBadge.component.CurrentState = State.None;
                return;
            }

            modulesNotificationBadge.component.CurrentState = State.None;

            foreach (MarketModuleNode marketModule in marketModules) {
                if (modulesNotificationBadge.component.TankPart != marketModule.moduleTankPart.TankPart) {
                    continue;
                }

                long num = 0L;
                IList<ModuleCardNode> list = Select<ModuleCardNode>(marketModule.Entity, typeof(ParentGroupComponent));

                if (list.Count > 0) {
                    num = list.Single().userItemCounter.Count;
                }

                if (num == 0) {
                    continue;
                }

                List<UserModuleNode> list2 = (from m in Select<UserModuleNode>(marketModule.Entity, typeof(ParentGroupComponent))
                                              where m.userGroup.Key == selfUser.userGroup.Key
                                              select m).ToList();

                if (list2.Count > 0) {
                    if (modulesNotificationBadge.component.CurrentState == State.ModuleUpgradeAvailable) {
                        continue;
                    }

                    long level = list2.Single().moduleUpgradeLevel.Level;

                    if (level < marketModule.moduleCardsComposition.UpgradePrices.Count) {
                        int cards = marketModule.moduleCardsComposition.UpgradePrices[(int)level].Cards;

                        if (num >= cards && !ModuleLevelWasWatched(marketModule.marketItemGroup.Key, level)) {
                            modulesNotificationBadge.component.CurrentState = State.ModuleUpgradeAvailable;
                        }
                    }
                } else {
                    int cards2 = marketModule.moduleCardsComposition.CraftPrice.Cards;

                    if (num >= cards2 && !ModuleLevelWasWatched(marketModule.marketItemGroup.Key, -1L)) {
                        modulesNotificationBadge.component.CurrentState = State.NewModuleAvailable;
                        break;
                    }
                }
            }
        }

        [OnEventFire]
        public void ModuleUIAdded(NodeAddedEvent e, SelectedModuleUI module) {
            long currentLevel = !module.Entity.HasComponent<ModuleUpgradeLevelComponent>() ? -1 : module.Entity.GetComponent<ModuleUpgradeLevelComponent>().Level;
            ModuleWasWatched(module.marketItemGroup.Key, currentLevel);
        }

        bool ModuleLevelWasWatched(long moduleId, long currentLevel) {
            string playerPrefsKey = GetPlayerPrefsKey(moduleId);

            if (PlayerPrefs.HasKey(playerPrefsKey)) {
                int @int = PlayerPrefs.GetInt(playerPrefsKey);
                return @int >= currentLevel;
            }

            return false;
        }

        void ModuleWasWatched(long moduleId, long currentLevel) {
            string playerPrefsKey = GetPlayerPrefsKey(moduleId);
            PlayerPrefs.SetInt(playerPrefsKey, (int)currentLevel);
        }

        string GetPlayerPrefsKey(long moduleId) => "moduleLastWatchedLevel-" + moduleId;

        public class ModuleNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ItemBigIconComponent itemBigIcon;

            public ItemIconComponent itemIcon;

            public MarketItemGroupComponent marketItemGroup;

            public ModuleBehaviourTypeComponent moduleBehaviourType;

            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleItemComponent moduleItem;

            public ModuleTankPartComponent moduleTankPart;

            public ModuleTierComponent moduleTier;

            public OrderItemComponent orderItem;
        }

        public class UserModuleNode : ModuleNode {
            public ModuleGroupComponent moduleGroup;

            public ModuleUpgradeLevelComponent moduleUpgradeLevel;

            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
        }

        public class MarketModuleNode : ModuleNode {
            public MarketItemComponent marketItem;
        }

        public class ModuleCardNode : Node {
            public ModuleCardItemComponent moduleCardItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class SelectedModuleUI : ModuleNode {
            public ModuleCardItemUIComponent moduleCardItemUI;

            public ToggleListSelectedItemComponent toggleListSelectedItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelectedPresetNode : Node {
            public SelectedPresetComponent selectedPreset;

            public UserGroupComponent userGroup;
        }
    }
}