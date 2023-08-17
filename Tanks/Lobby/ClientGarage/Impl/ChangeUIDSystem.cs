using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientEntrance.Impl;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.Impl.ChangeUID;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientPayment.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ChangeUIDSystem : ECSSystem {
        [OnEventFire]
        public void SetPrice(NodeAddedEvent e, ChangeUIDNode node) {
            SetPriceEvent setPriceEvent = new();
            setPriceEvent.XPrice = node.goodsXPrice.Price;
            ScheduleEvent(setPriceEvent, node);
        }

        [OnEventFire]
        public void BuyUIDChange(ConfirmButtonClickYesEvent e, XButtonNode button,
            [JoinByScreen] LoginInputFieldValidStateNode inputField, [JoinAll] SelfUserNode user) => ScheduleEvent(
            new BuyUIDChangeEvent {
                Uid = inputField.inputField.Input,
                Price = button.xPriceLabel.Price
            },
            user);

        [OnEventFire]
        public void CompleteBuyUIDChange(CompleteBuyUIDChangeEvent e, SelfUserNode userNode,
            [JoinAll] ActiveChangeUIDScreenNode activeChangeUIDScreenNode, [JoinByScreen] XButtonNode buttonNode,
            [JoinByScreen] LoginInputFieldValidStateNode inputField) {
            if (e.Success) {
                ScheduleEvent<UIDChangedEvent>(userNode);
                ScheduleEvent<ShowScreenLeftEvent<HomeScreenComponent>>(userNode);
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