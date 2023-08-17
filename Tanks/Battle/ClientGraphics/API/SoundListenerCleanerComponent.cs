using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class SoundListenerCleanerComponent : BehaviourComponent {
        [SerializeField] float tankPartCleanTimeSec = 2f;

        [SerializeField] float mineCleanTimeSec = 2.2f;

        [SerializeField] float ctfCleanTimeSec = 5.2f;

        public float TankPartCleanTimeSec => tankPartCleanTimeSec;

        public float MineCleanTimeSec => mineCleanTimeSec;

        public float CTFCleanTimeSec => ctfCleanTimeSec;
    }
}