using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class FractionButtonComponent : BehaviourComponent {
        public enum FractionActions {
            SELECT = 0,
            AWARDS = 1,
            LEARN_MORE = 2
        }

        public FractionActions Action;

        [HideInInspector] public Entity FractionEntity;
    }
}