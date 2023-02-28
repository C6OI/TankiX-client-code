using System;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientUnityIntegration.Impl {
    [Serializable]
    public class SceneDesc {
        public bool initAfterLoading = true;

        public string sceneName;

        public Object scene;

        [NonSerialized] public Object sceneAsset;
    }
}