using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientControls.API {
    [Serializable]
    public class Palette : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] List<string> names = new();

        [SerializeField] List<int> uids = new();

        [SerializeField] List<Color> colors = new();

        readonly Dictionary<int, Color> colorsMap = new();

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() {
            colorsMap.Clear();

            for (int i = 0; i < uids.Count; i++) {
                colorsMap.Add(uids[i], colors[i]);
            }
        }

        public Color Get(int uid) => !colorsMap.ContainsKey(uid) ? Color.magenta : colorsMap[uid];
    }
}