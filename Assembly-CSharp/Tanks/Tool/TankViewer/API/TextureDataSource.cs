using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Tanks.Tool.TankViewer.API {
    public class TextureDataSource : MonoBehaviour {
        string basePath;

        readonly List<TextureData> convertedToNormalMap = new();

        readonly List<TextureData> data = new();
        List<string> filePaths;

        int index;

        public Action onCompleteUpdatingAction;

        public Action onStartUpdatingAction;

        UnityWebRequest webRequest;

        public bool TexturesAreReady { get; private set; }

        void Update() {
            if (webRequest != null && webRequest.isNetworkError) {
                Debug.Log(webRequest.error + " url:  " + webRequest.url);
            }

            if (webRequest != null && webRequest.isDone && !TexturesAreReady) {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                data.Add(new TextureData(filePaths[index], TextureLoadingUtility.CreateTextureWithGamma(texture)));
                convertedToNormalMap.Add(new TextureData(filePaths[index], TextureLoadingUtility.CreateNormalMap(texture)));
                Destroy(texture);
                index++;

                if (index < filePaths.Count) {
                    LoadNextTexture();
                } else {
                    Complete();
                }
            }
        }

        public void UpdateData() {
            TexturesAreReady = false;
            Clean();
            onStartUpdatingAction();
            basePath = Path.GetFullPath("workspace");

            if (!Directory.Exists(basePath)) {
                Directory.CreateDirectory(basePath);
            }

            filePaths = Directory.GetFiles(basePath).ToList();

            for (int num = filePaths.Count - 1; num >= 0; num--) {
                string text = Path.GetExtension(filePaths[num]).ToLower();

                if (!text.Equals(".png") && !text.Equals(".jpg")) {
                    filePaths.RemoveAt(num);
                }
            }

            index = 0;

            if (filePaths.Count > 0) {
                LoadNextTexture();
            } else {
                Complete();
            }
        }

        void Complete() {
            webRequest = null;
            TexturesAreReady = true;
            onCompleteUpdatingAction();
        }

        void LoadNextTexture() {
            webRequest = UnityWebRequestTexture.GetTexture(filePaths[index]);
            webRequest.Send();
        }

        public List<TextureData> GetData() => data;

        public List<TextureData> GetNormalMapsData() => convertedToNormalMap;

        void Clean() {
            for (int i = 0; i < data.Count; i++) {
                Destroy(data[i].texture2D);

                if (convertedToNormalMap[i] != null) {
                    Destroy(convertedToNormalMap[i].texture2D);
                }
            }

            data.Clear();
            convertedToNormalMap.Clear();
        }
    }
}