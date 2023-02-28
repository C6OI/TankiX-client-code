using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class ColorInBattleComponent : Component {
        public ColorInBattleComponent(TeamColor color) => TeamColor = color;

        public TeamColor TeamColor { get; set; }
    }
}