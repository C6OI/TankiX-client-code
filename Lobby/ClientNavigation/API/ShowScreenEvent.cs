using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class ShowScreenEvent : Event {
        public ShowScreenEvent(Type screenType, AnimationDirection animationDirection) =>
            ShowScreenData = new ShowScreenData(screenType, animationDirection);

        public ShowScreenData ShowScreenData { get; protected set; }

        public void SetContext(Entity context, bool autoDelete) {
            ShowScreenData.Context = context;
            ShowScreenData.AutoDeleteContext = autoDelete;
        }
    }
}