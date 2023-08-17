using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public class SkinStructure : ScriptableObject {
        [SerializeField] List<SkinStructureEntry> categories = new();

        [SerializeField] List<SkinStructureEntry> sprites = new();

        public List<SkinStructureEntry> Categories => categories;

        public List<SkinStructureEntry> Sprites => sprites;
    }
}