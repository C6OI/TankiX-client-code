using System.Collections;
using UnityEngine;

public class WebsocketCouroutine : MonoBehaviour {
    public static void StartOneShotCoroutine(IEnumerator coroutine) {
        GameObject gameObject = new("WebsocketCouroutine");
        WebsocketCouroutine websocketCouroutine = gameObject.AddComponent<WebsocketCouroutine>();
        websocketCouroutine.StartCoroutine(websocketCouroutine.OneShot(coroutine));
    }

    public IEnumerator OneShot(IEnumerator coroutine) {
        yield return StartCoroutine(coroutine);

        Destroy(gameObject);
    }
}