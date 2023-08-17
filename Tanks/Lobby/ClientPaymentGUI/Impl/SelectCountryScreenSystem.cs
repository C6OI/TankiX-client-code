using System;
using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientPayment.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientPayment.Impl;
using Tanks.Lobby.ClientProfile.Impl;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class SelectCountryScreenSystem : ECSSystem {
        [OnEventFire]
        public void InitChangeCountryButton(NodeAddedEvent e, SingleNode<ChangeCountryButtonComponent> button,
            UserWithCountryNode country) {
            if (country.userCountry.CountryCode == "RU") {
                NewEvent<DisableCountryButtonEvent>().Attach(button).ScheduleDelayed(0f);
            }

            button.component.CountryCode = country.userCountry.CountryCode;
        }

        [OnEventFire]
        public void DisableCountryButton(DisableCountryButtonEvent e, SingleNode<ChangeCountryButtonComponent> button) =>
            button.component.transform.parent.gameObject.SetActive(false);

        [OnEventFire]
        public void GoToPayment(ButtonClickEvent e, SingleNode<UserXCrystalsIndicatorComponent> button,
            [JoinAll] SingleNode<SelfUserComponent> user) => ScheduleEvent<GoToPaymentRequestEvent>(user);

        [OnEventFire]
        public void LogEnterPayment(GoToPaymentRequestEvent e, SingleNode<SelfUserComponent> user,
            [JoinByUser] SingleNode<ClientSessionComponent> session, [JoinAll] ActiveScreenNode activeScreenNode) =>
            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.OPEN_PAYMENT,
                    Screen = activeScreenNode.screen.gameObject.name
                },
                session);

        [OnEventFire]
        public void SelectScreen(GoToPaymentEvent e, UserWithCountryNode user) =>
            ScheduleEvent(new ShowScreenLeftEvent<GoodsSelectionScreenComponent>(), user);

        [OnEventFire]
        public void SelectScreen(GoToPaymentEvent e, UserWithoutCountryNode user) =>
            ScheduleEvent(new ShowScreenLeftEvent<SelectCountryScreenComponent>(), user);

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] SingleNode<CountriesComponent> countries) {
            List<KeyValuePair<string, string>> list = new();

            foreach (KeyValuePair<string, string> name in countries.component.Names) {
                list.Add(name);
            }

            list.Sort((a, b) => string.Compare(a.Value, b.Value, StringComparison.Ordinal));

            foreach (KeyValuePair<string, string> item in list) {
                screen.selectCountryScreen.List.AddItem(item);
            }
        }

        [OnEventFire]
        public void Continue(SelectCountryEvent e, Node stub, [JoinAll] SingleNode<SelectCountryScreenComponent> screen,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent(new ConfirmUserCountryEvent {
                    CountryCode = e.CountryCode
                },
                selfUser);

            ScheduleEvent(new ShowScreenLeftEvent<GoodsSelectionScreenComponent>(), screen);

            ScheduleEvent(new PaymentStatisticsEvent {
                    Action = PaymentStatisticsAction.COUNTRY_SELECT,
                    Screen = screen.component.gameObject.name
                },
                session);
        }

        [OnEventFire]
        public void ChangeCountry(ConfirmUserCountryEvent e, SingleNode<UserCountryComponent> country) =>
            country.component.CountryCode = e.CountryCode;

        public class ScreenNode : Node {
            public SelectCountryScreenComponent selectCountryScreen;
        }

        public class ActiveScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public ScreenComponent screen;
        }

        public class UserWithCountryNode : Node {
            public SelfUserComponent selfUser;

            public UserCountryComponent userCountry;
        }

        [Not(typeof(UserCountryComponent))]
        public class UserWithoutCountryNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}