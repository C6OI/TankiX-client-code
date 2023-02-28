using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlagTransparencySystem : ECSSystem {
        [OnEventFire]
        public void PickupFlag(NodeAddedEvent e, CarriedFlagNode flagNode) {
            setAlpha(flagNode.flagInstance, 0.5f);
        }

        [OnEventFire]
        public void DropFlag(NodeAddedEvent e, DroppedFlagNode flagNode) {
            setAlpha(flagNode.flagInstance, 1f);
        }

        static void setAlpha(FlagInstanceComponent flagInstanceComponent, float val) {
            GameObject flagInstance = flagInstanceComponent.FlagInstance;
            Sprite3D component = flagInstance.GetComponent<Sprite3D>();
            component.material.SetFloat("_Alpha", val);
        }

        public class CarriedFlagNode : Node {
            public FlagInstanceComponent flagInstance;

            public TankGroupComponent tankGroup;
        }

        public class DroppedFlagNode : Node {
            public FlagGroundedStateComponent flagGroundedState;
            public FlagInstanceComponent flagInstance;
        }
    }
}