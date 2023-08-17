using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using UnityEngine;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelActivator : UnityAwareActivator<AutoCompleting> {
        [SerializeField] public GameObject UserLabel;

        protected override void Activate() {
            if (UserLabel == null) {
                Debug.LogError("UserLabelActivator: not set prefab UserLabel");
            } else {
                UserLabelBuilder.userLabelPrefab = UserLabel;
            }
        }
    }
}