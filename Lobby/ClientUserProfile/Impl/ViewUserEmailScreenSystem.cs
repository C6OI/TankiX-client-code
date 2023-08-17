using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ViewUserEmailScreenSystem : ECSSystem {
        [OnEventFire]
        public void ViewEmail(NodeAddedEvent e, SingleNode<ViewUserEmailScreenComponent> screen,
            SelfUserWithConfirmedEmailNode user) => SetEmail(user.confirmedUserEmail, screen.component);

        void SetEmail(ConfirmedUserEmailComponent userEmail, ViewUserEmailScreenComponent screen) =>
            screen.YourEmailReplaced = screen.YourEmail.Replace("%EMAIL%",
                string.Format("<color=#{1}>{0}</color>", userEmail.Email, screen.EmailColor.ToHexString()));

        [OnEventFire]
        public void EmailChanged(ConfirmedUserEmailChangedEvent e, SelfUserWithConfirmedEmailNode user,
            [JoinAll] SingleNode<ViewUserEmailScreenComponent> screen) =>
            SetEmail(user.confirmedUserEmail, screen.component);

        public class SelfUserWithConfirmedEmailNode : Node {
            public ConfirmedUserEmailComponent confirmedUserEmail;

            public SelfUserComponent selfUser;
        }
    }
}