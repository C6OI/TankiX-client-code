namespace Tanks.Lobby.ClientNavigation.API {
    public class ShowScreenNoAnimationEvent<T> : ShowScreenEvent {
        public ShowScreenNoAnimationEvent()
            : base(typeof(T), AnimationDirection.NONE) { }
    }
}