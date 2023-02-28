using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(636451471987331092L)]
    public class BattleResultsTankPositionComponent : MonoBehaviour, Component {
        public string hullGuid;

        public string weaponGuid;

        public string paintGuid;

        public string coverGuid;
    }
}