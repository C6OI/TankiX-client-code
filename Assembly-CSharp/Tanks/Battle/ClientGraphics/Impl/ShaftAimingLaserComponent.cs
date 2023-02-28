using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824352161555226L)]
    public class ShaftAimingLaserComponent : BehaviourComponent {
        [SerializeField] float maxLength = 1000f;

        [SerializeField] float minLength = 8f;

        [SerializeField] GameObject asset;

        [SerializeField] float interpolationCoeff = 0.333f;

        public readonly List<ShaftAimingLaserBehaviour> EffectInstances = new();

        public ShaftAimingLaserBehaviour EffectInstance {
            get {
                if (EffectInstances.Count > 0) {
                    return EffectInstances[0];
                }

                return null;
            }
            set {
                EffectInstances.RemoveAll(item => item == null);
                EffectInstances.Add(value);
            }
        }

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