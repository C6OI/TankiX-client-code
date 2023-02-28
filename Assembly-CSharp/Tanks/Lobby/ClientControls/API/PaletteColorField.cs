using System;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    [Serializable]
    public class PaletteColorField {
        [SerializeField] Palette palette;

        [SerializeField] int uid;

        public Color Color => Apply(Color.white);

        public Color Apply(Color color) => palette.Apply(uid, color);

        public static implicit operator Color(PaletteColorField field) => field.Apply(Color.white);
    }
}