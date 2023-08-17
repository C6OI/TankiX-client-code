using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DigitalRuby.LightningBolt {
    [RequireComponent(typeof(LineRenderer))]
    public class LightningBoltScript : MonoBehaviour {
        [Tooltip("The game object where the lightning will emit from. If null, StartPosition is used.")]
        public GameObject StartObject;

        [Tooltip(
            "The start position where the lightning will emit from. This is in world space if StartObject is null, otherwise this is offset from StartObject position.")]
        public Vector3 StartPosition;

        [Tooltip("The game object where the lightning will end at. If null, EndPosition is used.")]
        public GameObject EndObject;

        [Tooltip(
            "The end position where the lightning will end at. This is in world space if EndObject is null, otherwise this is offset from EndObject position.")]
        public Vector3 EndPosition;

        [Range(0f, 8f)] [Tooltip("How manu generations? Higher numbers create more line segments.")]
        public int Generations = 6;

        [Range(0.01f, 1f)]
        [Tooltip(
            "How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
        public float Duration = 0.05f;

        [Tooltip("How chaotic should the lightning be? (0-1)")] [Range(0f, 1f)]
        public float ChaosFactor = 0.15f;

        [Tooltip("In manual mode, the trigger method must be called to create a bolt")]
        public bool ManualMode;

        [Tooltip("The number of rows in the texture. Used for animation.")] [Range(1f, 64f)]
        public int Rows = 1;

        [Range(1f, 64f)] [Tooltip("The number of columns in the texture. Used for animation.")]
        public int Columns = 1;

        [Tooltip("The animation mode for the lightning")]
        public LightningBoltAnimationMode AnimationMode = LightningBoltAnimationMode.PingPong;

        readonly List<KeyValuePair<Vector3, Vector3>> segments = new();

        int animationOffsetIndex;

        int animationPingPongDirection = 1;

        LineRenderer lineRenderer;

        Vector2[] offsets;

        [NonSerialized] [HideInInspector] public Random RandomGenerator = new();

        Vector2 size;

        int startIndex;

        float timer;

        void Start() {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(0);
            UpdateFromMaterialChange();
        }

        void Update() {
            if (timer <= 0f) {
                if (ManualMode) {
                    timer = Duration;
                    lineRenderer.SetVertexCount(0);
                } else {
                    Trigger();
                }
            }

            timer -= Time.deltaTime;
        }

        void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side) {
            if (directionNormalized == Vector3.zero) {
                side = Vector3.right;
                return;
            }

            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float num = Mathf.Abs(x);
            float num2 = Mathf.Abs(y);
            float num3 = Mathf.Abs(z);
            float num4;
            float num5;
            float num6;

            if (num >= num2 && num2 >= num3) {
                num4 = 1f;
                num5 = 1f;
                num6 = (0f - (y * num4 + z * num5)) / x;
            } else if (num2 >= num3) {
                num6 = 1f;
                num5 = 1f;
                num4 = (0f - (x * num6 + z * num5)) / y;
            } else {
                num6 = 1f;
                num4 = 1f;
                num5 = (0f - (x * num6 + y * num4)) / z;
            }

            side = new Vector3(num6, num4, num5).normalized;
        }

        void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount) {
            if (generation < 0 || generation > 8) {
                return;
            }

            segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));

            if (generation == 0) {
                return;
            }

            if (offsetAmount <= 0f) {
                offsetAmount = (end - start).magnitude * ChaosFactor;
            }

            while (generation-- > 0) {
                int num = startIndex;
                startIndex = segments.Count;

                for (int i = num; i < startIndex; i++) {
                    start = segments[i].Key;
                    end = segments[i].Value;
                    Vector3 vector = (start + end) * 0.5f;
                    Vector3 result;
                    RandomVector(ref start, ref end, offsetAmount, out result);
                    vector += result;
                    segments.Add(new KeyValuePair<Vector3, Vector3>(start, vector));
                    segments.Add(new KeyValuePair<Vector3, Vector3>(vector, end));
                }

                offsetAmount *= 0.5f;
            }
        }

        public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result) {
            if (Camera.main != null && Camera.main.orthographic) {
                end.z = start.z;
                Vector3 normalized = (end - start).normalized;
                Vector3 vector = new(0f - normalized.y, normalized.x, end.z);
                float num = (float)RandomGenerator.NextDouble() * offsetAmount * 2f - offsetAmount;
                result = vector * num;
            } else {
                Vector3 directionNormalized = (end - start).normalized;
                Vector3 side;
                GetPerpendicularVector(ref directionNormalized, out side);
                float num2 = ((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount;
                float angle = (float)RandomGenerator.NextDouble() * 360f;
                result = Quaternion.AngleAxis(angle, directionNormalized) * side * num2;
            }
        }

        void SelectOffsetFromAnimationMode() {
            if (AnimationMode == LightningBoltAnimationMode.None) {
                lineRenderer.material.mainTextureOffset = offsets[0];
                return;
            }

            int num;

            if (AnimationMode == LightningBoltAnimationMode.PingPong) {
                num = animationOffsetIndex;
                animationOffsetIndex += animationPingPongDirection;

                if (animationOffsetIndex >= offsets.Length) {
                    animationOffsetIndex = offsets.Length - 2;
                    animationPingPongDirection = -1;
                } else if (animationOffsetIndex < 0) {
                    animationOffsetIndex = 1;
                    animationPingPongDirection = 1;
                }
            } else if (AnimationMode == LightningBoltAnimationMode.Loop) {
                num = animationOffsetIndex++;

                if (animationOffsetIndex >= offsets.Length) {
                    animationOffsetIndex = 0;
                }
            } else {
                num = RandomGenerator.Next(0, offsets.Length);
            }

            if (num >= 0 && num < offsets.Length) {
                lineRenderer.material.mainTextureOffset = offsets[num];
            } else {
                lineRenderer.material.mainTextureOffset = offsets[0];
            }
        }

        void UpdateLineRenderer() {
            int num = segments.Count - startIndex + 1;
            lineRenderer.SetVertexCount(num);

            if (num >= 1) {
                int num2 = 0;
                lineRenderer.SetPosition(num2++, segments[startIndex].Key);

                for (int i = startIndex; i < segments.Count; i++) {
                    lineRenderer.SetPosition(num2++, segments[i].Value);
                }

                segments.Clear();
                SelectOffsetFromAnimationMode();
            }
        }

        public void Trigger() {
            timer = Duration + Mathf.Min(0f, timer);
            Vector3 start = !(StartObject == null) ? StartObject.transform.position + StartPosition : StartPosition;
            Vector3 end = !(EndObject == null) ? EndObject.transform.position + EndPosition : EndPosition;
            startIndex = 0;
            GenerateLightningBolt(start, end, Generations, Generations, 0f);
            UpdateLineRenderer();
        }

        public void UpdateFromMaterialChange() {
            size = new Vector2(1f / Columns, 1f / Rows);
            lineRenderer.material.mainTextureScale = size;
            offsets = new Vector2[Rows * Columns];

            for (int i = 0; i < Rows; i++) {
                for (int j = 0; j < Columns; j++) {
                    offsets[j + i * Columns] = new Vector2(j / (float)Columns, i / (float)Rows);
                }
            }
        }
    }
}