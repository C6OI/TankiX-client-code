using System;
using System.Collections.Generic;
using System.Linq;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public struct RaycastInclude : IDisposable {
        int[] initialLayers;

        readonly IEnumerable<GameObject> gameObjects;

        public RaycastInclude(IEnumerable<GameObject> gameObjects) {
            initialLayers = null;
            this.gameObjects = gameObjects;

            if (gameObjects != null) {
                IncludeGameObjectsToRaycast();
            }
        }

        public void Dispose() {
            if (gameObjects != null) {
                ReturnGameObjectsLayers();
            }
        }

        void IncludeGameObjectsToRaycast() {
            int num = 0;
            initialLayers = new int[gameObjects.Count()];
            IEnumerator<GameObject> enumerator = gameObjects.GetEnumerator();

            while (enumerator.MoveNext()) {
                GameObject current = enumerator.Current;
                initialLayers[num++] = current.layer;
                current.layer = Layers.VISUAL_STATIC;
            }
        }

        void ReturnGameObjectsLayers() {
            int num = 0;
            IEnumerator<GameObject> enumerator = gameObjects.GetEnumerator();

            while (enumerator.MoveNext()) {
                GameObject current = enumerator.Current;
                current.layer = initialLayers[num++];
            }
        }
    }
}