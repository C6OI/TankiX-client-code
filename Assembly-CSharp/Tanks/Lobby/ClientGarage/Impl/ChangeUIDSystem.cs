using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientUserProfile.Impl.ChangeUID;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ChangeUIDSystem : ECSSystem {
        [OnEventFire]
        public void CompleteBuyUIDChange(CompleteBuyUIDChangeEvent e, SelfUserNode userNode, [JoinAll] ActiveChangeUIDScreenNode activeChangeUIDScreenNode,
            [JoinByScreen] XButtonNode buttonNode, [JoinByScreen] LoginInputFieldValidStateNode inputField) {
            if (e.Success) {
                ScheduleEvent<UIDChangedEvent>(userNode);
                ScheduleEvent<ShowScreenLeftEvent<MainScreenComponent>>(userNode);
            } else {
                inputField.inputField.Input = string.Empty;
                buttonNode.confirmButton.FlipFront();
            }
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class SelfSteamUserNode : Node {
            public SteamUserComponent steamUser;
        }

        public class ChangeUIDNode : Node {
            public ChangeUIDComponent changeUid;

            public GoodsXPriceComponent goodsXPrice;

            public XPriceLabelComponent xPriceLabel;
        }

        public class XButtonNode : Node {
            public BuyButtonComponent buyButton;

            public ConfirmButtonComponent confirmButton;
            public XPriceLabelComponent xPriceLabel;
        }

        public class LoginInputFieldValidStateNode : Node {
            public InputFieldComponent inputField;

            public InputFieldValidStateComponent inputFieldValidState;
            public RegistrationLoginInputComponent registrationLoginInput;
        }

        public class ActiveChangeUIDScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public ChangeUIDScreenComponent changeUIDScreen;
        }
    }
}