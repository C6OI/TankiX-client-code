using System;
using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.Impl.Batching.Zones;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ZoneBatching : MonoBehaviour {
        public float size = 10f;

        public bool castShadows = true;

        public bool receiveShadows = true;

        public bool useLigthProbes;

        public Transform anchorOverride;

        public bool encodePositionInColor;

        readonly List<Submesh> candidates = new();

        readonly CandidatesComparer candidatesComparer = new();

        readonly GroupingOrderComparer orderComparer = new();

        [NonSerialized] public int afterSubmeshes;

        [NonSerialized] public int beforeSubmeshes;

        List<Zone> zones;

        void Start() {
            SetupZones();
            MakeBatches();
        }

        void OnDrawGizmosSelected() {
            if (enabled) {
                for (int i = 0; i < zones.Count; i++) {
                    GizmoBounds(zones[i].bounds, Color.yellow);
                }
            }
        }

        void OnValidate() => SetupZones();

        void MakeBatches() {
            CombineParts(zones);
            RemoveOriginalSubmeshes(zones);
        }

        void CombineParts(List<Zone> zones) {
            for (int i = 0; i < zones.Count; i++) {
                Zone zone = zones[i];
                Mesh sharedMesh = CombineMeshes(zone.contents, zone.bounds, true);
                string text = "Zone " + zone.material.name;

                if (zone.lightmapIndex >= 0) {
                    text = text + " " + zone.lightmapIndex;
                }

                GameObject gameObject = new(text);
                gameObject.layer = this.gameObject.layer;
                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = sharedMesh;
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = zone.material;
                meshRenderer.lightmapIndex = zone.lightmapIndex;
                meshRenderer.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
                meshRenderer.receiveShadows = receiveShadows;
                meshRenderer.useLightProbes = useLigthProbes;

                if (anchorOverride != null) {
                    meshRenderer.probeAnchor = anchorOverride;
                }

                gameObject.transform.position = zone.bounds.center;
                gameObject.transform.parent = transform;
            }
        }

        void RemoveOriginalSubmeshes(List<Zone> zones) {
            Dictionary<MeshRenderer, List<Submesh>> dictionary = new();

            for (int i = 0; i < zones.Count; i++) {
                Zone zone = zones[i];

                for (int j = 0; j < zone.contents.Count; j++) {
                    Submesh submesh = zone.contents[j];
                    MeshRenderer renderer = submesh.renderer;
                    List<Submesh> list = null;

                    if (dictionary.ContainsKey(renderer)) {
                        list = dictionary[renderer];
                    } else {
                        list = new List<Submesh>();
                        dictionary.Add(renderer, list);
                    }

                    list.Add(submesh);
                }
            }

            foreach (KeyValuePair<MeshRenderer, List<Submesh>> item in dictionary) {
                MeshRenderer key = item.Key;
                List<Submesh> value = item.Value;
                MeshFilter component = key.GetComponent<MeshFilter>();
                Mesh sharedMesh = component.sharedMesh;

                if (sharedMesh.subMeshCount == value.Count) {
                    Destroy(component);
                    Destroy(key);
                    GameObject gameObject = key.gameObject;

                    if (gameObject.transform.childCount == 0 && !HasImportantComponents(gameObject)) {
                        key.gameObject.SetActive(false);
                    }
                } else {
                    RemoveSubmeshesFromMesh(component, key, value);
                }
            }
        }

        static bool HasImportantComponents(GameObject gm) {
            Component[] components = gm.GetComponents<Component>();

            foreach (Component component in components) {
                if (!(component is Transform) && !(component is MeshFilter) && !(component is MeshRenderer)) {
                    return true;
                }
            }

            return false;
        }

        void RemoveSubmeshesFromMesh(MeshFilter meshFilter, MeshRenderer meshRenderer, List<Submesh> list) {
            Mesh sharedMesh = meshFilter.sharedMesh;
            Mesh mesh = new();
            Material[] materials = meshRenderer.materials;
            List<Material> list2 = new();
            int num = 0;
            List<CombineInstance> list3 = new();

            for (int i = 0; i < sharedMesh.subMeshCount; i++) {
                if (num >= materials.Length) {
                    num = materials.Length - 1;
                }

                if (!ContainsSubmeshWithIndex(list, i)) {
                    CombineInstance item = default;
                    item.mesh = sharedMesh;
                    item.subMeshIndex = i;
                    list3.Add(item);
                    list2.Add(materials[num]);
                }

                num++;
            }

            mesh.CombineMeshes(list3.ToArray(), false, false);
            meshFilter.mesh = mesh;
            meshRenderer.materials = list2.ToArray();
        }

        static bool ContainsSubmeshWithIndex(List<Submesh> list, int index) {
            for (int i = 0; i < list.Count; i++) {
                Submesh submesh = list[i];

                if (index == submesh.submesh) {
                    return true;
                }
            }

            return false;
        }

        public void SetupZones() {
            beforeSubmeshes = 0;
            List<Submesh> parts = CollectParts(gameObject, size);
            List<Zone> list = CombineZones(parts, size);
            afterSubmeshes = beforeSubmeshes;

            for (int i = 0; i < list.Count; i++) {
                Zone zone = list[i];
                afterSubmeshes = afterSubmeshes - zone.contents.Count + 1;
            }

            zones = list.Count <= 0 ? new List<Zone>() : list;
        }

        List<Zone> CombineZones(List<Submesh> parts, float maxSize) {
            SortForBetterGrouping(parts, maxSize);
            List<Zone> list = new();

            for (int i = 0; i < parts.Count; i++) {
                Submesh submesh = parts[i];

                if (submesh.merged || submesh.nearValue == 0) {
                    continue;
                }

                Vector3 vector = default;

                for (int j = i + 1; j < parts.Count; j++) {
                    Submesh submesh2 = parts[j];

                    if (!submesh2.merged &&
                        !(submesh2.material != submesh.material) &&
                        submesh2.lightmapIndex == submesh.lightmapIndex &&
                        CanGroup(submesh.bounds, submesh2.bounds, maxSize)) {
                        vector += submesh2.bounds.center;
                        candidates.Add(submesh2);
                    }
                }

                if (candidates.Count <= 0) {
                    continue;
                }

                Zone zone = new();
                zone.bounds = submesh.bounds;
                zone.contents = new List<Submesh>();
                zone.contents.Add(submesh);
                submesh.merged = true;
                candidatesComparer.center = vector / candidates.Count;
                candidates.Sort(candidatesComparer);

                for (int k = 0; k < candidates.Count; k++) {
                    Submesh submesh3 = candidates[k];

                    if (k == 0 || CanGroup(zone.bounds, submesh3.bounds, maxSize)) {
                        zone.bounds.Encapsulate(submesh3.bounds);
                        zone.contents.Add(submesh3);
                        submesh3.merged = true;
                    }
                }

                list.Add(zone);
                candidates.Clear();
            }

            return list;
        }

        static bool CanGroup(Bounds a, Bounds b, float maxSize) {
            float magnitude = (a.center - b.center).magnitude;
            return a.extents.magnitude + b.extents.magnitude + magnitude <= maxSize;
        }

        void SortForBetterGrouping(List<Submesh> parts, float maxSize) {
            for (int i = 0; i < parts.Count; i++) {
                Submesh submesh = parts[i];

                for (int j = i + 1; j < parts.Count; j++) {
                    Submesh submesh2 = parts[j];
                    float magnitude = submesh.bounds.extents.magnitude;

                    float num = submesh2.bounds.extents.magnitude +
                                (submesh.bounds.center - submesh2.bounds.center).magnitude +
                                magnitude;

                    if (num <= maxSize) {
                        float num2 = (num - magnitude - magnitude) / magnitude;

                        if (num2 <= 0.1) {
                            submesh.nearValue += 10;
                        } else if (num2 <= 0.2) {
                            submesh.nearValue += 5;
                        } else if (num2 <= 0.3) {
                            submesh.nearValue += 3;
                        } else {
                            submesh.nearValue++;
                        }
                    }
                }
            }

            parts.Sort(orderComparer);
        }

        List<Submesh> CollectParts(GameObject root, float maxSize) {
            MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();
            List<Submesh> list = new();

            foreach (MeshRenderer meshRenderer in componentsInChildren) {
                if (meshRenderer.enabled && !meshRenderer.isPartOfStaticBatch) {
                    MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
                    Mesh mesh = !(component != null) ? null : component.sharedMesh;

                    if (mesh != null) {
                        beforeSubmeshes += mesh.subMeshCount;
                        CollectMeshParts(list, mesh, meshRenderer, maxSize);
                    }
                }
            }

            return list;
        }

        void CollectMeshParts(List<Submesh> collector, Mesh mesh, MeshRenderer renderer, float maxSize) {
            Matrix4x4 localToWorldMatrix = renderer.localToWorldMatrix;
            Material[] sharedMaterials = renderer.sharedMaterials;
            int num = 0;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < mesh.subMeshCount; i++) {
                if (num >= sharedMaterials.Length) {
                    num = sharedMaterials.Length - 1;
                }

                Bounds bounds = CalculatePartBounds(vertices, mesh.GetTriangles(i), localToWorldMatrix);
                float num2 = 2f * bounds.extents.magnitude;

                if (num2 <= maxSize) {
                    Submesh submesh = new();
                    submesh.renderer = renderer;
                    submesh.material = sharedMaterials[num];
                    submesh.mesh = mesh;
                    submesh.submesh = i;
                    submesh.bounds = bounds;
                    collector.Add(submesh);
                }

                num++;
            }
        }

        static Bounds CalculatePartBounds(Vector3[] vertices, int[] triangles, Matrix4x4 matrix) {
            bool flag = false;
            Bounds result = default;

            foreach (int num in triangles) {
                Vector3 vector = matrix.MultiplyPoint3x4(vertices[num]);

                if (!flag) {
                    result.center = vector;
                    flag = true;
                } else {
                    result.Encapsulate(vector);
                }
            }

            return result;
        }

        Mesh CombineMeshes(List<Submesh> meshes, Bounds bounds, bool moveInCenter) {
            Mesh mesh = new();
            Dictionary<int, int> dictionary = new();
            int num = 0;
            List<Vector3> list = new();
            List<Vector2> list2 = new();
            List<Vector2> list3 = new();
            List<Vector3> list4 = new();
            List<Vector4> list5 = new();
            List<int> list6 = new();
            List<Color32> list7 = null;

            if (encodePositionInColor) {
                list7 = new List<Color32>();
            }

            Vector2 defaultValue = new(0f, 0f);
            Vector3 vector = new(0f, 0f, 1f);
            Vector4 defaultValue2 = new(1f, 0f, 0f, 1f);
            Vector3 center = bounds.center;

            for (int i = 0; i < meshes.Count; i++) {
                Submesh submesh = meshes[i];
                Mesh mesh2 = submesh.mesh;
                int submesh2 = submesh.submesh;
                Matrix4x4 localToWorldMatrix = submesh.renderer.localToWorldMatrix;
                Matrix4x4 worldToLocalMatrix = submesh.renderer.worldToLocalMatrix;
                Vector4 lightmapScaleOffset = submesh.renderer.lightmapScaleOffset;
                dictionary.Clear();
                int[] triangles = mesh2.GetTriangles(submesh2);

                if (triangles.Length == 0) {
                    continue;
                }

                Vector3[] vertices = mesh2.vertices;
                Vector2[] uv = mesh2.uv;
                Vector2[] array = mesh2.uv2.Length <= 0 ? null : mesh2.uv2;
                Vector3[] array2 = mesh2.normals.Length <= 0 ? null : mesh2.normals;
                Vector4[] array3 = mesh2.tangents.Length <= 0 ? null : mesh2.tangents;

                foreach (int num2 in triangles) {
                    int num3;

                    if (!dictionary.ContainsKey(num2)) {
                        num3 = num;
                        dictionary.Add(num2, num3);
                        num++;
                        Vector3 item = localToWorldMatrix.MultiplyPoint3x4(vertices[num2]);

                        if (moveInCenter) {
                            item -= center;
                        }

                        list.Add(item);
                        list2.Add(uv[num2]);

                        if (array != null) {
                            UpdateListMinCount(list3, num3, defaultValue);
                            Vector2 vector2 = array[num2];
                            float x = lightmapScaleOffset.x * vector2.x + lightmapScaleOffset.z;
                            float y = lightmapScaleOffset.y * vector2.y + lightmapScaleOffset.w;
                            list3.Add(new Vector2(x, y));
                        }

                        if (array3 != null) {
                            UpdateListMinCount(list5, num3, defaultValue2);
                            Vector4 vector3 = array3[num2];

                            float num4 = localToWorldMatrix[0, 0] * vector3.x +
                                         localToWorldMatrix[0, 1] * vector3.y +
                                         localToWorldMatrix[0, 2] * vector3.z;

                            float num5 = localToWorldMatrix[1, 0] * vector3.x +
                                         localToWorldMatrix[1, 1] * vector3.y +
                                         localToWorldMatrix[1, 2] * vector3.z;

                            float num6 = localToWorldMatrix[2, 0] * vector3.x +
                                         localToWorldMatrix[2, 1] * vector3.y +
                                         localToWorldMatrix[2, 2] * vector3.z;

                            float num7 = Mathf.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);

                            Vector4 item2 = !(num7 < 1E-06) ? new Vector4(num4 / num7, num5 / num7, num6 / num7, vector3.w)
                                                : new Vector4(1f, 0f, 0f, vector3.w);

                            list5.Add(item2);
                        }

                        if (array2 != null) {
                            UpdateListMinCount(list4, num3, vector);
                            Vector3 vector4 = array2[num2];

                            float num8 = vector4.x * worldToLocalMatrix[0, 0] +
                                         vector4.y * worldToLocalMatrix[1, 0] +
                                         vector4.z * worldToLocalMatrix[2, 0];

                            float num9 = vector4.x * worldToLocalMatrix[0, 1] +
                                         vector4.y * worldToLocalMatrix[1, 1] +
                                         vector4.z * worldToLocalMatrix[2, 1];

                            float num10 = vector4.x * worldToLocalMatrix[0, 2] +
                                          vector4.y * worldToLocalMatrix[1, 2] +
                                          vector4.z * worldToLocalMatrix[2, 2];

                            float num11 = Mathf.Sqrt(num8 * num8 + num9 * num9 + num10 * num10);

                            Vector3 item3 = !(num11 < 1E-06) ? new Vector3(num8 / num11, num9 / num11, num10 / num11)
                                                : vector;

                            list4.Add(item3);
                        }

                        if (encodePositionInColor) {
                            Vector4 column = localToWorldMatrix.GetColumn(3);
                            byte r = (byte)(column.x % 1f * 256f);
                            byte g = (byte)(column.y % 1f * 256f);
                            byte b = (byte)(column.z % 1f * 256f);
                            Color32 item4 = new(r, g, b, byte.MaxValue);
                            list7.Add(item4);
                        }
                    } else {
                        num3 = dictionary[num2];
                    }

                    list6.Add(num3);
                }
            }

            mesh.vertices = list.ToArray();
            mesh.uv = list2.ToArray();
            mesh.uv2 = list3.ToArray();
            mesh.normals = list4.ToArray();
            mesh.tangents = list5.ToArray();
            mesh.triangles = list6.ToArray();

            if (encodePositionInColor) {
                mesh.colors32 = list7.ToArray();
            }

            bounds.center = Vector3.zero;
            mesh.bounds = bounds;
            return mesh;
        }

        static void UpdateListMinCount<T>(List<T> list, int minCount, T defaultValue) {
            for (int i = list.Count; i < minCount; i++) {
                list.Add(defaultValue);
            }
        }

        static void GizmoBounds(Bounds bounds, Color color, Transform transform = null) {
            Vector3 vector = new(bounds.min.x, bounds.min.y, bounds.min.z);
            Vector3 vector2 = new(bounds.max.x, bounds.min.y, bounds.min.z);
            Vector3 vector3 = new(bounds.max.x, bounds.max.y, bounds.min.z);
            Vector3 vector4 = new(bounds.min.x, bounds.max.y, bounds.min.z);
            Vector3 vector5 = new(bounds.min.x, bounds.min.y, bounds.max.z);
            Vector3 vector6 = new(bounds.max.x, bounds.min.y, bounds.max.z);
            Vector3 vector7 = new(bounds.max.x, bounds.max.y, bounds.max.z);
            Vector3 vector8 = new(bounds.min.x, bounds.max.y, bounds.max.z);

            if (transform != null) {
                vector = transform.TransformPoint(vector);
                vector2 = transform.TransformPoint(vector2);
                vector3 = transform.TransformPoint(vector3);
                vector4 = transform.TransformPoint(vector4);
                vector5 = transform.TransformPoint(vector5);
                vector6 = transform.TransformPoint(vector6);
                vector7 = transform.TransformPoint(vector7);
                vector8 = transform.TransformPoint(vector8);
            }

            Gizmos.color = color;
            Gizmos.DrawLine(vector, vector2);
            Gizmos.DrawLine(vector2, vector3);
            Gizmos.DrawLine(vector3, vector4);
            Gizmos.DrawLine(vector4, vector);
            Gizmos.DrawLine(vector5, vector6);
            Gizmos.DrawLine(vector6, vector7);
            Gizmos.DrawLine(vector7, vector8);
            Gizmos.DrawLine(vector8, vector5);
            Gizmos.DrawLine(vector, vector5);
            Gizmos.DrawLine(vector2, vector6);
            Gizmos.DrawLine(vector4, vector8);
            Gizmos.DrawLine(vector3, vector7);
        }
    }
}