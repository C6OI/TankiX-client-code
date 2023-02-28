using System;
using System.Collections;
using UnityEngine;

public class ChatSection : MonoBehaviour {
    [SerializeField] Transform header;

    [SerializeField] Transform hideIcon;

    bool hiden;

    public void SwitchHideState() {
        hiden = !hiden;
        hideIcon.transform.localScale = new Vector3(1f, hiden ? 1 : -1, 1f) * 0.25f;
        IEnumerator enumerator = transform.GetEnumerator();

        try {
            while (enumerator.MoveNext()) {
                object current = enumerator.Current;

                if (current != header) {
                    ((Transform)current).gameObject.SetActive(!hiden);
                }
            }
        } finally {
            IDisposable disposable;

            if ((disposable = enumerator as IDisposable) != null) {
                disposable.Dispose();
            }
        }
    }
}