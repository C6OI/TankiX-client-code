using System;

namespace Lobby.ClientNavigation.API {
    public class ShowFirstScreenEvent<T> : ShowFirstScreenEvent {
        public ShowFirstScreenEvent()
            : base(typeof(T)) { }
    }

    public class ShowFirstScreenEvent : ShowScreenEvent {
        public ShowFirstScreenEvent(Type screenType)
            : base(screenType, AnimationDirection.NONE) { }
    }
}