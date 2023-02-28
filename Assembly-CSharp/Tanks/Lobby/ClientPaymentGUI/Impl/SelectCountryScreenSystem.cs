using System;
using System.Collections.Generic;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientProfile.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SelectCountryScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitChangeCountryButton(NodeAddedEvent e, SingleNode<OpenSelectCountryButtonComponent> button, UserWithCountryNode country) {
            if (country.userCountry.CountryCode == "RU" || country.userPublisher.Publisher == Publisher.CONSALA) {
                NewEvent<DisableCountryButtonEvent>().Attach(button).ScheduleDelayed(0f);
            }

            button.component.CountryCode = country.userCountry.CountryCode;
        }

        [OnEventFire]
        public void DisableCountryButton(DisableCountryButtonEvent e, SingleNode<OpenSelectCountryButtonComponent> button) {
            button.component.gameObject.SetActive(false);
        }

        [OnEventFire]
        public void GoToPayment(ButtonClickEvent e, SingleNode<UserXCrystalsIndicatorComponent> button, [JoinAll] SingleNode<SelfUserComponent> user) {
            ScheduleEvent<GoToXCryShopScreen>(user);
        }

        [OnEventFire]
        public void ParsePaymentLink(ParseLinkEvent e, Node node, [JoinAll] SingleNode<SelfUserComponent> user) {
            if (e.Link.StartsWith("payment")) {
                e.CustomNavigationEvent = NewEvent<GoToXCryShopScreen>().Attach(user);
            }
        }

        [OnEventFire]
        public void LogEnterPayment(GoToPaymentRequestEvent e, SingleNode<SelfUserComponent> user, [JoinByUser] SingleNode<ClientSessionComponent> session,
            [JoinAll] ActiveScreenNode activeScreenNode) {
            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.OPEN_PAYMENT,
                Screen = activeScreenNode.screen.gameObject.name
            }, session);
        }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] SingleNode<CountriesComponent> countries, [JoinAll] UserWithCountryNode country) {
            List<KeyValuePair<string, string>> list = new();

            foreach (KeyValuePair<string, string> name in countries.component.Names) {
                if (!(name.Key == "TR")) {
                    list.Add(name);
                }
            }

            list.Sort((KeyValuePair<string, string> a, KeyValuePair<string, string> b) => string.Compare(a.Value, b.Value, StringComparison.Ordinal));
            screen.selectCountryDialog.Init(list);
            screen.selectCountryDialog.Select(country.userCountry.CountryCode);
        }

        [OnEventFire]
        public void ChangeCountry(DialogConfirmEvent e, SingleNode<SelectCountryDialogComponent> dialog) {
            if (!string.IsNullOrEmpty(dialog.component.country.Value)) {
                SelectCountryEvent selectCountryEvent = new();
                selectCountryEvent.CountryCode = dialog.component.country.Key;
                selectCountryEvent.CountryName = dialog.component.country.Value;
                SelectCountryEvent eventInstance = selectCountryEvent;
                ScheduleEvent(eventInstance, dialog.Entity);
                dialog.component.Hide();
            }
        }

        [OnEventFire]
        public void Continue(SelectCountryEvent e, Node stub, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new ConfirmUserCountryEvent {
                CountryCode = e.CountryCode
            }, selfUser);

            ScheduleEvent(new PaymentStatisticsEvent {
                Action = PaymentStatisticsAction.COUNTRY_SELECT,
                Screen = "SelectCountryScreen"
            }, session);
        }

        [OnEventFire]
        public void ChangeCountry(ConfirmUserCountryEvent e, SingleNode<UserCountryComponent> country, [JoinAll] Optional<SingleNode<OpenSelectCountryButtonComponent>> button) {
            country.component.CountryCode = e.CountryCode;

            if (button.IsPresent()) {
                button.Get().component.CountryCode = e.CountryCode;
            }
        }

        public class ScreenNode : Node {
            public SelectCountryDialogComponent selectCountryDialog;
        }

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ScreenComponent screen;
        }

        public class UserWithCountryNode : Node {
            public SelfUserComponent selfUser;

            public UserCountryComponent userCountry;

            public UserPublisherComponent userPublisher;
        }

        [Not(typeof(UserCountryComponent))]
        public class UserWithoutCountryNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}