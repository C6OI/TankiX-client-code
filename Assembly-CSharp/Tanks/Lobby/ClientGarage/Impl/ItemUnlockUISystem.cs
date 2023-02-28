using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemUnlockUISystem : ECSSystem {
        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SelfUserNode user, [Context] TurretItemUnlockUINode node) {
            InitializeSkinUnlocks(node.itemUnlockUI, node.selectedTurretUI, node.Entity);
            SetExperienceProgress(node.selectedTurretUI, node.itemUnlockUI);
        }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SelfUserNode user, [Context] HullItemUnlockUINode node) {
            InitializeSkinUnlocks(node.itemUnlockUI, node.selectedHullUI, node.Entity);
            SetExperienceProgress(node.selectedHullUI, node.itemUnlockUI);
        }

        [OnEventComplete]
        public void UpdateTurret(NodeAddedEvent e, MountedTurretNode turret, [JoinByUser] SelfUserNode self, [JoinAll] TurretItemUnlockUINode uiNode) {
            CleanSkinUnlocks(uiNode.itemUnlockUI);
            InitializeSkinUnlocks(uiNode.itemUnlockUI, uiNode.selectedTurretUI, uiNode.Entity);
            SetExperienceProgress(uiNode.selectedTurretUI, uiNode.itemUnlockUI);
        }

        [OnEventComplete]
        public void UpdateHull(NodeAddedEvent e, MountedHullNode turret, [JoinByUser] SelfUserNode self, [JoinAll] HullItemUnlockUINode uiNode) {
            CleanSkinUnlocks(uiNode.itemUnlockUI);
            InitializeSkinUnlocks(uiNode.itemUnlockUI, uiNode.selectedHullUI, uiNode.Entity);
            SetExperienceProgress(uiNode.selectedHullUI, uiNode.itemUnlockUI);
        }

        void InitializeSkinUnlocks(ItemUnlockUIComponent itemUnlockUI, SelectedItemUI selectedItemUI, Entity entity) {
            TankPartItem tankPartItem = selectedItemUI.TankPartItem;
            List<VisualItem> list = new();

            foreach (VisualItem skin in tankPartItem.Skins) {
                if (skin.RestrictionLevel > 0) {
                    list.Add(skin);
                }
            }

            GetAllGraffitiesEvent getAllGraffitiesEvent = new();
            ScheduleEvent(getAllGraffitiesEvent, entity);
            List<VisualItem> items = getAllGraffitiesEvent.Items;

            foreach (VisualItem item in items) {
                if (item.ParentItem == tankPartItem && item.RestrictionLevel > 0) {
                    list.Add(item);
                }
            }

            list.Sort((VisualItem item1, VisualItem item2) => item1.RestrictionLevel.CompareTo(item2.RestrictionLevel));

            foreach (VisualItem item2 in list) {
                GameObject rewardContainer = itemUnlockUI.rewardContainer;
                GameObject gameObject = Object.Instantiate(itemUnlockUI.rewardPrefab);
                gameObject.transform.SetParent(rewardContainer.transform, false);
                RewardElement component = gameObject.GetComponent<RewardElement>();
                component.titleText.text = MarketItemNameLocalization.Instance.GetCategoryName(item2.MarketItem) + " " + MarketItemNameLocalization.Instance.GetGarageItemName(item2);

                if (item2.IsRestricted) {
                    component.descriptionText.text = (string)itemUnlockUI.levelText + " " + item2.RestrictionLevel;
                } else {
                    component.descriptionText.text = itemUnlockUI.recievedText;
                }

                ImageItemComponent component2 = item2.MarketItem.GetComponent<ImageItemComponent>();
                component.imageSkin.SpriteUid = component2.SpriteUid;
                itemUnlockUI.rewardPreviews.Add(gameObject);
            }
        }

        [OnEventFire]
        public void CleanSkinUnlocks(NodeAddedEvent e, TurretItemUnlockUINode node) {
            CleanSkinUnlocks(node.itemUnlockUI);
        }

        [OnEventFire]
        public void CleanSkinUnlocks(NodeAddedEvent e, HullItemUnlockUINode node) {
            CleanSkinUnlocks(node.itemUnlockUI);
        }

        void CleanSkinUnlocks(ItemUnlockUIComponent itemUnlockUI) {
            foreach (GameObject rewardPreview in itemUnlockUI.rewardPreviews) {
                Object.Destroy(rewardPreview);
            }
        }

        void SetExperienceProgress(SelectedItemUI selectedItemUI, ItemUnlockUIComponent itemUnlockUI) {
            if (selectedItemUI.TankPartItem.UserItem != null && selectedItemUI.TankPartItem.UserItem.HasComponent<UpgradeLevelItemComponent>()) {
                itemUnlockUI.gameObject.SetActive(true);

                if (selectedItemUI.TankPartItem.UpgradeLevel == UpgradablePropertiesUtils.MAX_LEVEL) {
                    itemUnlockUI.experienceProgressBar.InitialNormalizedValue = 0f;
                    itemUnlockUI.experienceProgressBar.NormalizedValue = 1f;
                    itemUnlockUI.text.text = itemUnlockUI.maxText;
                    return;
                }

                ExperienceToLevelUpItemComponent experience = selectedItemUI.TankPartItem.Experience;
                float num = selectedItemUI.TankPartItem.AbsExperience;
                float num2 = experience.FinalLevelExperience;
                float normalizedValue = num / num2;
                itemUnlockUI.experienceProgressBar.NormalizedValue = normalizedValue;
                itemUnlockUI.text.text = num + "/" + num2;
            }
        }

        public class TurretItemUnlockUINode : Node {
            public ItemUnlockUIComponent itemUnlockUI;

            public SelectedTurretUIComponent selectedTurretUI;
        }

        public class HullItemUnlockUINode : Node {
            public ItemUnlockUIComponent itemUnlockUI;

            public SelectedHullUIComponent selectedHullUI;
        }

        public class GarageUserItemNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ExperienceItemComponent experienceItem;
            public MarketItemGroupComponent marketItemGroup;

            public UpgradeLevelItemComponent upgradeLevelItem;

            public UserGroupComponent userGroup;

            public VisualPropertiesComponent visualProperties;
        }

        public class MountedTurretNode : GarageUserItemNode {
            public MountedItemComponent mountedItem;

            public WeaponItemComponent weaponItem;
        }

        public class MountedHullNode : GarageUserItemNode {
            public MountedItemComponent mountedItem;

            public TankItemComponent tankItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }
    }
}