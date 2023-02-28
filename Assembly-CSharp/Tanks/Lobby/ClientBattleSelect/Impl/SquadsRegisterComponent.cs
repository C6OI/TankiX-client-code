using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadsRegisterComponent : BehaviourComponent {
        public Dictionary<long, Color> squads = new();
    }
}