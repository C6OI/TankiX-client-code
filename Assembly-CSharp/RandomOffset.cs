using UnityEngine;

public class RandomOffset : MonoBehaviour {
    [SerializeField] float min;

    [SerializeField] float max = 1f;

    void OnEnable() {
        GetComponent<Animator>().SetFloat("offset", Random.Range(min, max));
    }
}