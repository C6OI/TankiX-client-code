using SSAA;
using UnityEngine;

public class SuperSampling_SSAA : MonoBehaviour {
    public float Scale;

    public bool unlocked;

    public SSAAFilter Filter = SSAAFilter.BilinearDefault;

    public bool UseDynamicOutputResolution;

    void OnEnable() {
        internal_SSAA internal_SSAA = gameObject.AddComponent<internal_SSAA>();
        internal_SSAA.hideFlags = HideFlags.HideAndDontSave;
        internal_SSAA.UseDynamicOutputResolution = UseDynamicOutputResolution;
        internal_SSAA.Filter = Filter;
        internal_SSAA.ChangeScale(Scale);
    }

    void OnDisable() => Destroy(gameObject.GetComponent<internal_SSAA>());
}