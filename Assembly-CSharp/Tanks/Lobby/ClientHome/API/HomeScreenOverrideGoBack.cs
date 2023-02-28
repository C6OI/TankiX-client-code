using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientHome.API {
    public class HomeScreenOverrideGoBack : OverrideGoBack {
        public override ShowScreenData ScreenData { get; } = new(typeof(HomeScreenComponent), AnimationDirection.DOWN);
    }
}