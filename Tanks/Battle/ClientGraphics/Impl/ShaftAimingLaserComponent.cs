using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824352161555226L)]
    public class ShaftAimingLaserComponent : MonoBehaviour, Component {
        [SerializeField] float maxLength = 1000f;

        [SerializeField] float minLength = 8f;

        [SerializeField] GameObject asset;

        [SerializeField] float interpolationCoeff = 0.333f;

        public GameObject EffectInstance { get; set; }

        public GameObject Asset {
            get => asset;
            set => asset = value;
        }

        public float MaxLength {
            get => maxLength;
            set => maxLength = value;
        }

        public float MinLength {
            get => minLength;
            set => minLength = value;
        }

        public float InterpolationCoeff {
            get => interpolationCoeff;
            set => interpolationCoeff = value;
        }

        public Vector3 CurrentLaserDirection { get; set; }
    }
}