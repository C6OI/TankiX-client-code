using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRayEffectUpdateEvent : Event {
        public float[] bezierPointsRandomness = new float[3];
        public float[] speedMultipliers = new float[3];
    }
}