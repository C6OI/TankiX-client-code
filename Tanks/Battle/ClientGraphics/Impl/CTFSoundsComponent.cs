using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824351950335226L)]
    public class CTFSoundsComponent : Component {
        public GameObject EffectRoot { get; set; }

        public AudioSource FlagLost { get; set; }

        public AudioSource FlagReturn { get; set; }

        public AudioSource FlagStole { get; set; }

        public AudioSource FlagWin { get; set; }
    }
}