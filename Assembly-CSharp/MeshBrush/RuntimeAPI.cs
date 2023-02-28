using UnityEngine;

namespace MeshBrush {
    public class RuntimeAPI : MonoBehaviour {
        ushort _amount = 1;

        float _brushRadius = 1f;

        float _delayBetweenPaintStrokes = 0.15f;

        float _maxSlopeFilterAngle = 30f;

        Vector4 _randomScale = new(1f, 1f, 1f, 1f);

        float _slopeInfluence = 100f;

        GameObject brushObj;

        Transform brushTransform;

        RaycastHit hit;

        GameObject holder;

        Transform holderTransform;

        GameObject paintedMesh;

        Transform paintedMeshTransform;

        float randomHeight;

        float randomWidth;

        float scatteringInsetThreshold;

        float slopeAngle;
        Transform thisTransform;

        public GameObject[] setOfMeshesToPaint { get; set; }

        public ushort amount {
            get => _amount;
            set => _amount = (ushort)Mathf.Clamp(value, 1, 100);
        }

        public float delayBetweenPaintStrokes {
            get => _delayBetweenPaintStrokes;
            set => _delayBetweenPaintStrokes = Mathf.Clamp(value, 0.1f, 1f);
        }

        public float brushRadius {
            get => _brushRadius;
            set {
                _brushRadius = value;

                if (_brushRadius <= 0.1f) {
                    _brushRadius = 0.1f;
                }
            }
        }

        public float meshOffset { get; set; }

        public float scattering { get; set; }

        public bool yAxisIsTangent { get; set; }

        public float slopeInfluence {
            get => _slopeInfluence;
            set => _slopeInfluence = Mathf.Clamp(value, 0f, 100f);
        }

        public bool activeSlopeFilter { get; set; }

        public float maxSlopeFilterAngle {
            get => _maxSlopeFilterAngle;
            set => _maxSlopeFilterAngle = Mathf.Clamp(value, 0f, 180f);
        }

        public bool inverseSlopeFilter { get; set; }

        public bool manualRefVecSampling { get; set; }

        public Vector3 sampledSlopeRefVector { get; set; }

        public float randomRotation { get; set; }

        public Vector4 randomScale {
            get => _randomScale;
            set => _randomScale = new Vector4(Mathf.Clamp(value.x, 0.01f, value.x), Mathf.Clamp(value.y, 0.01f, value.y), Mathf.Clamp(value.z, 0.01f, value.z),
                       Mathf.Clamp(value.w, 0.01f, value.w));
        }

        public Vector3 additiveScale { get; set; }

        void Start() {
            thisTransform = transform;
            scattering = 75f;
        }

        public void Paint_SingleMesh(RaycastHit paintHit) {
            if (!paintHit.collider.transform.Find("Holder")) {
                holder = new GameObject("Holder");
                holderTransform = holder.transform;
                holderTransform.position = paintHit.collider.transform.position;
                holderTransform.rotation = paintHit.collider.transform.rotation;
                holderTransform.parent = paintHit.collider.transform;
            }

            slopeAngle = activeSlopeFilter ? Vector3.Angle(paintHit.normal, !manualRefVecSampling ? Vector3.up : sampledSlopeRefVector) : !inverseSlopeFilter ? 0f : 180f;

            if (!inverseSlopeFilter ? slopeAngle < maxSlopeFilterAngle : slopeAngle > maxSlopeFilterAngle) {
                paintedMesh = Instantiate(setOfMeshesToPaint[Random.Range(0, setOfMeshesToPaint.Length)], paintHit.point, Quaternion.LookRotation(paintHit.normal));
                paintedMeshTransform = paintedMesh.transform;

                if (yAxisIsTangent) {
                    paintedMeshTransform.up = Vector3.Lerp(paintedMeshTransform.up, paintedMeshTransform.forward, slopeInfluence * 0.01f);
                } else {
                    paintedMeshTransform.up = Vector3.Lerp(Vector3.up, paintedMeshTransform.forward, slopeInfluence * 0.01f);
                }

                paintedMeshTransform.parent = holderTransform;
                ApplyRandomScale(paintedMesh);
                ApplyRandomRotation(paintedMesh);
                ApplyMeshOffset(paintedMesh, hit.normal);
            }
        }

