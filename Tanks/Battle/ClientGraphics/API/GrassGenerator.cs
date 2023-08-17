using System;
using System.Collections.Generic;
using Platform.Library.ClientLogger.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientGraphics.API {
    public class GrassGenerator : MonoBehaviour {
        public GrassLocationParams grassLocationParams = new();

        public List<GrassPrefabData> grassPrefabDataList = new();

        public List<Color> colors = new();

        public List<GrassCell> grassCells;

        public bool tryReuseGrassMeshes;

        public float farCullingDistance = 200f;

        public float nearCullingDistance = 100f;

        public float fadeRange = 15f;

        public float denstyMultipler = 1f;

        public float farRangeProbability = 0.2f;

        public bool ReadyForGeneration => grassCells != null && grassCells.Count > 0 && GrassPrefabsAreValid();

        public void SetCulling(float farCullingDistance, float nearCullingDistance, float fadeRange, float denstyMultipler) {
            this.farCullingDistance = farCullingDistance;
            this.nearCullingDistance = nearCullingDistance;
            this.fadeRange = fadeRange;
            this.denstyMultipler = denstyMultipler;
            InitCameraCulling();
            AdjustGrassDenstyByVideoMemorySize();
        }

        void AdjustGrassDenstyByVideoMemorySize() {
            if (SystemInfo.graphicsMemorySize <= 2100) {
                denstyMultipler = 0.25f;
            } else if (SystemInfo.graphicsMemorySize <= 512) {
                farCullingDistance = 0f;
                denstyMultipler = 0f;
            }
        }

        void InitCameraCulling() {
            float[] layerCullDistances = Camera.main.layerCullDistances;
            layerCullDistances[Layers.GRASS_FAR] = farCullingDistance;
            layerCullDistances[Layers.GRASS_NEAR] = nearCullingDistance;
            Camera.main.layerCullDistances = layerCullDistances;
        }

        public void Generate() {
            DeleteGrass();

            if (grassCells.Count == 0) {
                LoggerProvider.GetLogger(this)
                    .Error(string.Format("GrassGenerator {0} : combined grass positions are not ready.", name));

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
                grassInstancePrototypes.CreatePrototypes(colors, grassPrefabDataList);
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
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: <b>Terrain objects</b> aren't set",
                    name));
            }

            for (int i = 0; i < grassLocationParams.terrainObjects.Count; i++) {
                GameObject gameObject = grassLocationParams.terrainObjects[i];

                if (gameObject == null) {
                    throw new GrassGeneratorException(string.Format("GrassGenerator {0}: terrainObject '{1}' isn't set",
                        name,
                        i));
                }
            }

            if (grassLocationParams.densityPerMeter <= 0f) {
                throw new GrassGeneratorException(string.Format(
                    "GrassGenerator {0}: <b>Density</b> {1} is incorrect. Density has to be more than zero.",
                    name,
                    grassLocationParams.densityPerMeter));
            }

            if (grassLocationParams.grassCombineWidth <= 0f) {
                throw new GrassGeneratorException(string.Format(
                    "GrassGenerator {0}: <b>Grass combine width</b> {1} is incorrect.It has to be more than zero",
                    name,
                    grassLocationParams.grassCombineWidth));
            }
        }

        public void CleanGrassPositions() {
            if (grassCells != null) {
                grassCells.Clear();
            }
        }

        void Generate(GrassInstancePrototypes grassInstancePrototypes, List<GrassCell> grassCells) {
            Color middleColor = new(0f, 0f, 0f, 0f);

            for (int i = 0; i < colors.Count; i++) {
                middleColor += colors[i];
            }

            middleColor /= colors.Count;
            MeshRenderer component = grassPrefabDataList[0].grassPrefab.GetComponent<MeshRenderer>();
            MeshBuilder meshBuilder = new();
            MeshBuffersCache cache = new();
            Material material = new(component.sharedMaterial);
            material.SetFloat("_GrassCullingRange", fadeRange);
            material.SetFloat("_GrassCullingDistance", farCullingDistance);

            for (int j = 0; j < grassCells.Count; j++) {
                GrassCell grassCell = grassCells[j];
                CombineGrass(meshBuilder, cache, grassInstancePrototypes, grassCell.grassPositions, middleColor);
                Mesh mesh = new();
                meshBuilder.BuildToMesh(mesh);
                GameObject gameObject = new("GrassCell");
                gameObject.layer = Layers.GRASS_FAR;
                gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
                meshRenderer.receiveShadows = component.receiveShadows;
                meshRenderer.shadowCastingMode = component.shadowCastingMode;
                gameObject.transform.SetParent(this.gameObject.transform, true);
                gameObject.transform.position = grassCell.center;
                gameObject.isStatic = false;
            }
        }

        void CombineGrass(MeshBuilder builder, MeshBuffersCache cache, GrassInstancePrototypes grassInstancePrototypes,
            List<GrassPosition> combinedPositions, Color middleColor) {
            builder.Clear();
            Dictionary<int, float> dictionary = new();

            for (int i = 0; i < combinedPositions.Count; i++) {
                Mesh mesh;
                GrassPrefabData grassPrefabData;
                grassInstancePrototypes.GetRandomPrototype(out mesh, out grassPrefabData);
                int[] triangles = cache.GetTriangles(mesh);
                Vector3[] vertices = cache.GetVertices(mesh);
                Vector3[] normals = cache.GetNormals(mesh);
                Vector2[] uv = mesh.uv;
                Matrix4x4 randomTransform = GetRandomTransform(grassPrefabData, combinedPositions[i].position);
                builder.ClearWeldHashing();
                Vector3 fromDirection = Vector3.up;

                if (combinedPositions[i].normal.sqrMagnitude > 0f) {
                    fromDirection = combinedPositions[i].normal;
                }

                Quaternion quaternion = Quaternion.FromToRotation(fromDirection, combinedPositions[i].normal);
                Color color = colors[Random.Range(0, colors.Count)] * 0.5f + middleColor * 0.5f;
                bool flag = Random.value < farRangeProbability;
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
                        num = builder.AddVertexWeld(num, new VertexData(v, v4, SurfaceType.UNDEFINED), uv[num], color);
                        num2 = builder.AddVertexWeld(num2, new VertexData(v2, v5, SurfaceType.UNDEFINED), uv[num2], color);
                        num3 = builder.AddVertexWeld(num3, new VertexData(v3, v6, SurfaceType.UNDEFINED), uv[num3], color);
                        builder.AddTriangle(num, num2, num3);
                    }
                }
            }
        }

        void ValidateGrassPrefab(GrassPrefabData grassPrefabData) {
            string errorReason;

            if (!grassPrefabData.IsValid(out errorReason)) {
                throw new GrassGeneratorException(string.Format("GrassGenerator {0}: grass prefab {1} is not valid. {2} ",
                    name,
                    grassPrefabData,
                    errorReason));
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