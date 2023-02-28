using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class Skin : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] string structureGuid;

        [SerializeField] List<SkinSprite> sprites = new();

        Dictionary<string, SkinSprite> spritesMap = new();

        public void OnBeforeSerialize() {
            sprites = new List<SkinSprite>();

            foreach (SkinSprite value in spritesMap.Values) {
                if (value.Sprite != null) {
                    sprites.Add(value);
                }
            }
        }

        public void OnAfterDeserialize() {
            spritesMap = new Dictionary<string, SkinSprite>();

            foreach (SkinSprite sprite in sprites) {
                if (!string.IsNullOrEmpty(sprite.Uid)) {
                    spritesMap.Add(sprite.Uid, sprite);
                }
            }
        }

        public Sprite GetSprite(string uid) {
            if (!spritesMap.ContainsKey(uid)) {
                return null;
            }

            return spritesMap[uid].Sprite;
        }
    }
}