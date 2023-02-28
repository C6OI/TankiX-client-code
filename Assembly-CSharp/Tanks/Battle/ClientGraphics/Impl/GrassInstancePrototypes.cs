using System;
using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GrassInstancePrototypes {
        List<Mesh> grassMeshes = new();

        List<GrassPrefabData> grassPrefabDataList;

        public int PrototypesCount => grassMeshes.Count;

        public void CreatePrototypes(List<GrassPrefabData> grassPrefabDataList) {
            if (grassPrefabDataList.Count == 0) {
                throw new ArgumentException("GrassPrefabDataList can't be empty");
            }

            this.grassPrefabDataList = grassPrefabDataList;

            for (int i = 0; i < grassPrefabDataList.Count; i++) {
                GameObject grassPrefab = grassPrefabDataList[i].grassPrefab;
                Mesh sharedMesh = grassPrefab.GetComponent<MeshFilter>().sharedMesh;
                Mesh item = CreateMesh(sharedMesh);
                grassMeshes.Add(item);
            }

            for (int j = 0; j < grassPrefabDataList.Count; j++) {
                GameObject grassPrefab2 = grassPrefabDataList[j].grassPrefab;
                Mesh sharedMesh2 = grassPrefab2.GetComponent<MeshFilter>().sharedMesh;
                grassMeshes.Add(sharedMesh2);
            }
        }

        public void GetRandomPrototype(out Mesh mesh, out GrassPrefabData grassPrefabData) {
            int index = Random.Range(0, grassMeshes.Count);
            GetPrototype(index, out mesh, out grassPrefabData);
        }

        public void GetPrototype(int index, out Mesh mesh, out GrassPrefabData grassPrefabData) {
            if (index < 0 || index >= grassMeshes.Count) {
                throw new GrassPrototypeIndexOutOfRange(string.Format("Index: {0}, prototypes count: {1}", index, grassMeshes.Count));
            }

            mesh = grassMeshes[index];
            int index2 = index - grassPrefabDataList.Count * (index / grassPrefabDataList.Count);
            grassPrefabData = grassPrefabDataList[index2];
        }

        public void DestroyPrototypes() {
            for (int num = grassMeshes.Count - grassPrefabDataList.Count - 1; num >= 0; num--) {
                Mesh obj = grassMeshes[num];
                Object.DestroyImmediate(obj);
            }

            grassMeshes = null;
        }

        Mesh CreateMesh(Mesh sourceMesh) {
            Mesh mesh = new();
            mesh.vertices = sourceMesh.vertices;
            mesh.triangles = sourceMesh.triangles;
            mesh.bounds = sourceMesh.bounds;
            mesh.normals = sourceMesh.normals;
            mesh.uv = sourceMesh.uv;
            return mesh;
        }
    }
}