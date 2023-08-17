using UnityEngine;

[AddComponentMenu("Image Effects/Amplify Color Volume")]
[RequireComponent(typeof(BoxCollider))]
public class AmplifyColorVolume : AmplifyColorVolumeBase {
    void OnTriggerEnter(Collider other) {
        AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();

        if (component != null &&
            component.OwnerEffect.UseVolumes &&
            (component.OwnerEffect.VolumeCollisionMask & 1 << gameObject.layer) != 0) {
            component.OwnerEffect.EnterVolume(this);
        }
    }

    void OnTriggerExit(Collider other) {
        AmplifyColorTriggerProxy component = other.GetComponent<AmplifyColorTriggerProxy>();

        if (component != null &&
            component.OwnerEffect.UseVolumes &&
            (component.OwnerEffect.VolumeCollisionMask & 1 << gameObject.layer) != 0) {
            component.OwnerEffect.ExitVolume(this);
        }
    }
}