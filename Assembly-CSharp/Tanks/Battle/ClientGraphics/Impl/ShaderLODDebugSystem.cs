using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaderLODDebugSystem : ECSSystem {
        static bool simpleShaderMode;

        [OnEventFire]
        public void Update(UpdateEvent e, SingleNode<SelfBattleUserComponent> battle) {
            if (Input.GetKeyDown(KeyCode.L)) {
                simpleShaderMode = !simpleShaderMode;
            }

            if (simpleShaderMode) {
                Shader.globalMaximumLOD = 100;
            } else {
                Shader.globalMaximumLOD = 500;
            }
        }
    }
}