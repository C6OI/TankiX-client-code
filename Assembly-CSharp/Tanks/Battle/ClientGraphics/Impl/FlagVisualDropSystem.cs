using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlagVisualDropSystem : ECSSystem {
        [OnEventFire]
        public void DropFlag(NodeAddedEvent e, FlagNode flag, [JoinByBattle] SingleNode<CTFConfigComponent> ctfConfig) {
            if (!(flag.flagInstance.FlagInstance.transform.parent == null)) {
                Transform transform = flag.flagInstance.FlagInstance.transform;
                Vector3 localEulerAngles = transform.GetChild(0).transform.localEulerAngles;
                Vector3 localEulerAngles2 = transform.parent.transform.localEulerAngles;
                transform.GetComponent<Sprite3D>().scale = 0f;
                Transform transform2 = transform.GetChild(0).transform;
                transform2.localEulerAngles = new Vector3(0f, localEulerAngles.y, 0f);
                transform2.localScale = new Vector3(ctfConfig.component.flagScaleOnGround, ctfConfig.component.flagScaleOnGround, ctfConfig.component.flagScaleOnGround);
                transform2.localPosition = new Vector3(0f, 0f, 0f);
                transform.parent = null;
                transform.position = flag.flagPosition.Position;
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            }
        }

        public class FlagNode : Node {
            public FlagGroundedStateComponent flagGroundedState;

            public FlagInstanceComponent flagInstance;
            public FlagPositionComponent flagPosition;
        }
    }
}