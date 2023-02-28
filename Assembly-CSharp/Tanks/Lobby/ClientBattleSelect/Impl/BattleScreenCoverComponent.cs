using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleScreenCoverComponent : UIBehaviour, Component {
        public Animator battleCoverAnimator;
    }
}