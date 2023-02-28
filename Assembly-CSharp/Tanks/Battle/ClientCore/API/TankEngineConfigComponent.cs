using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class TankEngineConfigComponent : MonoBehaviour, Component {
        [SerializeField] [Range(0f, 1f)] float minEngineMovingBorder;

        [SerializeField] [Range(0f, 1f)] float maxEngineMovingBorder;

        [SerializeField] [Range(0f, 1f)] float engineTurningBorder;

        [SerializeField] float engineCollisionIntervalSec = 0.5f;

        public float EngineCollisionIntervalSec {
            get => engineCollisionIntervalSec;
            set => engineCollisionIntervalSec = value;
        }

        public float MinEngineMovingBorder {
            get => minEngineMovingBorder;
            set => minEngineMovingBorder = value;
        }

        public float MaxEngineMovingBorder {
            get => maxEngineMovingBorder;
            set => maxEngineMovingBorder = value;
        }

        public float EngineTurningBorder {
            get => engineTurningBorder;
            set => engineTurningBorder = value;
        }
    }
}