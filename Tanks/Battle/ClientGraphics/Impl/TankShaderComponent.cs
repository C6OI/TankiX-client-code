using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankShaderComponent : MonoBehaviour, Component {
        public Shader opaqueShader;

        public Shader transparentShader;

        public Shader deadTransparentShader;
    }
}