        public void Paint_MultipleMeshes(RaycastHit paintHit) {
            scatteringInsetThreshold = brushRadius * 0.01f * scattering;

            if (brushObj == null) {
                brushObj = new GameObject("Brush");
                brushTransform = brushObj.transform;
                brushTransform.position = thisTransform.position;
                brushTransform.parent = paintHit.collider.transform;
            }

            if (!paintHit.collider.transform.Find("Holder")) {
                holder = new GameObject("Holder");
                holderTransform = holder.transform;
                holderTransform.position = paintHit.collider.transform.position;
                holderTransform.rotation = paintHit.collider.transform.rotation;
                holderTransform.parent = paintHit.collider.transform;
            }

            for (int num = amount; num > 0; num--) {
                brushTransform.position = paintHit.point + paintHit.normal * 0.5f;
                brushTransform.rotation = Quaternion.LookRotation(paintHit.normal);
                brushTransform.up = brushTransform.forward;

                brushTransform.Translate(Random.Range((0f - Random.insideUnitCircle.x) * scatteringInsetThreshold, Random.insideUnitCircle.x * scatteringInsetThreshold), 0f,
                    Random.Range((0f - Random.insideUnitCircle.y) * scatteringInsetThreshold, Random.insideUnitCircle.y * scatteringInsetThreshold), Space.Self);

                if (Physics.Raycast(brushTransform.position, -paintHit.normal, out hit, 2.5f)) {
                    slopeAngle = activeSlopeFilter ? Vector3.Angle(hit.normal, !manualRefVecSampling ? Vector3.up : sampledSlopeRefVector) : !inverseSlopeFilter ? 0f : 180f;

                    if (!inverseSlopeFilter ? slopeAngle < maxSlopeFilterAngle : slopeAngle > maxSlopeFilterAngle) {
                        paintedMesh = Instantiate(setOfMeshesToPaint[Random.Range(0, setOfMeshesToPaint.Length)], hit.point, Quaternion.LookRotation(hit.normal));
                        paintedMeshTransform = paintedMesh.transform;

                        if (yAxisIsTangent) {
                            paintedMeshTransform.up = Vector3.Lerp(paintedMeshTransform.up, paintedMeshTransform.forward, slopeInfluence * 0.01f);
                        } else {
                            paintedMeshTransform.up = Vector3.Lerp(Vector3.up, paintedMeshTransform.forward, slopeInfluence * 0.01f);
                        }

                        paintedMeshTransform.parent = holderTransform;
                    }

                    ApplyRandomScale(paintedMesh);
                    ApplyRandomRotation(paintedMesh);
                    ApplyMeshOffset(paintedMesh, hit.normal);
                }
            }
        }

        void ApplyRandomScale(GameObject sMesh) {
            randomWidth = Random.Range(randomScale.x, randomScale.y);
            randomHeight = Random.Range(randomScale.z, randomScale.w);
            sMesh.transform.localScale = new Vector3(randomWidth, randomHeight, randomWidth);
        }

        void AddConstantScale(GameObject sMesh) {
            sMesh.transform.localScale += new Vector3(Mathf.Clamp(additiveScale.x, -0.9f, additiveScale.x), Mathf.Clamp(additiveScale.y, -0.9f, additiveScale.y),
                Mathf.Clamp(additiveScale.z, -0.9f, additiveScale.z));
        }

        void ApplyRandomRotation(GameObject rMesh) {
            rMesh.transform.Rotate(new Vector3(0f, Random.Range(0f, 3.6f * Mathf.Clamp(randomRotation, 0f, 100f)), 0f));
        }

        void ApplyMeshOffset(GameObject oMesh, Vector3 offsetDirection) {
            oMesh.transform.Translate(offsetDirection.normalized * meshOffset * 0.01f, Space.World);
        }
    }
}