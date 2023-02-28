using System;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [Serializable]
    public class LocalizedField {
        [SerializeField] string uid;

        public string Value => LocalizationUtils.Localize(uid).Replace("\\n", "\n");

        public string Uid {
            get => uid;
            set => uid = value;
        }

        public static implicit operator string(LocalizedField field) => field.Value;
    }
}