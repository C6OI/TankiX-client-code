using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingMapEffectComponent : MonoBehaviour, Component {
        [SerializeField] float shrubsHidingRadiusMin = 20f;

        [SerializeField] float shrubsHidingRadiusMax = 80f;

        [SerializeField] Shader hidingLeavesShader;

        [SerializeField] Shader defaultLeavesShader;

        [SerializeField] Shader hidingBillboardTreesShader;

        [SerializeField] Shader defaultBillboardTreesShader;

        public float ShrubsHidingRadiusMin {
            get => shrubsHidingRadiusMin;
            set => shrubsHidingRadiusMin = value;
        }

        public float ShrubsHidingRadiusMax {
            get => shrubsHidingRadiusMax;
            set => shrubsHidingRadiusMax = value;
        }

        public Shader HidingLeavesShader {
            get => hidingLeavesShader;
            set => hidingLeavesShader = value;
        }

        public Shader DefaultLeavesShader {
            get => defaultLeavesShader;
            set => defaultLeavesShader = value;
        }

        public Shader HidingBillboardTreesShader {
            get => hidingBillboardTreesShader;
            set => hidingBillboardTreesShader = value;
        }

        public Shader DefaultBillboardTreesShader {
            get => defaultBillboardTreesShader;
            set => defaultBillboardTreesShader = value;
        }
    }
}