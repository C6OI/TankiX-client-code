using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FixedUpdateLimitSystem : ECSSystem {
        const float FIXED_DELTATIME = 0.02f;

        const float FIXED_OUT_DELTATIME = 0.05f;

        const int MAX_FIXEDUPDATE_PER_FRAME = 3;

        int fixedUpdatePerFrame;

        int lastFixedUpdateFrame;

        [OnEventFire]
        public void CheckFixedUpdateLimit(FixedUpdateEvent evt, SingleNode<SelfBattleUserComponent> battleUser) {
            if (Time.frameCount == lastFixedUpdateFrame) {
                fixedUpdatePerFrame++;

                if (fixedUpdatePerFrame > 3) {
                    Time.fixedDeltaTime = 0.05f;
                }

                return;
            }

            lastFixedUpdateFrame = Time.frameCount;

            if (Time.fixedDeltaTime > 0.02f) {
                Time.fixedDeltaTime = 0.02f;
            }

            fixedUpdatePerFrame = 0;
        }
    }
}