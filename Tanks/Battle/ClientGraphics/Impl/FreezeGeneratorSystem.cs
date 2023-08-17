using System.Threading;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FreezeGeneratorSystem : ECSSystem {
        [OnEventFire]
        public void GenerateFreeze(UpdateEvent e, SingleNode<UserReadyToBattleComponent> any) {
            if (Random.Range(0f, 1f) < 0.03f) {
                Thread.Sleep(Random.Range(0, 500));
            }
        }
    }
}