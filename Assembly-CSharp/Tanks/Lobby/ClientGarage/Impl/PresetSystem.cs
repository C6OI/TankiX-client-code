using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PresetSystem : ECSSystem {
        public static readonly PresetComparer comparer = new();

        static readonly PresetNodeComparer nodeComparer = new();

        public PresetSystem(SystemRegistry systemRegistry) {
            systemRegistry.RegisterSingleNode<CreateByRankConfigComponent>();
            systemRegistry.RegisterSingleNode<ItemsBuyCountLimitComponent>();
            systemRegistry.RegisterSingleNode<FirstBuySaleComponent>();
            systemRegistry.RegisterSingleNode<CreatedByRankItemComponent>();
        }

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void MountPreset(MountPresetEvent e, BasePresetNode presetNode, [JoinAll] SelfUserNode selfUser, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset,
            [JoinAll] ICollection<MountedPresetNode> mountedPresets) {
            Entity entity = presetNode.Entity;

            if (!entity.HasComponent<UserItemComponent>()) {
                MountedPresetNode mountedPresetNode = mountedPresets.First(it => it.userGroup.Key == selfUser.userGroup.Key);
                ChangeSelectedPreset(selectedPreset, mountedPresetNode.Entity);
            }

            if (entity.HasComponent<UserItemComponent>() && entity.GetComponent<UserGroupComponent>().Key == selfUser.userGroup.Key) {
                if (!entity.HasComponent<MountedItemComponent>()) {
                    ScheduleEvent<MountItemEvent>(entity);
                }

                ChangeSelectedPreset(selectedPreset, entity);
            }

            if (entity.HasComponent<UserGroupComponent>() && entity.GetComponent<UserGroupComponent>().Key != selfUser.userGroup.Key) {
                NewEvent(new MountRentedPresetEvent()).Attach(entity).Attach(selfUser).Schedule();
                ChangeSelectedPreset(selectedPreset, entity);
            } else {
                ScheduleEvent(new RemoveRentedPresetEvent(), selfUser);
            }
        }

        [OnEventFire]
        public void ScrollList(MountPresetEvent e, BasePresetNode preset, [JoinAll] PresetListNode presetList, [JoinAll] ICollection<ListItemNode> listItems) {
            List<Entity> listPresets = GetListPresets(presetList);
            ListItemNode listItemNode = listItems.First(it => it.presetListItem.Preset.Equals(preset));
            bool flag = false;
            bool flag2 = false;

            if (!listItemNode.presetListItem.IsUserItem) {
                flag = listItemNode.presetListItem.Rank > 0;
                flag2 = GarageItemsRegistry.GetItem<GarageItem>(preset.Entity).IsBuyable && !flag;
            }

            presetList.presetList.SetBuyButtonEnabled(flag2);
            presetList.presetList.SetLockedByRankTextEnabled(flag);

            if (flag2) {
                GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(preset.Entity);
                presetList.presetList.XBuyPrice.SetPrice(item.OldXPrice, item.XPrice);
            }

            if (flag) {
                presetList.presetList.LockedByRankText = string.Format(presetList.presetList.LockedByRankLocalizedText, listItemNode.presetListItem.Rank);
            }
        }

        [OnEventFire]
        public void ProvidePresetName(GetPresetNameEvent e, UserPresetNode preset, [JoinAll] SelfUserNode selfUser) {
            if (preset.userGroup.Key == selfUser.userGroup.Key) {
                string name = preset.presetName.Name;
                e.Name = name;
                return;
            }

            IList<OfferNode> source = Select<OfferNode, SpecialOfferGroupComponent>(preset.Entity);

            if (source.Any()) {
                e.Name = source.First().legendaryTankSpecialOffer.TankRole.ToString();
            } else {
                e.Name = preset.presetName.Name;
            }
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeAddedEvent e, MountedPresetNode presetNode, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset) {
            if (!selectedPreset.IsPresent() && !presetNode.Entity.HasComponent<SelectedPresetComponent>()) {
                presetNode.Entity.AddComponent<SelectedPresetComponent>();
                ScheduleEvent(new ResetPreviewEvent(), presetNode);
            }
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeRemoveEvent e, SelfUserWithPrototypeNode user, [JoinByUser] MountedPresetNode presetNode,
            [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset) {
            if (!selectedPreset.IsPresent() && !presetNode.Entity.HasComponent<SelectedPresetComponent>()) {
                presetNode.Entity.AddComponent<SelectedPresetComponent>();
                ScheduleEvent(new ResetPreviewEvent(), presetNode);
            }
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeAddedEvent e, SelectedPresetNode preset, SelfUserNode selfUser, SingleNode<ElementsToChangeWhenRentedTankComponent> helper,
            SingleNode<PresetListComponent> presetsScreen) {
            if (preset.userGroup.Key != selfUser.userGroup.Key) {
                helper.component.SetScreenToRentedTankState();
            } else {
                helper.component.ReturnScreenToNormalState();
            }
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeAddedEvent e, NotOwnedSelectedPreset preset, SelfUserNode selfUser, SingleNode<ElementsToChangeWhenRentedTankComponent> helper,
            SingleNode<PresetListComponent> presetsScreen) {
            helper.component.ReturnScreenToNormalState();
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeRemoveEvent e, SelectedPresetNode preset, [JoinAll] SelfUserNode selfUser,
            [JoinAll] SingleNode<ElementsToChangeWhenRentedTankComponent> helper, [JoinAll] SingleNode<PresetListComponent> presetsScreen) {
            helper.component.ReturnScreenToNormalState();
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeAddedEvent e, SelfUserWithPrototypeNode userWithRentedPreset, [Context] [Combine] MountedPresetNode presetNode,
            [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset) {
            if (presetNode.Entity.Equals(userWithRentedPreset.userUseItemsPrototype.Preset)) {
                if (selectedPreset.IsPresent()) {
                    selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
                }

                if (!presetNode.Entity.HasComponent<SelectedPresetComponent>()) {
                    presetNode.Entity.AddComponent<SelectedPresetComponent>();
                    ScheduleEvent(new ResetPreviewEvent(), presetNode);
                }
            }
        }

        [OnEventFire]
        public void InitName(NodeAddedEvent e, SingleNode<PresetNameEditorComponent> presetNameEditor, SelfUserNode selfUser, SelectedPresetNode preset) {
            string presetName = GetPresetName(preset.Entity);
            presetNameEditor.component.PresetName = presetName;
        }

        [OnEventFire]
        public void InitName(NodeAddedEvent e, SingleNode<PresetLabelComponent> presetNameLabel, SelectedPresetNode preset, SelfUserNode selfUser) {
            string presetName = GetPresetName(preset.Entity);
            presetNameLabel.component.PresetName = presetName;
        }

        [OnEventFire]
        public void SaveName(PresetNameChangedEvent e, SingleNode<PresetNameEditorComponent> presetNameEditor, [JoinAll] SelfUserNode selfUser,
            [JoinByUser] [Mandatory] MountedPresetNode preset, [JoinAll] PresetListNode presetList) {
            string presetName = presetNameEditor.component.PresetName;
            preset.presetName.Name = presetName;
            preset.presetName.OnChange();
            GetListItems(presetList).Find(item => item.Preset == preset.Entity).PresetName = presetName;
        }

        [OnEventFire]
        public void InitList(NodeAddedEvent e, PresetListNode presetList, [JoinAll] ICollection<BasePresetNode> presets, [JoinAll] SelfUserNode selfUser,
            [JoinAll] MarketPresetNode marketPreset) {
            List<BasePresetNode> list = new(presets);
            list.Sort(nodeComparer);
            int elementIndex = 0;

            for (int i = 0; i < list.Count; i++) {
                BasePresetNode basePresetNode = list[i];

                if (basePresetNode.Entity.HasComponent<CreateByRankConfigComponent>()) {
                    int[] userRankListToCreateItem = basePresetNode.Entity.GetComponent<CreateByRankConfigComponent>().UserRankListToCreateItem;

                    foreach (int num in userRankListToCreateItem) {
                        int rank = selfUser.userRank.Rank;

                        if (rank < num) {
                            PresetListItemComponent presetListItemComponent = AddListItem(presetList, basePresetNode, selfUser);
                            presetListItemComponent.Rank = num;
                        }
                    }
                }

                if (!basePresetNode.Entity.HasComponent<ItemsBuyCountLimitComponent>() || !IsMaxCount(presetList, marketPreset)) {
                    AddListItem(presetList, basePresetNode, selfUser);

                    if (basePresetNode.Entity.HasComponent<MountedItemComponent>() && basePresetNode.Entity.HasComponent<SelectedPresetComponent>()) {
                        elementIndex = i;
                    }
                }
            }

            UpdateMarketItemsNames(presetList);
            presetList.presetList.ScrollToElement(elementIndex, true, false);
        }

        void UpdateMarketItemsNames(PresetListNode presetList) {
            List<PresetListItemComponent> listItems = GetListItems(presetList);

            for (int i = 0; i < listItems.Count; i++) {
                PresetListItemComponent presetListItemComponent = listItems[i];

                if (!presetListItemComponent.IsUserItem) {
                    presetListItemComponent.PresetName = presetListItemComponent.Preset.GetComponent<DescriptionItemComponent>().Name + " " + (i + 1);
                }
            }
        }

        [OnEventFire]
        public void ClearList(NodeRemoveEvent e, PresetListNode presetList) {
            presetList.presetList.Clear();
            presetList.presetList.SetBuyButtonEnabled(false);
        }

        [OnEventFire]
        public void AddListItemOnNewPreset(NodeAddedEvent e, UserPresetNode preset, [JoinAll] PresetListNode presetList, [JoinAll] MarketPresetNode marketPreset,
            [JoinAll] SelfUserNode user) {
            List<Entity> listPresets = GetListPresets(presetList);
            int num = listPresets.BinarySearch(preset.Entity, comparer);

            if (num < 0) {
                int num2 = ~num;
                AddListItem(presetList, preset, user).transform.SetSiblingIndex(num2);
                bool sendSelected = false;
                presetList.presetList.ScrollToElement(num2, true, sendSelected);
                presetList.presetList.SetBuyButtonEnabled(false);
            }

            if (IsMaxCount(presetList, marketPreset)) {
                int num3 = GetListItems(presetList).FindLastIndex(i => marketPreset.Entity.Equals(i.Preset));

                if (num3 != -1) {
                    presetList.presetList.RemoveElement(num3);
                }
            }

            UpdateMarketItemsNames(presetList);
        }

        [OnEventFire]
        public void RemoveListItem(NodeRemoveEvent e, BasePresetNode preset, [JoinAll] PresetListNode presetList) {
            List<Entity> listPresets = GetListPresets(presetList);
            int num = listPresets.BinarySearch(preset.Entity, comparer);

            if (num >= 0) {
                presetList.presetList.RemoveElement(num);
            }

            UpdateMarketItemsNames(presetList);
        }

        [OnEventFire]
        public void RemoveLockedByRankItem(UpdateRankEvent e, SelfUserNode user, [JoinAll] PresetListNode presetList) {
            int rank = user.userRank.Rank;
            List<PresetListItemComponent> listItems = GetListItems(presetList);

            for (int i = 0; i < listItems.Count; i++) {
                PresetListItemComponent presetListItemComponent = listItems[i];

                if (presetListItemComponent.Preset.HasComponent<CreateByRankConfigComponent>() && rank > presetListItemComponent.Rank && presetListItemComponent.Rank > 0) {
                    presetList.presetList.RemoveElement(i);
                }
            }

            UpdateMarketItemsNames(presetList);
        }

        bool IsMaxCount(PresetListNode presetList, MarketPresetNode marketPreset) {
            int num = !marketPreset.Entity.HasComponent<ItemsBuyCountLimitComponent>() ? int.MaxValue : marketPreset.Entity.GetComponent<ItemsBuyCountLimitComponent>().Limit;
            return GetListItems(presetList).Count(item => item.IsOwned && !item.Preset.HasComponent<CreatedByRankItemComponent>()) >= num;
        }

        PresetListItemComponent AddListItem(PresetListNode presetList, BasePresetNode preset, SelfUserNode user) {
            GameObject gameObject = presetList.presetList.AddElement();
            PresetListItemComponent component = gameObject.GetComponent<PresetListItemComponent>();
            component.Preset = preset.Entity;
            component.IsUserItem = preset.Entity.HasComponent<UserItemComponent>();
            component.IsOwned = component.IsUserItem && preset.Entity.GetComponent<UserGroupComponent>().Key == user.userGroup.Key;
            component.PresetName = !component.IsUserItem ? presetList.presetList.LockedByRankLocalizedText.Value : GetPresetName(preset.Entity);
            component.Locked = !component.IsUserItem;
            return component;
        }

        [OnEventFire]
        public void MountPreset(ListItemSelectedEvent e, ListItemNode listItem) {
            Entity preset = listItem.presetListItem.Preset;

            if (preset != null) {
                ScheduleEvent(new MountPresetEvent(), preset);
            }
        }

        void ChangeSelectedPreset(Optional<SingleNode<SelectedPresetComponent>> selectedPreset, Entity preset) {
            if (selectedPreset.IsPresent()) {
                selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
            }

            preset.AddComponent<SelectedPresetComponent>();
        }

        [OnEventFire]
        public void SetBuyButton(ListItemSelectedEvent e, ListItemNode listItem, [JoinAll] PresetListNode presetList) {
            Entity preset = listItem.presetListItem.Preset;

            if (preset != null) {
                bool flag = false;
                bool flag2 = false;

                if (!listItem.presetListItem.IsUserItem) {
                    flag = listItem.presetListItem.Rank > 0;
                    flag2 = GarageItemsRegistry.GetItem<GarageItem>(preset).IsBuyable && !flag;
                }

                presetList.presetList.SetBuyButtonEnabled(flag2);
                presetList.presetList.SetLockedByRankTextEnabled(flag);

                if (flag2) {
                    GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(preset);
                    int xPrice = item.XPrice;
                    presetList.presetList.XBuyPrice.SetPrice(item.OldXPrice, xPrice);
                }

                if (flag) {
                    presetList.presetList.LockedByRankText = string.Format(presetList.presetList.LockedByRankLocalizedText, listItem.presetListItem.Rank);
                }
            }
        }

        [OnEventFire]
        public void ShowBuyDialog(ButtonClickEvent e, BuyButtonNode buyButton, [JoinAll] PresetListNode presetList) {
            int selectedItemIndex = presetList.presetList.SelectedItemIndex;
            PresetListItemComponent listItem = GetListItem(presetList, selectedItemIndex);
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(listItem.Preset);
            presetList.presetList.BuyDialog.XShow(item, delegate { }, item.XPrice);
        }

        [OnEventFire]
        public void ResetPreviewOnOfferRemove(NodeRemoveEvent e, SelfUserWithPrototypeNode userWithRentedPreset, [JoinAll] ICollection<MountedPresetNode> mountedPresets,
            [JoinAll] SelfUserNode selfUser, [JoinAll] Optional<SelectedPresetNode> selectedPreset, [JoinAll] Optional<PresetListNode> presetList) {
            if ((!selectedPreset.IsPresent() || userWithRentedPreset.userUseItemsPrototype.Preset.Equals(selectedPreset.Get().Entity)) && !presetList.IsPresent()) {
                if (selectedPreset.IsPresent()) {
                    selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
                }

                IEnumerable<MountedPresetNode> source = mountedPresets.Where(it => it.userGroup.Key == selfUser.userGroup.Key);

                if (source.Count() != 0) {
                    MountedPresetNode mountedPresetNode = source.First();
                    mountedPresetNode.Entity.AddComponent<SelectedPresetComponent>();
                    ScheduleEvent<ResetPreviewEvent>(userWithRentedPreset);
                }
            }
        }

        static PresetListItemComponent GetListItem(PresetListNode presetList, int index) => presetList.presetList.ContentRoot.GetChild(index).GetComponent<PresetListItemComponent>();

        static List<PresetListItemComponent> GetListItems(PresetListNode presetList) {
            RectTransform contentRoot = presetList.presetList.ContentRoot;
            List<PresetListItemComponent> list = new(contentRoot.childCount);

            for (int i = 0; i < contentRoot.childCount; i++) {
                Transform child = contentRoot.GetChild(i);
                PresetListItemComponent component = child.GetComponent<PresetListItemComponent>();
                list.Add(component);
            }

            return list;
        }

        static List<Entity> GetListPresets(PresetListNode presetList) {
            return GetListItems(presetList).ConvertAll(item => item.Preset);
        }

        string GetPresetName(Entity preset) {
            GetPresetNameEvent getPresetNameEvent = new();
            ScheduleEvent(getPresetNameEvent, preset);
            return getPresetNameEvent.Name;
        }

        public class BasePresetNode : Node {
            public PresetItemComponent presetItem;
        }

        public class MarketPresetNode : BasePresetNode {
            public MarketItemComponent marketItem;
        }

        public class UserPresetNode : BasePresetNode {
            public PresetNameComponent presetName;

            public UserGroupComponent userGroup;
        }

        public class MountedPresetNode : UserPresetNode {
            public MountedItemComponent mountedItem;
        }

        public class PresetListNode : Node {
            public PresetListComponent presetList;
        }

        public class ListItemNode : Node {
            public PresetListItemComponent presetListItem;
        }

        public class BuyButtonNode : Node {
            public PresetBuyButtonComponent presetBuyButton;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserRankComponent userRank;
        }

        [Not(typeof(UserGroupComponent))]
        public class NotOwnedSelectedPreset : SelectedPresetNode { }

        public class SelfUserWithPrototypeNode : SelfUserNode {
            public UserUseItemsPrototypeComponent userUseItemsPrototype;
        }

        public class SelectedPresetNode : MountedPresetNode {
            public SelectedPresetComponent selectedPreset;
        }

        public class PresetComparer : IComparer<Entity> {
            public int Compare(Entity p1, Entity p2) {
                int num = p1.HasComponent<UserItemComponent>() ? 1 : 0;
                int num2 = p2.HasComponent<UserItemComponent>() ? 1 : 0;

                if (num != num2) {
                    return num2 - num;
                }

                long id = SelfUserComponent.SelfUser.Id;

                if (p1.HasComponent<UserItemComponent>()) {
                    long key = p1.GetComponent<UserGroupComponent>().Key;
                    long key2 = p2.GetComponent<UserGroupComponent>().Key;
                    int num3 = key == id ? 1 : 0;
                    int num4 = key2 == id ? 1 : 0;

                    if (num3 != num4) {
                        return num3 - num4;
                    }
                }

                return p1.Id.CompareTo(p2.Id);
            }
        }

        class PresetNodeComparer : IComparer<BasePresetNode> {
            public int Compare(BasePresetNode p1, BasePresetNode p2) => comparer.Compare(p1.Entity, p2.Entity);
        }

        public class OfferNode : Node {
            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
        }
    }
}