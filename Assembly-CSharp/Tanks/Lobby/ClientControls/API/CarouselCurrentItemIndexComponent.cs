using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class CarouselCurrentItemIndexComponent : Component {
        public CarouselCurrentItemIndexComponent(int index) => Index = index;

        public int Index { get; set; }
    }
}