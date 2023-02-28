using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using UnityEngine.Networking;
using YamlDotNet.Serialization;

namespace Tanks.Tool.TankViewer.API {
    public class ResultsDataSource : MonoBehaviour {
        readonly List<ColoringComponent> coloringList = new();
        readonly List<string> folderNames = new();

        Dictionary<UnityWebRequest, ColoringComponent> loadingNormalMaps;

        Dictionary<UnityWebRequest, ColoringComponent> loadingTextures;

        public Action onChange;

        public bool IsReady { get; private set; }

        void Awake() {
            UpdateData();
        }

        public void Update() {
            if (IsReady || loadingNormalMaps == null || TryComplete()) {
                return;
            }

            List<UnityWebRequest> list = loadingTextures.Keys.ToList();

            for (int num = list.Count - 1; num >= 0; num--) {
                UnityWebRequest unityWebRequest = list[num];

                if (unityWebRequest.isNetworkError) {
                    Debug.Log(unityWebRequest.error);
                    loadingTextures.Remove(unityWebRequest);
                } else if (unityWebRequest.isDone) {
                    Texture2D texture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                    ColoringComponent coloringComponent = loadingTextures[unityWebRequest];
                    coloringComponent.coloringTexture = TextureLoadingUtility.CreateTextureWithGamma(texture);
                    loadingTextures.Remove(unityWebRequest);
                }
            }

            list = loadingNormalMaps.Keys.ToList();

            for (int num2 = list.Count - 1; num2 >= 0; num2--) {
                UnityWebRequest unityWebRequest2 = list[num2];

                if (unityWebRequest2.isNetworkError) {
                    Debug.Log(unityWebRequest2.error);
                    loadingNormalMaps.Remove(unityWebRequest2);
                } else if (unityWebRequest2.isDone) {
                    Texture2D texture2 = ((DownloadHandlerTexture)unityWebRequest2.downloadHandler).texture;
                    ColoringComponent coloringComponent2 = loadingNormalMaps[unityWebRequest2];
                    coloringComponent2.coloringNormalMap = TextureLoadingUtility.CreateNormalMap(texture2);
                    loadingNormalMaps.Remove(unityWebRequest2);
                }
            }
        }

        public void UpdateData() {
            IsReady = false;
            string[] directories = Directory.GetDirectories(".", "Results_*", SearchOption.TopDirectoryOnly);
            folderNames.Clear();
            coloringList.Clear();
            loadingTextures = new Dictionary<UnityWebRequest, ColoringComponent>();
            loadingNormalMaps = new Dictionary<UnityWebRequest, ColoringComponent>();
            string[] array = directories;

            foreach (string text in array) {
                Dictionary<object, object> config = GetConfig(text);

                if (config == null) {
                    continue;
                }

                config = (Dictionary<object, object>)config.First().Value;
                string text2 = config["coloringTexture"] as string;
                string text3 = config["coloringNormalMap"] as string;

                if (TexInfoValid(text, text2) && TexInfoValid(text, text3)) {
                    GameObject gameObject = new("ResultColoring");
                    ColoringComponent coloringComponent = gameObject.AddComponent<ColoringComponent>();
                    folderNames.Add(text);
                    coloringList.Add(coloringComponent);
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (string)config["color"], out color);
                    coloringComponent.color = color;
                    string text4 = Path.GetFullPath(text) + "/";

                    if (!string.IsNullOrEmpty(text2)) {
                        UnityWebRequest texture = UnityWebRequestTexture.GetTexture(text4 + text2);
                        texture.Send();
                        loadingTextures.Add(texture, coloringComponent);
                    }

                    if (!string.IsNullOrEmpty(text3)) {
                        UnityWebRequest texture2 = UnityWebRequestTexture.GetTexture(text4 + text3);
                        texture2.Send();
                        loadingNormalMaps.Add(texture2, coloringComponent);
                    }

                    switch ((string)config["coloringTextureAlphaMode"]) {
                        case "AS_EMISSION_MASK":
                            coloringComponent.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK;
                            break;

                        case "AS_SMOOTHNESS":
                            coloringComponent.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS;
                            break;

                        default:
                            coloringComponent.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE;
                            break;
                    }

                    coloringComponent.coloringNormalScale = float.Parse((string)config["coloringNormalScale"]);
                    coloringComponent.metallic = float.Parse((string)config["metallic"]);
                    coloringComponent.overwriteSmoothness = bool.Parse((string)config["overwriteSmoothness"]);
                    coloringComponent.smoothnessStrength = float.Parse((string)config["smoothnessStrength"]);
                    coloringComponent.useColoringIntensityThreshold = bool.Parse((string)config["useColoringIntensityThreshold"]);
                    coloringComponent.coloringMaskThreshold = float.Parse((string)config["coloringMaskThreshold"]);
                }
            }

            TryComplete();
        }

        bool TryComplete() {
            if (loadingNormalMaps.Count == 0 && loadingTextures.Count == 0 && !IsReady) {
                IsReady = true;

                if (onChange != null) {
                    Debug.Log("call onChange");
                    onChange();
                }

                return true;
            }

            return false;
        }

        public bool TexInfoValid(string dir, string texName) {
            if (string.IsNullOrEmpty(texName)) {
                return true;
            }

            if (File.Exists(dir + "/" + texName)) {
                return true;
            }

            Debug.Log("File not exist " + dir + "/" + texName);
            return false;
        }

        Dictionary<object, object> GetConfig(string dir) {
            string[] files = Directory.GetFiles(dir);
            string[] array = files;

            foreach (string path in array) {
                if (Path.GetFileName(path).Equals("coloring.yml")) {
                    using (StreamReader input = new(path)) {
                        object obj = new Deserializer().Deserialize(input);
                        return (Dictionary<object, object>)obj;
                    }
                }
            }

            return null;
        }

        public List<string> GetFolderNames() => IsReady ? folderNames : null;

        public List<ColoringComponent> GetColoringComponents() => IsReady ? coloringList : null;

        public void Add(string directoryName, ColoringComponent coloring) {
            folderNames.Add(directoryName);
            coloringList.Add(coloring);
            onChange();
        }
    }
}