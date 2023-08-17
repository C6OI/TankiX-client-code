using System;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PhysicsSetup {
        const byte LAYERS_COUNT = 32;

        static readonly bool[] matrix = new bool[1024];

        public static void CheckCollisionMatrix() {
            AddCollisionCheck(Layers.DEFAULT, Layers.STATIC, Layers.DEFAULT);
            AddCollisionCheck(Layers.STATIC, Layers.TANK_TO_STATIC, Layers.STATIC);
            AddCollisionCheck(Layers.SELF_SEMIACTIVE_TANK_BOUNDS, Layers.REMOTE_TANK_BOUNDS);
            AddCollisionCheck(Layers.TANK_TO_TANK, Layers.TANK_TO_TANK);
            AddCollisionCheck(Layers.TRIGGER_WITH_SELF_TANK, Layers.SELF_TANK_BOUNDS);
            AddCollisionCheck(Layers.CASE, Layers.STATIC, Layers.TANK_TO_TANK, Layers.CASE);
            AddCollisionCheck(Layers.FRICTION, Layers.FRICTION, Layers.STATIC);
            AddCollisionCheck(Layers.DETACHED_WEAPON, Layers.STATIC, Layers.TANK_TO_TANK, Layers.DETACHED_WEAPON);
            Check();
        }

        static void Check() {
            for (int i = 0; i < 32; i++) {
                for (int j = 0; j < 32; j++) {
                    bool flag = matrix[GetIndex(i, j)] || matrix[GetIndex(j, i)];
                    bool flag2 = !Physics.GetIgnoreLayerCollision(i, j);

                    if (flag2 != flag) {
                        throw new Exception("Collision matrix setup error: shouldCollide=" +
                                            flag +
                                            " layer1=" +
                                            i +
                                            " layer2=" +
                                            j);
                    }
                }
            }
        }

        static void AddCollisionCheck(int layer1, params int[] layer2) {
            foreach (int j in layer2) {
                matrix[GetIndex(layer1, j)] = true;
            }
        }

        static int GetIndex(int i, int j) => i * 32 + j;
    }
}