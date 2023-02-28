using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientBattleSelect.Impl.ModuleContainer {
    public class ModuleContainerRewardSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void ShowModuleContainerReward(ShowRewardEvent e, SingleNode<BattleResultsAwardsScreenComponent> screen,
            SingleNode<ModuleContainerPersonalBattleRewardComponent> personalReward, [JoinBy(typeof(BattleRewardGroupComponent))] ModuleContainerRewardNote battleReward,
            [JoinAll] SingleNode<BattleResultsComponent> battleResults) {
            string name = battleReward.descriptionItem.Name;
            string empty = string.Empty;

            empty = battleResults.component.ResultForClient.PersonalResult.TeamBattleResult != 0 ? battleReward.moduleContainerRewardTextConfig.LooseText
                        : battleReward.moduleContainerRewardTextConfig.WinText;

            long сontainerId = personalReward.component.СontainerId;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(сontainerId);
            string spriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
            string name2 = entity.GetComponent<DescriptionItemComponent>().Name;
            List<SpecialOfferItem> list = new();
            list.Add(new SpecialOfferItem(0, spriteUid, name2));
            BattleResultSpecialOfferUiComponent specialOfferUI = screen.component.specialOfferUI;
            specialOfferUI.ShowContent(name, empty, list);
            int price = entity.GetComponent<XPriceItemComponent>().Price;
            specialOfferUI.SetCrystalButton(0, price, 0, true);
            specialOfferUI.Appear();
        }

        [OnEventFire]
        public void BuyContainer(ButtonClickEvent e, SingleNode<SpecialOfferCrystalButtonComponent> button, [JoinAll] SingleNode<BattleResultsComponent> battleResults,
            [JoinAll] SingleNode<BattleResultsAwardsScreenComponent> screen) {
            Entity reward = battleResults.component.ResultForClient.PersonalResult.Reward;
            long itemId = reward.GetComponent<ModuleContainerPersonalBattleRewardComponent>().СontainerId;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(itemId);
            int regularPrice = entity.GetComponent<XPriceItemComponent>().Price;
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(itemId);

            Object.FindObjectOfType<Dialogs60Component>().Get<BuyConfirmationDialog>().XShow(item, delegate {
                BattleResultSpecialOfferUiComponent ui = screen.component.specialOfferUI;

                Action onOpen = delegate {
                    ui.SetCrystalButton(0, regularPrice, 0, true);
                };

                ui.SetOpenButton(itemId, 1, onOpen);
            }, regularPrice, 1, item.Name, true);
        }

        public class ModuleContainerRewardNote : Node {
            public DescriptionItemComponent descriptionItem;
            public ModuleContainerRewardTextConfigComponent moduleContainerRewardTextConfig;
        }
    }
}