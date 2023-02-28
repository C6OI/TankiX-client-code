using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerBattleMixerSnapshotsComponent : BehaviourComponent {
        [SerializeField] int loudSnapshotIndex;

        [SerializeField] int silentSnapshotIndex = 1;

        [SerializeField] int selfUserSnapshotIndex = 2;

        [SerializeField] int melodySilentSnapshotIndex = 3;

        public int LoudSnapshotIndex => loudSnapshotIndex;

        public int SilentSnapshotIndex => silentSnapshotIndex;

        public int SelfUserSnapshotIndex => selfUserSnapshotIndex;

        public int MelodySilentSnapshotIndex => melodySilentSnapshotIndex;
    }
}