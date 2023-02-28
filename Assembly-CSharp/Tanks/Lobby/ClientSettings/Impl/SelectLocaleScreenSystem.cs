using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientLocale.API;
using Platform.Library.ClientLocale.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.Impl {
    public class SelectLocaleScreenSystem : ECSSystem {
        [OnEventFire]
        public void CreateLocaleEntities(NodeAddedEvent e, LocaleListNode node, [JoinAll] UserNode user) {
            foreach (string locale in node.localeList.Locales) {
                string value = locale.Split('/')[1];

                if (!"tr".Equals(value) || user.userPublisher.Publisher == Publisher.CONSALA) {
                    Entity entity = CreateEntity<LocaleTemplate>(locale);
                    Debug.Log("Language accepted!");
                    node.simpleHorizontalList.AddItem(entity);
                    entity.AddComponent(new ScreenGroupComponent(node.screenGroup.Key));
                }
            }
        }

        [OnEventFire]
        public void DestroyLocaleEntities(NodeRemoveEvent e, SingleNode<SelectLocaleScreenComponent> screen, [JoinAll] ICollection<SingleNode<LocaleComponent>> locales) {
            foreach (SingleNode<LocaleComponent> locale in locales) {
                DeleteEntity(locale.Entity);
            }

            screen.component.DisableButtons();
        }

        [OnEventFire]
        public void ClearLocaleList(NodeRemoveEvent e, LocaleListNode list) {
            list.simpleHorizontalList.ClearItems();
        }

        [OnEventFire]
        public void InitSelectedLocaleItem(NodeAddedEvent e, LocaleItemNode node, [Context] [JoinByScreen] SelectedLocaleNode selected,
            [Context] [JoinByScreen] LocaleListNode localesList) {
            LocaleComponent locale = node.locale;
            LocaleItemComponent localeItem = node.localeItem;
            localeItem.SetText(locale.Caption, locale.LocalizedCaption);
            string savedLocaleCode = LocaleUtils.GetSavedLocaleCode();

            if (locale.Code == savedLocaleCode) {
                SetLocaleText(selected, node.locale);
                localesList.simpleHorizontalList.Select(node.Entity);
            }
        }

        [OnEventFire]
        public void InitLocaleItem(ListItemSelectedEvent e, LocaleItemNode node, [JoinByScreen] SelectedLocaleNode selected, LocaleItemNode nodeA,
            [JoinByScreen] SingleNode<SelectLocaleScreenComponent> screen) {
            selected.selectedLocale.Code = node.locale.Code;

            if (node.locale.Code == LocaleUtils.GetSavedLocaleCode()) {
                screen.component.DisableButtons();
            } else {
                screen.component.EnableButtons();
            }
        }

        void SetLocaleText(SelectedLocaleNode destination, LocaleComponent source) {
            destination.selectedLocale.Code = source.Code;
        }

        [OnEventFire]
        public void Apply(ButtonClickEvent e, SingleNode<ApplyButtonComponent> button, [JoinByScreen] SingleNode<SelectLocaleScreenComponent> screen,
            SingleNode<ApplyButtonComponent> buttonA, [JoinByScreen] SelectedLocaleNode selected) {
            LocaleUtils.SaveLocaleCode(selected.selectedLocale.Code);
            ScheduleEvent<SwitchToEntranceSceneEvent>(button);
        }

        public class LocaleListNode : Node {
            public LocaleListComponent localeList;

            public ScreenGroupComponent screenGroup;

            public SimpleHorizontalListComponent simpleHorizontalList;
        }

        public class LocaleItemNode : Node {
            public LocaleComponent locale;

            public LocaleItemComponent localeItem;

            public ScreenGroupComponent screenGroup;
        }

        public class SelectedLocaleNode : Node {
            public ScreenGroupComponent screenGroup;
            public SelectedLocaleComponent selectedLocale;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserPublisherComponent userPublisher;
        }
    }
}