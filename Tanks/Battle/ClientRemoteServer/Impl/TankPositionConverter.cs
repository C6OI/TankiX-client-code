using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public static class TankPositionConverter {
        public static Vector3 ConvertedSentToServer(TankCollidersUnityComponent tankCollidersUnity) =>
            tankCollidersUnity.GetBoundsCenterGlobal();

        public static Vector3 ConvertedReceptionFromServer(Vector3 serverPosition,
            TankCollidersUnityComponent tankCollidersUnity, Vector3 transformPosition) {
            Vector3 vector = serverPosition - tankCollidersUnity.GetBoundsCenterGlobal();
            return transformPosition += vector;
        }
    }
}