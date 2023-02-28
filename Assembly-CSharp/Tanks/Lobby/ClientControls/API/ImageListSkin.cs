using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class ImageListSkin : ImageSkin {
        [SerializeField] List<string> uids = new();

        [SerializeField] List<string> names = new();

        [SerializeField] int selectedSpriteIndex;

        public int SelectedSpriteIndex {
            get => selectedSpriteIndex;
            set {
                selectedSpriteIndex = value;
                SelectSprite(names[selectedSpriteIndex]);
            }
        }

        public int Count => uids.Count;

        protected override void OnEnable() {
            if (selectedSpriteIndex >= 0 && selectedSpriteIndex < uids.Count) {
                SpriteUid = uids[selectedSpriteIndex];
            }

            base.OnEnable();
        }

        public void SelectSprite(string name) {
            int num = names.IndexOf(name);

            if (num < 0) {
                throw new SpriteNotFoundException(name);
            }

            SpriteUid = uids[num];
            selectedSpriteIndex = num;
        }

        public class SpriteNotFoundException : ArgumentException {
            public SpriteNotFoundException(string name)
                : base("Sprite with name " + name + " not found") { }
        }
    }
}