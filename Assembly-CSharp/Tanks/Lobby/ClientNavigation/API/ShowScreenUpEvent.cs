namespace Tanks.Lobby.ClientNavigation.API {
    public class ShowScreenUpEvent<T> : ShowScreenEvent {
        public ShowScreenUpEvent()
            : base(typeof(T), AnimationDirection.UP) { }
    }
}