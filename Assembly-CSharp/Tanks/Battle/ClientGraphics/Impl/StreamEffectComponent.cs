using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        public StreamEffectBehaviour Instance { get; private set; }

        public GameObject EffectPrefab {
            get => effectPrefab;
            set => effectPrefab = value;
        }

        public void Init(MuzzlePointComponent muzzlePoint) {
            GameObject gameObject = Instantiate(effectPrefab);
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzlePoint.Current);
            Instance = gameObject.GetComponent<StreamEffectBehaviour>();
            CustomRenderQueue.SetQueue(gameObject, 3150);
        }
    }
}