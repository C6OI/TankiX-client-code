using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ParseLinkEvent : Event {
        public string Link { get; set; }

        public EventBuilder CustomNavigationEvent { get; set; }

        public Type ScreenType { get; set; }

        public Entity ScreenContext { get; set; }

        public bool ScreenContextAutoDelete { get; set; }

        public string ParseMessage { get; set; }
    }
}