using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class SubscribeCheckboxSystem : ECSSystem {
        [OnEventFire]
        public void InitSubscribeCheckbox(NodeAddedEvent e, SubscribeCheckboxNode subscribeCheckbox, [JoinAll] Optional<UserNode> user) {
            if (user.IsPresent()) {
                subscribeCheckbox.checkbox.IsChecked = user.Get().userSubscribe.Subscribed;
            } else {
                subscribeCheckbox.checkbox.IsChecked = true;
            }
        }

        [OnEventFire]
        public void SendSubscribeToServer(CheckboxEvent e, SubscribeCheckboxNode checkbox, [JoinAll] UserNode user,
            [JoinAll] Optional<SingleNode<RegistrationScreenComponent>> registration) {
            bool isChecked = e.IsChecked;

            if (!registration.IsPresent() && user.userSubscribe.Subscribed != isChecked) {
                ScheduleEvent(new SubscribeChangeEvent {
                    Subscribed = isChecked
                }, user.Entity);
            }
        }

        public class SubscribeCheckboxNode : Node {
            public CheckboxComponent checkbox;
            public SubscribeCheckboxComponent subscribeCheckbox;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;

            public UserSubscribeComponent userSubscribe;
        }
    }
}