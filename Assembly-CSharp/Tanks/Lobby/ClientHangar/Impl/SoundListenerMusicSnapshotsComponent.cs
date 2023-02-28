using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class SoundListenerMusicSnapshotsComponent : BehaviourComponent {
        [SerializeField] int hymnLoopSnapshot;

        [SerializeField] int battleResultMusicSnapshot = 1;

        [SerializeField] int cardsContainerMusicSnapshot = 2;

        [SerializeField] int garageModuleMusicSnapshot = 3;

        public int HymnLoopSnapshot => hymnLoopSnapshot;

        public int BattleResultMusicSnapshot => battleResultMusicSnapshot;

        public int CardsContainerMusicSnapshot => cardsContainerMusicSnapshot;

        public int GarageModuleMusicSnapshot => garageModuleMusicSnapshot;
    }
}