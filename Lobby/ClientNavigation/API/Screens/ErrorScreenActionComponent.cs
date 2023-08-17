using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API.Screens {
    public class ErrorScreenActionComponent : Component {
        public ErrorScreenActionComponent() { }

        public ErrorScreenActionComponent(Action action) => Action = action;

        public Action Action { get; set; }
    }
}