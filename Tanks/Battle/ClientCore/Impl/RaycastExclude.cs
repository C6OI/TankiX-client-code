using System;
using System.Collections.Generic;
using System.Linq;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public struct RaycastExclude : IDisposable {
        int[] initialLayers;

        readonly IEnumerable<GameObject> gameObjects;

        public RaycastExclude(IEnumerable<GameObject> gameObjects) {
            initialLayers = null;
            this.gameObjects = gameObjects;

            if (gameObjects != null) {
                ExcludeGameObjectsFromRaycast();
            }
        }

        public void Dispose() {
            if (gameObjects != null) {
                ReturnGameObjectsLayers();
            }
        }

        void ExcludeGameObjectsFromRaycast() {
            int num = 0;
            initialLayers = new int[gameObjects.Count()];

            foreach (GameObject gameObject in gameObjects) {
                initialLayers[num++] = gameObject.layer;
                gameObject.layer = Layers.EXCLUSION_RAYCAST;
            }
        }

        void ReturnGameObjectsLayers() {
            int num = 0;

            foreach (GameObject gameObject in gameObjects) {
                gameObject.layer = initialLayers[num++];
            }
        }
    }
}