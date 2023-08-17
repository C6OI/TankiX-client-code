using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientGraphics.API {
    public abstract class DynamicDecalManager {
        protected const int BASE_RENDER_QUEUE = 2100;

        const int DECAL_COUNT_LIMIT = 500;

        protected const float DECAL_FADE_TIME = 2f;

        const float SHADER_TIME_DIMENSION = 0.0001f;

        readonly int maxDecalCount;

        readonly GameObject root;

        protected float decalLifeTimeKoeff;

        protected int decalsCount;

        protected LinkedList<DecalEntry> decalsQueue;

        public DynamicDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff,
            LinkedList<DecalEntry> decalsQueue) {
            this.root = root;
            this.maxDecalCount = Math.Min(500, maxDecalCount);
            this.decalLifeTimeKoeff = decalLifeTimeKoeff;
            this.decalsQueue = decalsQueue;
        }

        protected abstract DecalEntryType DecalType { get; }

        protected abstract string DecalMeshObjectName { get; }

        protected void TrimQueue() {
            if (decalsCount <= maxDecalCount) {
                return;
            }

            DecalEntryType decalType = DecalType;
            LinkedListNode<DecalEntry> linkedListNode = decalsQueue.First;
            int num = 0;

            while (linkedListNode != null) {
                LinkedListNode<DecalEntry> next = linkedListNode.Next;

                if (linkedListNode.Value.type == decalType) {
                    Object.Destroy(linkedListNode.Value.gameObject);
                    decalsQueue.Remove(linkedListNode);
                    decalsCount--;
                    break;
                }

                linkedListNode = next;
            }
        }

        protected DecalEntry CreateDecalEntry(Mesh decalMesh, Material material, float timeToDestroy) {
            GameObject gameObject = new(DecalMeshObjectName);
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = decalMesh;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(material);
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = true;
            meshRenderer.useLightProbes = true;
            gameObject.transform.parent = root.transform;
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            DecalEntry result = default;
            result.gameObject = gameObject;
            result.material = meshRenderer.material;
            result.timeToDestroy = timeToDestroy;
            return result;
        }

        protected void SetMeshColorAndLifeTime(Mesh mesh, Color color, float lifeTime) {
            color.a = (Time.timeSinceLevelLoad + lifeTime) * 0.0001f;
            int vertexCount = mesh.vertexCount;
            Color[] array = new Color[mesh.vertexCount];

            for (int i = 0; i < vertexCount; i++) {
                array[i] = color;
            }

            mesh.colors = array;
        }
    }
}