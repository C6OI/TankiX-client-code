using System;
using System.IO;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Tanks.Tool.TankViewer.API {
    public class ColoringCreationLogic : MonoBehaviour {
        public TankConstructor tankConstructor;

        public ResultsDataSource resultsDataSource;

        TextureData coloringTexture;

        ColoringComponent newColoring;

        TextureData normalMap;

        public ColoringComponent CreateNewColoring() {
            Cancel();
            GameObject gameObject = new("NewColoring");
            newColoring = gameObject.AddComponent<ColoringComponent>();
            tankConstructor.ChangeColoring(newColoring);
            return newColoring;
        }

        public void Save() {
            string text = DateTime.Now.ToString("HH-mm_dd-MM-yyyy");
            DirectoryInfo directoryInfo = Directory.CreateDirectory("Results_" + text);

            if (coloringTexture != null) {
                File.Copy(coloringTexture.filePath, directoryInfo.FullName + "/" + coloringTexture.name);
            }

            if (normalMap != null && !normalMap.filePath.Equals(coloringTexture.filePath)) {
                File.Copy(normalMap.filePath, directoryInfo.FullName + "/" + normalMap.name);
            }

            var graph = new {
                coloring = new {
                    color = ColorUtility.ToHtmlStringRGB(newColoring.color),
                    coloringTexture = coloringTexture == null ? string.Empty : coloringTexture.name,
                    coloringTextureAlphaMode = newColoring.coloringTextureAlphaMode.ToString(),
                    coloringNormalMap = normalMap == null ? string.Empty : normalMap.name, newColoring.coloringNormalScale, newColoring.metallic, newColoring.overwriteSmoothness,
                    newColoring.smoothnessStrength, newColoring.useColoringIntensityThreshold, newColoring.coloringMaskThreshold
                }
            };

            StreamWriter streamWriter = File.CreateText(directoryInfo.FullName + "/coloring.yml");
            new Serializer(SerializationOptions.DisableAliases | SerializationOptions.EmitDefaults).Serialize(streamWriter, graph);
            streamWriter.Close();
            resultsDataSource.Add(directoryInfo.Name, newColoring);
        }

        public void UpdateColoring(Color color, TextureData coloringTexture, ColoringComponent.COLORING_MAP_ALPHA_MODE alphaMode, TextureData normalMap, float normalScale, float metallic,
            bool overrideSmoothness, float smoothnessStrenght, bool useIntensityThreshold, float intensityThreshold) {
            this.coloringTexture = coloringTexture;
            this.normalMap = normalMap;
            newColoring.color = color;
            newColoring.coloringTextureAlphaMode = alphaMode;
            newColoring.coloringTexture = coloringTexture == null ? null : coloringTexture.texture2D;
            newColoring.coloringNormalMap = normalMap == null ? null : normalMap.texture2D;
            newColoring.coloringNormalScale = normalScale;
            newColoring.metallic = metallic;
            newColoring.overwriteSmoothness = overrideSmoothness;
            newColoring.smoothnessStrength = smoothnessStrenght;
            newColoring.useColoringIntensityThreshold = useIntensityThreshold;
            newColoring.coloringMaskThreshold = intensityThreshold;
            tankConstructor.ChangeColoring(newColoring);
        }

        public void UpdateColoring(ColoringComponent coloringComponent) {
            tankConstructor.ChangeColoring(coloringComponent);
        }

        public void Cancel() {
            if (newColoring != null) {
                Destroy(newColoring.gameObject);
                newColoring = null;
            }
        }

        public void CleanTextures() {
            if (newColoring != null) {
                newColoring.coloringTexture = null;
                newColoring.coloringNormalMap = null;
                tankConstructor.ChangeColoring(newColoring);
            }
        }
    }
}