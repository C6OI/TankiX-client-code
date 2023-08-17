using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ShellsButtonNavigationComponent : Component {
        public ShellsButtonNavigationComponent() { }

        public ShellsButtonNavigationComponent(bool recalculateNavigationScheduled) =>
            RecalculateNavigationScheduled = recalculateNavigationScheduled;

        public bool RecalculateNavigationScheduled { get; set; }
    }
}