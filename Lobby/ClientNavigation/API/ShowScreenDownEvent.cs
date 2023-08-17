namespace Lobby.ClientNavigation.API {
    public class ShowScreenDownEvent<T> : ShowScreenEvent {
        public ShowScreenDownEvent()
            : base(typeof(T), AnimationDirection.DOWN) { }
    }
}