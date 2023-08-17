using System.Collections;
using MeshBrush;
using UnityEngine;

public class Example_Runtime : MonoBehaviour {
    public GameObject[] exampleCubes = new GameObject[2];

    RaycastHit hit;
    RuntimeAPI mb;

    Ray paintRay;

    void Start() {
        StartCoroutine(PaintExampleCubes());

        if (!GetComponent<RuntimeAPI>()) {
            gameObject.AddComponent<RuntimeAPI>();
        }

        mb = GetComponent<RuntimeAPI>();

        for (int i = 0; i < exampleCubes.Length; i++) {
            if (exampleCubes[i] == null) {
                Debug.LogError("One or more GameObjects in the set of meshes to paint are unassigned.");
            }
        }

        mb.brushRadius = 10f;
        mb.amount = 7;
        mb.delayBetweenPaintStrokes = 0.2f;
        mb.randomScale = new Vector4(0.4f, 1.4f, 0.5f, 1.5f);
        mb.randomRotation = 100f;
        mb.meshOffset = 1.5f;
        mb.scattering = 75f;
    }

    IEnumerator PaintExampleCubes() {
        while (true) {
            if (Input.GetKey(KeyCode.P)) {
                mb.setOfMeshesToPaint = exampleCubes;
                paintRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(paintRay, out hit)) {
                    mb.Paint_MultipleMeshes(hit);
                }

                yield return new WaitForSeconds(mb.delayBetweenPaintStrokes);
            }

            yield return null;
        }
    }
}