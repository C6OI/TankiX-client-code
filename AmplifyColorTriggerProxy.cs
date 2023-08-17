using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[AddComponentMenu("")]
public class AmplifyColorTriggerProxy : MonoBehaviour {
    public Transform Reference;

    public AmplifyColorBase OwnerEffect;

    Rigidbody rigidBody;

    SphereCollider sphereCollider;

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = 0.01f;
        sphereCollider.isTrigger = true;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }

    void LateUpdate() {
        transform.position = Reference.position;
        transform.rotation = Reference.rotation;
    }
}