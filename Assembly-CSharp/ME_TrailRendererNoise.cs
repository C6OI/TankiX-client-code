using System.Collections.Generic;
using UnityEngine;

public class ME_TrailRendererNoise : MonoBehaviour {
    const float MinimumSqrDistance = 0.01f;

    const float DivisionThreshold = -0.99f;

    const float SmoothCurvesScale = 0.5f;

    [Range(0.01f, 10f)] public float MinVertexDistance = 0.1f;

    public float VertexTime = 1f;

    public float TotalLifeTime = 3f;

    public bool SmoothCurves;

    public bool IsRibbon;

    public bool IsActive = true;

    [Range(0.001f, 10f)] public float Frequency = 1f;

    [Range(0.001f, 10f)] public float TimeScale = 0.1f;

    [Range(0.001f, 10f)] public float Amplitude = 1f;

    public float Gravity = 1f;

    public float TurbulenceStrength = 1f;

    public bool AutodestructWhenNotActive;

    readonly List<Vector3> controlPoints = new();

    int curveCount;

    readonly List<float> lifeTimes = new(500);

    LineRenderer lineRenderer;

    readonly List<Vector3> points = new(500);

    Vector3 prevPos;

    float randomOffset;

    Transform t;

    readonly List<Vector3> velocities = new(500);

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        t = transform;
        prevPos = t.position;
        points.Insert(0, t.position);
        lifeTimes.Insert(0, VertexTime);
        velocities.Insert(0, Vector3.zero);
        randomOffset = Random.Range(0, 10000000) / 1000000f;
    }

    void Update() {
        if (IsActive) {
            AddNewPoints();
        }

        UpdatetPoints();

        if (SmoothCurves && points.Count > 2) {
            UpdateLineRendererBezier();
        } else {
            UpdateLineRenderer();
        }

        if (AutodestructWhenNotActive && !IsActive && points.Count <= 1) {
            Destroy(gameObject, TotalLifeTime);
        }
    }

    void OnEnable() {
        points.Clear();
        lifeTimes.Clear();
        velocities.Clear();
    }

    void AddNewPoints() {
        if ((t.position - prevPos).magnitude > MinVertexDistance || IsRibbon && points.Count == 0 ||
            IsRibbon && points.Count > 0 && (t.position - points[0]).magnitude > MinVertexDistance) {
            prevPos = t.position;
            points.Insert(0, t.position);
            lifeTimes.Insert(0, VertexTime);
            velocities.Insert(0, Vector3.zero);
        }
    }

    void UpdatetPoints() {
        for (int i = 0; i < lifeTimes.Count; i++) {
            lifeTimes[i] -= Time.deltaTime;

            if (lifeTimes[i] <= 0f) {
                int count = lifeTimes.Count - i;
                lifeTimes.RemoveRange(i, count);
                points.RemoveRange(i, count);
                velocities.RemoveRange(i, count);
                break;
            }

            CalculateTurbuelence(points[i], TimeScale, Frequency, Amplitude, Gravity, i);
        }
    }

    void UpdateLineRendererBezier() {
        if (SmoothCurves && points.Count > 2) {
            InterpolateBezier(points, 0.5f);
            List<Vector3> drawingPoints = GetDrawingPoints();
            lineRenderer.positionCount = drawingPoints.Count - 1;
            lineRenderer.SetPositions(drawingPoints.ToArray());
        }
    }

    void UpdateLineRenderer() {
        lineRenderer.positionCount = Mathf.Clamp(points.Count - 1, 0, int.MaxValue);
        lineRenderer.SetPositions(points.ToArray());
    }

    void CalculateTurbuelence(Vector3 position, float speed, float scale, float height, float gravity, int index) {
        float num = Time.timeSinceLevelLoad * speed + randomOffset;
        float x = position.x * scale + num;
        float num2 = position.y * scale + num + 10f;
        float y = position.z * scale + num + 25f;
        position.x = (Mathf.PerlinNoise(num2, y) - 0.5f) * height * Time.deltaTime;
        position.y = (Mathf.PerlinNoise(x, y) - 0.5f) * height * Time.deltaTime - gravity * Time.deltaTime;
        position.z = (Mathf.PerlinNoise(x, num2) - 0.5f) * height * Time.deltaTime;
        points[index] += position * TurbulenceStrength;
    }

    public void InterpolateBezier(List<Vector3> segmentPoints, float scale) {
        controlPoints.Clear();

        if (segmentPoints.Count < 2) {
            return;
        }

        for (int i = 0; i < segmentPoints.Count; i++) {
            if (i == 0) {
                Vector3 vector = segmentPoints[i];
                Vector3 vector2 = segmentPoints[i + 1];
                Vector3 vector3 = vector2 - vector;
                Vector3 item = vector + scale * vector3;
                controlPoints.Add(vector);
                controlPoints.Add(item);
            } else if (i == segmentPoints.Count - 1) {
                Vector3 vector4 = segmentPoints[i - 1];
                Vector3 vector5 = segmentPoints[i];
                Vector3 vector6 = vector5 - vector4;
                Vector3 item2 = vector5 - scale * vector6;
                controlPoints.Add(item2);
                controlPoints.Add(vector5);
            } else {
                Vector3 vector7 = segmentPoints[i - 1];
                Vector3 vector8 = segmentPoints[i];
                Vector3 vector9 = segmentPoints[i + 1];
                Vector3 normalized = (vector9 - vector7).normalized;
                Vector3 item3 = vector8 - scale * normalized * (vector8 - vector7).magnitude;
                Vector3 item4 = vector8 + scale * normalized * (vector9 - vector8).magnitude;
                controlPoints.Add(item3);
                controlPoints.Add(vector8);
                controlPoints.Add(item4);
            }
        }

        curveCount = (controlPoints.Count - 1) / 3;
    }

    public List<Vector3> GetDrawingPoints() {
        List<Vector3> list = new();

        for (int i = 0; i < curveCount; i++) {
            List<Vector3> list2 = FindDrawingPoints(i);

            if (i != 0) {
                list2.RemoveAt(0);
            }

            list.AddRange(list2);
        }

        return list;
    }

    List<Vector3> FindDrawingPoints(int curveIndex) {
        List<Vector3> list = new();
        Vector3 item = CalculateBezierPoint(curveIndex, 0f);
        Vector3 item2 = CalculateBezierPoint(curveIndex, 1f);
        list.Add(item);
        list.Add(item2);
        FindDrawingPoints(curveIndex, 0f, 1f, list, 1);
        return list;
    }

    int FindDrawingPoints(int curveIndex, float t0, float t1, List<Vector3> pointList, int insertionIndex) {
        Vector3 vector = CalculateBezierPoint(curveIndex, t0);
        Vector3 vector2 = CalculateBezierPoint(curveIndex, t1);

        if ((vector - vector2).sqrMagnitude < 0.01f) {
            return 0;
        }

        float num = (t0 + t1) / 2f;
        Vector3 vector3 = CalculateBezierPoint(curveIndex, num);
        Vector3 normalized = (vector - vector3).normalized;
        Vector3 normalized2 = (vector2 - vector3).normalized;

        if (Vector3.Dot(normalized, normalized2) > -0.99f || Mathf.Abs(num - 0.5f) < 0.0001f) {
            int num2 = 0;
            num2 += FindDrawingPoints(curveIndex, t0, num, pointList, insertionIndex);
            pointList.Insert(insertionIndex + num2, vector3);
            num2++;
            return num2 + FindDrawingPoints(curveIndex, num, t1, pointList, insertionIndex + num2);
        }

        return 0;
    }

    public Vector3 CalculateBezierPoint(int curveIndex, float t) {
        int num = curveIndex * 3;
        Vector3 p = controlPoints[num];
        Vector3 p2 = controlPoints[num + 1];
        Vector3 p3 = controlPoints[num + 2];
        Vector3 p4 = controlPoints[num + 3];
        return CalculateBezierPoint(t, p, p2, p3, p4);
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float num = 1f - t;
        float num2 = t * t;
        float num3 = num * num;
        float num4 = num3 * num;
        float num5 = num2 * t;
        Vector3 vector = num4 * p0;
        vector += 3f * num3 * t * p1;
        vector += 3f * num * num2 * p2;
        return vector + num5 * p3;
    }
}