using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class StreamEffectComponent : MonoBehaviour, Component {
        [SerializeField] GameObject effectPrefab;

        public StreamEffectBehaviour Instance { get; private set; }

        public void Init(MuzzlePointComponent muzzlePoint) {
            GameObject gameObject = Instantiate(effectPrefab);
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzlePoint.Current);
            Instance = gameObject.GetComponent<StreamEffectBehaviour>();
        }
    }
}