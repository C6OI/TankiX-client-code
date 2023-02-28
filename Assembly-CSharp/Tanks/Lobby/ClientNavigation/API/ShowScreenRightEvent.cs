namespace Tanks.Lobby.ClientNavigation.API {
    public class ShowScreenRightEvent<T> : ShowScreenEvent {
        public ShowScreenRightEvent()
            : base(typeof(T), AnimationDirection.RIGHT) { }
    }
}