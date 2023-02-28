namespace Tanks.Lobby.ClientNavigation.API {
    public class ShowScreenLeftEvent<T> : ShowScreenEvent {
        public ShowScreenLeftEvent()
            : base(typeof(T), AnimationDirection.LEFT) { }
    }
}