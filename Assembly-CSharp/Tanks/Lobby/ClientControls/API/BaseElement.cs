using System;
using System.Collections.Generic;
using Platform.Library.ClientResources.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientControls.API {
    [Serializable]
    public class BaseElement {
        [SerializeField] int canvasHeight;

        [SerializeField] int size;

        [SerializeField] List<AssetReference> skins = new();

        bool loading;

        int loadingSkinIndex;
        HashSet<SpriteRequest> requests = new();

        HashSet<SpriteRequest> resolvedRequests = new();

        Dictionary<string, Sprite> resolvedSprites = new();

        public int CanvasHeight {
            get => canvasHeight;
            set => canvasHeight = value;
        }

        public int Size {
            get => size;
            set => size = value;
        }

        public void Init() {
            loadingSkinIndex = 0;
            loading = false;
            requests.Clear();
            resolvedSprites.Clear();
            resolvedRequests.Clear();
        }

        public void RequestSprite(SpriteRequest request) {
            Sprite value;
            resolvedSprites.TryGetValue(request.Uid, out value);

            if (value != null) {
                request.Resolve(value);
                return;
            }

            foreach (AssetReference skin2 in skins) {
                if (skin2.Reference != null) {
                    Skin skin = (Skin)skin2.Reference;
                    value = skin.GetSprite(request.Uid);

                    if (value != null) {
                        resolvedSprites.Add(request.Uid, value);
                        request.Resolve(value);
                        return;
                    }
                }
            }

            if (!requests.Contains(request)) {
                requests.Add(request);
            }

            LoadNextSkin();
        }

        void LoadNextSkin() {
            if (!loading) {
                loading = true;

                while (loadingSkinIndex < skins.Count && skins[loadingSkinIndex].Reference != null) {
                    loadingSkinIndex++;
                }

                if (loadingSkinIndex < skins.Count) {
                    skins[loadingSkinIndex].OnLoaded = SkinLoaded;
                    skins[loadingSkinIndex].Load();
                }
            }
        }

        public void CancelRequest(SpriteRequest request) {
            requests.Remove(request);
        }

        public void CancelAllRequests() {
            requests.Clear();
        }

        void SkinLoaded(Object result) {
            loading = false;
            TryResolveRequests((Skin)result);

            if (requests.Count > 0) {
                LoadNextSkin();
            }
        }

        void TryResolveRequests(Skin skin) {
            Dictionary<SpriteRequest, Sprite> dictionary = new();

            foreach (SpriteRequest request in requests) {
                if (request == null) {
                    resolvedRequests.Add(request);
                    continue;
                }

                Sprite sprite = skin.GetSprite(request.Uid);

                if (sprite != null) {
                    if (!resolvedSprites.ContainsKey(request.Uid)) {
                        resolvedSprites.Add(request.Uid, sprite);
                    }

                    dictionary.Add(request, sprite);
                }
            }

            foreach (KeyValuePair<SpriteRequest, Sprite> item in dictionary) {
                SpriteRequest key = item.Key;
                Sprite value = item.Value;
                key.Resolve(value);
                resolvedRequests.Add(key);
            }

            ClearResolvedRequests();
        }

        void ClearResolvedRequests() {
            foreach (SpriteRequest resolvedRequest in resolvedRequests) {
                requests.Remove(resolvedRequest);
            }

            resolvedRequests.Clear();
        }
    }
}