using System;
using UnityEngine;

namespace Lobby.ClientControls.API {
    [Serializable]
    public class SkinStructureEntry {
        [SerializeField] string name;

        [SerializeField] string uid;

        [SerializeField] string parentUid;

        public string Name {
            get => name;
            set => name = value;
        }

        public string Uid {
            get => uid;
            set => uid = value;
        }

        public string ParentUid {
            get => parentUid;
            set => parentUid = value;
        }
    }
}