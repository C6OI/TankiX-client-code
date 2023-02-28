using System;
using System.Collections.Generic;
using Platform.Library.ClientLogger.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientGraphics.API {
    public class GrassGenerator : MonoBehaviour {
        public GrassLocationParams grassLocationParams = new();

        public List<GrassPrefabData> grassPrefabDataList = new();

        public List<GrassCell> grassCells;

        public float farCullingDistance = 200f;

        public float nearCullingDistance = 100f;

        public float fadeRange = 15f;

        public float denstyMultipler = 1f;

        public bool ReadyForGeneration => grassCells != null && grassCells.Count > 0 && GrassPrefabsAreValid();

        public void SetCulling(float farCullingDistance, float nearCullingDistance, float fadeRange, float denstyMultipler) {
            this.farCullingDistance = farCullingDistance;
            this.nearCullingDistance = nearCullingDistance;
            this.fadeRange = fadeRange;
            this.denstyMultipler = denstyMultipler;
            InitCameraCulling();
            AdjustGrassDensityByVideoMemorySize();
        }

        void AdjustGrassDensityByVideoMemorySize() {
            if (SystemInfo.graphicsMemorySize <= 512) {
                farCullingDistance = 0f;
                denstyMultipler = 0f;
            } else if (SystemInfo.graphicsMemorySize <= 1100) {
                denstyMultipler = Mathf.Min(0.3f, denstyMultipler);
            } else if (SystemInfo.graphicsMemorySize <= 2100) {
                denstyMultipler = Mathf.Min(0.4f, denstyMultipler);
            }
        }

        void InitCameraCulling() {
            Camera main = Camera.main;
            float[] layerCullDistances = main.layerCullDistances;
            layerCullDistances[Layers.GRASS] = farCullingDistance;
            main.layerCullDistances = layerCullDistances;
        }

        public void Generate() {
            DeleteGrass();

            if (grassCells.Count == 0) {
                LoggerProvider.GetLogger(this).Error(string.Format("GrassGenerator {0} : combined grass positions are not ready.", name));
                return;
            }

            for (int num = grassPrefabDataList.Count - 1; num >= 0; num--) {
                try {
                    ValidateGrassPrefab(grassPrefabDataList[num]);
                } catch (Exception ex) {
                    LoggerProvider.GetLogger(this).Error(ex.Message);
                    grassPrefabDataList.RemoveAt(num);
                }
            }

            GrassInstancePrototypes grassInstancePrototypes = new();

            try {
                grassInstancePrototypes.CreatePrototypes(grassPrefabDataList);
                Generate(grassInstancePrototypes, grassCells);
            } catch (Exception exception) {
                LoggerProvider.GetLogger(this).Error("GrassGenerator " + name + ": grass generation failed", exception);
                DeleteGrass();
            } finally {
                grassInstancePrototypes.DestroyPrototypes();
            }
        }

        public void Validate() {
            ValidateGrassLocation();
            ValidateGrassPrefabs();
        }

        public void ValidateGrassPrefabs() {
            if (grassPrefabDataList.Count == 0) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: List of grass prefabs is empty", name));
            }

            for (int num = grassPrefabDataList.Count - 1; num >= 0; num--) {
                ValidateGrassPrefab(grassPrefabDataList[num]);
            }
        }

        void ValidateGrassLocation() {
            if (grassLocationParams.uvMask == null) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: <b>Mask</b> isn't set", name));
            }

            if (grassLocationParams.terrainObjects.Count == 0) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: <b>Terrain objects</b> aren't set", name));
            }

            for (int i = 0; i < grassLocationParams.terrainObjects.Count; i++) {
                GameObject gameObject = grassLocationParams.terrainObjects[i];

                if (gameObject == null) {
                    throw new GrassGeneratorException(string.Format("GrassGenerator {0}: terrainObject '{1}' isn't set", name, i));
                }
            }

            if (grassLocationParams.densityPerMeter <= 0f) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: <b>Density</b> {1} is incorrect. Density has to be more than zero.", name,
                    grassLocationParams.densityPerMeter));
            }

            if (grassLocationParams.grassCombineWidth <= 0f) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: <b>Grass combine width</b> {1} is incorrect.It has to be more than zero", name,
                    grassLocationParams.grassCombineWidth));
            }
        }

        public void CleanGrassPositions() {
            if (grassCells != null) {
                grassCells.Clear();
            }
        }

        void Generate(GrassInstancePrototypes grassInstancePrototypes, List<GrassCell> grassCells) {
            MeshRenderer component = grassPrefabDataList[0].grassPrefab.GetComponent<MeshRenderer>();
            MeshBuilder meshBuilder = new();
            MeshBuffersCache cache = new();
            GrassColors componentInParent = GetComponentInParent<GrassColors>();
            Dictionary<int, Material> dictionary = new();

            foreach (GrassCell grassCell in grassCells) {
                Material value;

                if (!dictionary.TryGetValue(grassCell.lightmapId, out value)) {
                    value = new Material(component.sharedMaterial);
                    value.SetFloat("_GrassCullingRange", fadeRange);
                    value.SetFloat("_GrassCullingDistance", farCullingDistance);

                    if (grassCell.lightmapId >= 0) {
                        value.SetTexture("_LightMap", LightmapSettings.lightmaps[grassCell.lightmapId].lightmapColor);
                    }

                    dictionary.Add(grassCell.lightmapId, value);
                }

                CombineGrass(meshBuilder, cache, grassInstancePrototypes, grassCell.grassPositions, componentInParent);
                Mesh mesh = new();
                meshBuilder.BuildToMesh(mesh, false);
                GameObject gameObject = new("GrassCell");
                gameObject.layer = Layers.GRASS;
                GameObject gameObject2 = gameObject;
                gameObject2.AddComponent<MeshFilter>().sharedMesh = mesh;
                MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
                meshRenderer.material = value;
                meshRenderer.receiveShadows = component.receiveShadows;
                meshRenderer.shadowCastingMode = component.shadowCastingMode;
                meshRenderer.lightProbeUsage = LightProbeUsage.Off;
                gameObject2.transform.SetParent(this.gameObject.transform, true);
                gameObject2.transform.position = grassCell.center;
                gameObject2.isStatic = true;
            }
        }

        void CombineGrass(MeshBuilder builder, MeshBuffersCache cache, GrassInstancePrototypes grassInstancePrototypes, List<GrassPosition> combinedPositions, GrassColors colors) {
            builder.Clear();
            Dictionary<int, float> dictionary = new();

            for (int i = 0; i < combinedPositions.Count; i++) {
                Mesh mesh;
                GrassPrefabData grassPrefabData;
                grassInstancePrototypes.GetRandomPrototype(out mesh, out grassPrefabData);
                int[] triangles = cache.GetTriangles(mesh);
                Vector3[] vertices = cache.GetVertices(mesh);
                Vector3[] normals = cache.GetNormals(mesh);
                Vector2[] uVs = cache.GetUVs(mesh);
                Matrix4x4 randomTransform = GetRandomTransform(grassPrefabData, combinedPositions[i].position);
                builder.ClearWeldHashing();
                Color color = Color.white;

                if (colors != null) {
                    color = colors.GetRandomColor();
                }

                dictionary.Clear();

                for (int j = 0; j < triangles.Length; j += 3) {
                    int num = triangles[j];
                    int num2 = triangles[j + 1];
                    int num3 = triangles[j + 2];
                    Vector3 v = vertices[num];
                    Vector3 v2 = vertices[num2];
                    Vector3 v3 = vertices[num3];
                    Vector3 v4 = normals[num];
                    Vector3 v5 = normals[num2];
                    Vector3 v6 = normals[num3];
                    float value = 0f;
                    int key = (int)(v4.x * 10f) ^ (int)(v4.y * 10f) ^ (int)(v4.z * 10f);

                    if (!dictionary.TryGetValue(key, out value)) {
                        value = Random.value;
                        dictionary.Add(key, value);
                    }

                    if (!(value > denstyMultipler)) {
                        v = randomTransform.MultiplyPoint(v);
                        v2 = randomTransform.MultiplyPoint(v2);
                        v3 = randomTransform.MultiplyPoint(v3);
                        v4 = randomTransform.MultiplyVector(v4);
                        v5 = randomTransform.MultiplyVector(v5);
                        v6 = randomTransform.MultiplyVector(v6);
                        Vector2 lightmapUV = combinedPositions[i].lightmapUV;
                        num = builder.AddVertexWeld(num, new VertexData(v, v4, SurfaceType.UNDEFINED), uVs[num], lightmapUV, color);
                        num2 = builder.AddVertexWeld(num2, new VertexData(v2, v5, SurfaceType.UNDEFINED), uVs[num2], lightmapUV, color);
                        num3 = builder.AddVertexWeld(num3, new VertexData(v3, v6, SurfaceType.UNDEFINED), uVs[num3], lightmapUV, color);
                        builder.AddTriangle(num, num2, num3);
                    }
                }
            }
        }

        void ValidateGrassPrefab(GrassPrefabData grassPrefabData) {
            string errorReason;

            if (!grassPrefabData.IsValid(out errorReason)) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: grass prefab {1} is not valid. {2} ", name, grassPrefabData, errorReason));
            }
        }

        bool GrassPrefabsAreValid() {
            try {
                ValidateGrassPrefabs();
                return true;
            } catch (GrassGeneratorException) {
                return false;
            }
        }

        Matrix4x4 GetRandomTransform(GrassPrefabData grassPrefabData, Vector3 position) {
            Quaternion q = GetRandomRotation() * grassPrefabData.grassPrefab.transform.rotation;
            float minScale = grassPrefabData.minScale;
            float num = minScale + Random.value * (grassPrefabData.maxScale - minScale);
            return Matrix4x4.TRS(position, q, new Vector3(num, num, num));
        }

        Quaternion GetRandomRotation() {
            Quaternion identity = Quaternion.identity;
            identity.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
            return identity;
        }

        public void DeleteGrass() {
            Transform transform = gameObject.transform;

            for (int num = transform.childCount - 1; num >= 0; num--) {
                DestroyImmediate(transform.GetChild(num).gameObject);
            }
        }

        public bool HasGeneratedGrass() => gameObject.transform.childCount > 0;
    }
}