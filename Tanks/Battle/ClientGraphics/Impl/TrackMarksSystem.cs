using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    class TrackMarksSystem : ECSSystem {
        const float MAX_DIRECTION_CHANGE_COS = 0.75f;

        const float MAX_NORMAL_CHANGE_COS = 0.95f;

        const string LEFT_PREFIX = "left_";

        const string RIGHT_PREFIX = "right_";

        const float SIDE_RAYCAST_SHIFT = 0.1f;

        const float MAIN_RAYCAST_SHIFT = 0.2f;

        const float MAIN_RAYCAST_MULTIPLIER = 2f;

        const float CHECK_EXTRA_CONTACTS_MULTIPILER = 1.5f;

        const int TRACK_COUNT = 2;

        const int LEFT_TRACK = 0;

        const int RIGHT_TRACK = 1;

        const int IMPORTANT_WHEELS_COUNT = 3;

        const float MIN_LAST_TRACK_UPDATE = 0.1f;

        const float MARK_ROTATION_THRESHOLD = 0.95f;

        const float MARK_ROTATION_MAX = 0.8f;

        const float MAX_DRIFT_FACTOR = 2f;

        static readonly int TRACK_LAYER_MASK = LayerMasks.VISUAL_STATIC;

        [OnEventFire]
        public void InitTrackMarksSystem(NodeAddedEvent evt, TrackMarksInitNode node) {
            TrackMarksRenderComponent trackMarksRenderComponent = new();
            TrackMarksBuilderComponent trackMarksBuilderComponent = new();
            TrackMarksComponent trackMarks = node.trackMarks;
            ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
            InitTracks(node.assembledTank.AssemblyRoot.transform, trackMarksRenderComponent, trackMarks);
            InitBuilder(trackMarksBuilderComponent, trackMarks, trackMarksRenderComponent, chassisAnimation);
            node.Entity.AddComponent(trackMarksRenderComponent);
            node.Entity.AddComponent(trackMarksBuilderComponent);
        }

        [OnEventFire]
        public void OnUpdate(UpdateEvent evt, TrackMarkUpdateNode node, [JoinAll] CameraNode cameraNode) {
            if (cameraNode.camera.Camera.enabled) {
                TrackMarksRenderComponent trackMarksRender = node.trackMarksRender;
                TrackMarksBuilderComponent trackMarksBuilder = node.trackMarksBuilder;
                ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
                TrackMarksComponent trackMarks = node.trackMarks;
                trackMarksBuilder.rigidbody = node.rigidbody.Rigidbody;

                if (NeedUpdateMarks(trackMarksBuilder, trackMarks)) {
                    UpdateSingleTrack(trackMarksBuilder,
                        chassisAnimation,
                        trackMarksRender,
                        trackMarks,
                        0,
                        trackMarksBuilder.leftWheels,
                        trackMarksBuilder.prevLeftWheelsData,
                        trackMarksBuilder.currentLeftWheelsData,
                        trackMarksBuilder.tempLeftWheelsData);

                    UpdateSingleTrack(trackMarksBuilder,
                        chassisAnimation,
                        trackMarksRender,
                        trackMarks,
                        1,
                        trackMarksBuilder.rightWheels,
                        trackMarksBuilder.prevRightWheelsData,
                        trackMarksBuilder.currentRightWheelsData,
                        trackMarksBuilder.tempRightWheelsData);
                }

                if (trackMarksRender.dirty) {
                    UpdateMesh(trackMarksRender);
                    trackMarksRender.dirty = false;
                }
            }
        }

        [OnEventFire]
        public void ClearTrackMarks(NodeRemoveEvent evt, TrackMarkUpdateNode node) => node.trackMarksRender.mesh.Clear();

        void InitTracks(Transform parent, TrackMarksRenderComponent render, TrackMarksComponent trackMarks) {
            render.mesh = new Mesh();
            render.mesh.MarkDynamic();
            GameObject gameObject = new("Track");
            gameObject.transform.SetParent(parent);
            gameObject.AddComponent<MeshFilter>().mesh = render.mesh;
            trackMarks.material = new Material(trackMarks.material);
            gameObject.AddComponent<MeshRenderer>().material = trackMarks.material;
            int num = 2 * trackMarks.maxSectorsPerTrack * 4;
            int num2 = 2 * trackMarks.maxSectorsPerTrack * 6;
            render.meshColors = new Color[num];
            render.meshNormals = new Vector3[num];
            render.meshPositions = new Vector3[num];
            render.meshUVs = new Vector2[num];
            render.meshTris = new int[num2];
            render.tracks = new TrackRenderData[2];

            for (int i = 0; i < 2; i++) {
                InitTrackRender(ref render.tracks[i],
                    trackMarks.maxSectorsPerTrack,
                    trackMarks.hideSectorsFrom,
                    trackMarks.baseAlpha,
                    trackMarks.parts);
            }
        }

        void InitTrackRender(ref TrackRenderData data, int sectorsPerTrack, int hideSectorsFrom, float baseAlpha,
            int parts) {
            data.maxSectors = sectorsPerTrack;
            data.firstSectorToHide = hideSectorsFrom;
            data.baseAlpha = baseAlpha;
            data.texturePart = 1f / parts;
            data.parts = parts;
            data.sectors = new TrackSector[sectorsPerTrack];
            ResetTrackRender(ref data);
        }

        void ResetTrackRender(ref TrackRenderData data) {
            data.position = -1;
            data.count = 0;
            data.currentPart = 0;
        }

        void AddSectorToRender(ref TrackRenderData data, Vector3 startPosition, Vector3 startForward, Vector3 endPosition,
            Vector3 endForward, Vector3 normal, float width, float textureWidth, float rotationCos, bool contiguous) {
            data.position++;
            data.position %= data.maxSectors;

            data.sectors[data.position] = new TrackSector {
                startPosition = startPosition,
                startForward = startForward,
                endPosition = endPosition,
                endForward = endForward,
                normal = normal,
                width = width,
                rotationCos = rotationCos,
                textureWidth = textureWidth,
                contiguous = contiguous
            };

            data.count++;

            if (data.count > data.maxSectors) {
                data.count = data.maxSectors;
            }

            data.currentPart++;
            data.currentPart %= data.parts;
        }

        void ComputeVerticesPosition(ref Vector3 position, ref Vector3 forward, ref Vector3 normal, float width,
            out Vector3 v0, out Vector3 v1) {
            float num = width / 2f;
            Vector3 normalized = Vector3.Cross(forward, normal).normalized;
            Vector3 vector = normalized * num;
            v0 = position + vector;
            v1 = position - vector;
        }

        void ComputeSectorVertices(TrackSector sector, int count, int indexByOrder, int indexInBufer, Vector3 prev0,
            Vector3 prev1, out float startTextureWidth, out float endTextureWidth, out Vector3 v0, out Vector3 v1,
            out Vector3 v2, out Vector3 v3) {
            ComputeVerticesPosition(ref sector.endPosition,
                ref sector.endForward,
                ref sector.normal,
                sector.width,
                out v0,
                out v1);

            float num = sector.width / 2f;
            Vector3 normalized = Vector3.Cross(sector.startForward, sector.normal).normalized;
            Vector3 vector = normalized * num;
            startTextureWidth = endTextureWidth = sector.textureWidth;

            if (sector.contiguous && indexByOrder < count - 1) {
                v2 = prev0;
                v3 = prev1;
            } else {
                ComputeVerticesPosition(ref sector.startPosition,
                    ref sector.startForward,
                    ref sector.normal,
                    sector.width,
                    out v2,
                    out v3);
            }
        }

        Vector2 RotateCoord(ref Vector2 point, ref Vector2 center, float c, float s) {
            Vector2 result = default;
            result.x = (point.x - center.x) * c - (point.y - center.y) * s + center.x;
            result.y = (point.x - center.x) * s + (point.y - center.y) * c + center.y;
            return result;
        }

        void RotateTextureCoords(ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, ref Vector2 uv4, out Vector2 r1,
            out Vector2 r2, out Vector2 r3, out Vector2 r4, float c, float s) {
            Vector2 center = (uv1 + uv2 + uv3 + uv3) / 4f;
            r1 = RotateCoord(ref uv1, ref center, c, s);
            r2 = RotateCoord(ref uv2, ref center, c, s);
            r3 = RotateCoord(ref uv3, ref center, c, s);
            r4 = RotateCoord(ref uv4, ref center, c, s);
        }

        void CopySectorToMesh(TrackRenderData data, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal,
            int indexByOrder, Vector3[] positions, Vector2[] uvs, Vector3[] normals, Color[] colors, int[] triangles,
            int firstVertex, int firstIndex, float halfAlpha, float startTextureWidth, float endTextureWidth,
            float rotationCos) {
            positions[firstVertex] = v0;
            positions[firstVertex + 1] = v1;
            positions[firstVertex + 2] = v2;
            positions[firstVertex + 3] = v3;
            float num = (indexByOrder - data.currentPart % data.parts) * data.texturePart;
            float y = num - data.texturePart;
            float f = Mathf.Acos(rotationCos);
            float s = Mathf.Sin(f);
            Vector2 uv = new(0f, y);
            Vector2 uv2 = new(startTextureWidth, y);
            Vector2 uv3 = new(0f, num);
            Vector2 uv4 = new(endTextureWidth, num);

            RotateTextureCoords(ref uv,
                ref uv2,
                ref uv3,
                ref uv4,
                out uvs[firstVertex],
                out uvs[firstVertex + 1],
                out uvs[firstVertex + 2],
                out uvs[firstVertex + 3],
                rotationCos,
                s);

            normals[firstVertex] = normal;
            normals[firstVertex + 1] = normal;
            normals[firstVertex + 2] = normal;
            normals[firstVertex + 3] = normal;
            float num2 = data.baseAlpha;
            float num3 = data.baseAlpha;

            if (indexByOrder > data.firstSectorToHide) {
                int num4 = data.maxSectors - data.firstSectorToHide;
                int num5 = indexByOrder - data.firstSectorToHide;
                float num6 = 1f - num5 / (float)num4;
                num2 *= num6 + halfAlpha;
                num3 *= num6 - halfAlpha;
            }

            Color color = Color.white * num2;
            Color color2 = Color.white * num3;
            colors[firstVertex] = color;
            colors[firstVertex + 1] = color;
            colors[firstVertex + 2] = color2;
            colors[firstVertex + 3] = color2;
            triangles[firstIndex] = firstVertex;
            triangles[firstIndex + 1] = firstVertex + 1;
            triangles[firstIndex + 2] = firstVertex + 2;
            triangles[firstIndex + 3] = firstVertex + 3;
            triangles[firstIndex + 4] = firstVertex + 2;
            triangles[firstIndex + 5] = firstVertex + 1;
        }

        void CopyToMesh(TrackRenderData data, Vector3[] positions, Vector2[] uvs, Vector3[] normals, Color[] colors,
            int[] triangles, int firstVertex, int firstIndex, out int nextIndex, out int nextVertex) {
            int num = data.position - data.count;

            if (num < 0) {
                num += data.maxSectors;
            }

            float halfAlpha = 0.5f / (data.maxSectors - data.firstSectorToHide);
            Vector3 prev = default;
            Vector3 prev2 = default;

            for (int num2 = data.count; num2 > 0; num2--) {
                TrackSector sector = data.sectors[num];
                float startTextureWidth;
                float endTextureWidth;
                Vector3 v;
                Vector3 v2;
                Vector3 v3;
                Vector3 v4;

                ComputeSectorVertices(sector,
                    data.count,
                    num2,
                    num,
                    prev,
                    prev2,
                    out startTextureWidth,
                    out endTextureWidth,
                    out v,
                    out v2,
                    out v3,
                    out v4);

                CopySectorToMesh(data,
                    v,
                    v2,
                    v3,
                    v4,
                    sector.normal,
                    num2,
                    positions,
                    uvs,
                    normals,
                    colors,
                    triangles,
                    firstVertex,
                    firstIndex,
                    halfAlpha,
                    startTextureWidth,
                    endTextureWidth,
                    sector.rotationCos);

                firstVertex += 4;
                firstIndex += 6;
                num++;
                num %= data.maxSectors;
                prev = v;
                prev2 = v2;
            }

            nextIndex = firstIndex;
            nextVertex = firstVertex;
        }

        void ResetRender(TrackMarksRenderComponent data) {
            for (int i = 0; i < data.tracks.Length; i++) {
                ResetTrackRender(ref data.tracks[i]);
            }
        }

        void UpdateMesh(TrackMarksRenderComponent data) {
            int nextVertex = 0;
            int nextIndex = 0;

            for (int i = 0; i < 2; i++) {
                CopyToMesh(data.tracks[i],
                    data.meshPositions,
                    data.meshUVs,
                    data.meshNormals,
                    data.meshColors,
                    data.meshTris,
                    nextVertex,
                    nextIndex,
                    out nextIndex,
                    out nextVertex);
            }

            data.mesh.vertices = data.meshPositions;
            data.mesh.uv = data.meshUVs;
            data.mesh.normals = data.meshNormals;
            data.mesh.triangles = data.meshTris;
            data.mesh.colors = data.meshColors;
            data.mesh.RecalculateBounds();
        }

        void MakeTracks(TrackMarksRenderComponent data, int track, Vector3 startPosition, Vector3 startForward,
            Vector3 endPosition, Vector3 endForward, Vector3 normal, float width, float textureWidth, float rotationCos,
            bool contiguous) {
            AddSectorToRender(ref data.tracks[track],
                startPosition,
                startForward,
                endPosition,
                endForward,
                normal,
                width,
                textureWidth,
                rotationCos,
                contiguous);

            data.dirty = true;
        }

        void ClearRender(TrackMarksRenderComponent data, TrackMarksComponent trackMarks) {
            for (int i = 0; i < 2; i++) {
                ResetTrackRender(ref data.tracks[i]);
            }

            for (int j = 0; j < 2 * trackMarks.maxSectorsPerTrack * 4; j++) {
                data.meshPositions[j] = Vector3.zero;
                data.meshUVs[j] = Vector2.zero;
                data.meshNormals[j] = Vector3.zero;
                data.meshColors[j] = Color.white;
            }

            for (int k = 0; k < 2 * trackMarks.maxSectorsPerTrack * 6; k++) {
                data.meshTris[k] = 0;
            }
        }

        void InitBuilder(TrackMarksBuilderComponent builder, TrackMarksComponent component,
            TrackMarksRenderComponent renderer, ChassisAnimationComponent chassis) {
            AllocateBuilder(builder, component, renderer);
            FindWheels(builder, chassis, component);
        }

        static void AllocateBuilder(TrackMarksBuilderComponent builder, TrackMarksComponent component,
            TrackMarksRenderComponent renderer) {
            builder.leftWheels = new Transform[3];
            builder.rightWheels = new Transform[3];
            builder.prevLeftWheelsData = new WheelData[3];
            builder.prevRightWheelsData = new WheelData[3];
            builder.tempLeftWheelsData = new WheelData[3];
            builder.tempRightWheelsData = new WheelData[3];
            builder.currentLeftWheelsData = new WheelData[3];
            builder.currentRightWheelsData = new WheelData[3];
            builder.positions = new Vector3[2];
            builder.nextPositions = new Vector3[2];
            builder.normals = new Vector3[2];
            builder.nextNormals = new Vector3[2];
            builder.directions = new Vector3[2];
            builder.contiguous = new bool[2];
            builder.prevHits = new bool[2];
            builder.remaingDistance = new float[2];
            builder.resetWheels = new bool[2];
            builder.side = new float[2] { -1f, 1f };

            for (int i = 0; i < 2; i++) {
                builder.contiguous[i] = false;
                builder.prevHits[i] = false;
            }

            builder.moveStep = component.markWidth / component.parts;
        }

        List<Transform> FindAllNodes(Transform root, string pattern, List<Transform> list = null) {
            if (list == null) {
                list = new List<Transform>();
            }

            for (int i = 0; i < root.childCount; i++) {
                Transform child = root.GetChild(i);

                if (child.name.Contains(pattern)) {
                    list.Add(child);
                }

                list = FindAllNodes(child, pattern, list);
            }

            return list;
        }

        void FindWheels(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis,
            TrackMarksComponent trackMarks) {
            List<Transform> list = FindAllNodes(trackMarks.transform, chassis.movingWheelName);
            List<Transform> list2 = new();
            List<Transform> list3 = new();

            for (int i = 0; i < list.Count; i++) {
                Transform transform = list[i];

                if (transform.name.Contains("left_")) {
                    list2.Add(transform);
                }

                if (transform.name.Contains("right_")) {
                    list3.Add(transform);
                }
            }

            ZSorter comparer = new();
            list2.Sort(comparer);
            list3.Sort(comparer);
            builder.leftWheels = list2.ToArray();
            builder.rightWheels = list3.ToArray();
        }

        void CopyWheelDataFromTransforms(Transform src, ref WheelData dst) {
            dst.position = src.position;
            dst.right = src.right;
        }

        void CopyWheelsPositionFromTransforms(Transform[] wheels, WheelData[] data) {
            CopyWheelDataFromTransforms(wheels[0], ref data[0]);
            CopyWheelDataFromTransforms(wheels[wheels.Length / 2], ref data[1]);
            CopyWheelDataFromTransforms(wheels[wheels.Length - 1], ref data[2]);
        }

        void ResetTrack(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i, ref RaycastHit hitData) {
            builder.directions[i] = trackMarks.transform.forward;
            builder.nextPositions[i] = builder.positions[i] = hitData.point;
            builder.nextNormals[i] = builder.normals[i] = hitData.normal;
            builder.contiguous[i] = false;
            builder.remaingDistance[i] = 0f;
        }

        Vector3 GetVelocity(TrackMarksBuilderComponent builder) => builder.rigidbody.velocity;

        bool CheckDirectionChange(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i,
            ref RaycastHit hitData) {
            float f = Vector3.Dot(GetVelocity(builder).normalized, builder.directions[i]);

            if (Mathf.Abs(f) >= 0.75f) {
                return true;
            }

            ResetTrack(builder, trackMarks, i, ref hitData);
            return false;
        }

        bool CheckNormalChange(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i,
            ref RaycastHit hitData) {
            float num = Vector3.Dot(builder.normals[i], hitData.normal);

            if (num >= 0.95f) {
                return true;
            }

            ResetTrack(builder, trackMarks, i, ref hitData);
            return false;
        }

        bool CheckEnoughMove(TrackMarksBuilderComponent builder, int i, ref RaycastHit hit) {
            float magnitude = (builder.nextPositions[i] - builder.positions[i]).magnitude;
            return magnitude >= builder.moveStep;
        }

        void UpdateTrack(TrackMarksBuilderComponent builder, TrackMarksRenderComponent render,
            TrackMarksComponent trackMarks, int track, bool hit, ref RaycastHit hitData, float width, float shiftToBack) {
            if (hit) {
                if (!builder.prevHits[track]) {
                    ResetTrack(builder, trackMarks, track, ref hitData);
                } else {
                    Vector3 normalized = GetVelocity(builder).normalized;

                    builder.nextPositions[track] = hitData.point -
                                                   trackMarks.transform.forward * shiftToBack * trackMarks.markWidth / 2f +
                                                   normalized * shiftToBack * trackMarks.markWidth;

                    builder.nextNormals[track] = hitData.normal;

                    if (CheckEnoughMove(builder, track, ref hitData) &&
                        CheckDirectionChange(builder, trackMarks, track, ref hitData) &&
                        CheckNormalChange(builder, trackMarks, track, ref hitData)) {
                        float num = Vector3.Dot(trackMarks.transform.forward, normalized);

                        if (num > 0.95f) {
                            num = 1f;
                        } else if (num < 0.8f) {
                            num = 0f;
                        }

                        MakeTracks(render,
                            track,
                            builder.positions[track],
                            builder.directions[track],
                            builder.nextPositions[track],
                            normalized,
                            hitData.normal,
                            width,
                            width / trackMarks.markWidth,
                            num,
                            builder.contiguous[track]);

                        builder.directions[track] = normalized;
                        builder.positions[track] = builder.nextPositions[track];
                        builder.normals[track] = builder.nextNormals[track];
                        builder.contiguous[track] = true;
                    }
                }
            } else {
                ResetTrack(builder, trackMarks, track, ref hitData);
            }

            builder.prevHits[track] = hit;
        }

        bool CheckExtraContacts(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] wheels,
            float hitDistance) {
            float maxDistance = hitDistance + 0.2f;

            return Physics.Raycast(wheels[0].position, -trackMarks.transform.up, maxDistance, TRACK_LAYER_MASK) ||
                   !Physics.Raycast(wheels[wheels.Length - 1].position,
                       -trackMarks.transform.up,
                       maxDistance,
                       TRACK_LAYER_MASK);
        }

        bool IsHit(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] wheels, float hitDistance,
            out RaycastHit hit) {
            WheelData wheelData = wheels[wheels.Length / 2];

            bool flag = Physics.Raycast(wheelData.position,
                -trackMarks.transform.up,
                out hit,
                hitDistance * 2f,
                TRACK_LAYER_MASK);

            if (flag) {
                if (hit.distance >= hitDistance * 1.5f && !CheckExtraContacts(builder, trackMarks, wheels, hitDistance)) {
                    return false;
                }

                float maxDistance = hit.distance + 0.1f;
                RaycastHit hitInfo;

                flag &= Physics.Raycast(wheelData.position + wheelData.right * trackMarks.markTestShift,
                    -trackMarks.transform.up,
                    out hitInfo,
                    maxDistance,
                    TRACK_LAYER_MASK);

                if (flag) {
                    RaycastHit hitInfo2;

                    flag &= Physics.Raycast(wheelData.position - wheelData.right * trackMarks.markTestShift,
                        -trackMarks.transform.up,
                        out hitInfo2,
                        maxDistance,
                        TRACK_LAYER_MASK);

                    hit.normal = (hitInfo2.normal + hitInfo2.normal) / 2f;
                    hit.point = (hitInfo2.point + hitInfo.point) / 2f;
                }
            }

            return flag;
        }

        bool NeedUpdateMarks(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks) {
            float magnitude = (Camera.main.transform.position - trackMarks.transform.position).magnitude;
            return magnitude <= trackMarks.maxDistance;
        }

        void InterpolateWheelsPosition(float delta, WheelData[] prev, WheelData[] current, WheelData[] result) {
            for (int i = 0; i < prev.Length; i++) {
                result[i].position = Vector3.Lerp(prev[i].position, current[i].position, delta);
                result[i].right = Vector3.Lerp(prev[i].right, current[i].right, delta);
            }
        }

        Vector3 ComputeTrackDiagonal(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] data,
            ref Vector3 left) {
            Vector3 vector = data[0].position - data[0].right * trackMarks.markWidth / 2f;
            Vector3 vector2 = data[data.Length - 1].position + data[data.Length - 1].right * trackMarks.markWidth / 2f;
            return vector2 - vector;
        }

        void UpdateWheels(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis,
            TrackMarksRenderComponent render, TrackMarksComponent trackMarks, int track, WheelData[] currentWheelsPositions,
            WheelData[] prevWheelsPosition, WheelData[] temp) {
            float highDistance = chassis.highDistance;
            Transform transform = trackMarks.transform;
            WheelData wheelData = prevWheelsPosition[prevWheelsPosition.Length / 2];
            WheelData wheelData2 = currentWheelsPositions[currentWheelsPositions.Length / 2];
            float num = (wheelData.position - wheelData2.position).magnitude + builder.remaingDistance[track];
            int num2 = (int)(num / builder.moveStep);

            Vector3 left = Vector3.Cross(transform.up, GetVelocity(builder) * Time.smoothDeltaTime).normalized *
                           builder.side[track];

            Vector3 rhs = ComputeTrackDiagonal(builder, trackMarks, currentWheelsPositions, ref left);
            Vector3 rhs2 = ComputeTrackDiagonal(builder, trackMarks, prevWheelsPosition, ref left);
            float num3 = Mathf.Abs(Vector3.Dot(left, rhs));
            float num4 = Mathf.Abs(Vector3.Dot(left, rhs2));
            num3 = Mathf.Max(num3 / 2f, trackMarks.markWidth);
            num4 = Mathf.Max(num4 / 2f, trackMarks.markWidth);
            bool flag = false;
            RaycastHit hit = default;

            for (int i = 0; i < num2; i++) {
                float num5 = i / (float)num2;
                float num6 = Mathf.Lerp(num4, num3, num5);
                float num7 = Mathf.Min(num6 / trackMarks.markWidth, 2f);
                InterpolateWheelsPosition(num5, prevWheelsPosition, currentWheelsPositions, temp);
                flag = IsHit(builder, trackMarks, temp, highDistance, out hit);
                hit.point -= builder.side[track] * trackMarks.transform.right * trackMarks.shiftToCenter;
                UpdateTrack(builder, render, trackMarks, track, flag, ref hit, num6, num7 - 1f);
                num -= builder.moveStep;
            }

            builder.remaingDistance[track] = num;
        }

        void CheckResetWheels(TrackMarksBuilderComponent builder, int i, Transform[] wheels, WheelData[] result) {
            if (builder.resetWheels[i]) {
                CopyWheelsPositionFromTransforms(wheels, result);
                builder.resetWheels[i] = false;
            }
        }

        void UpdateSingleTrack(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis,
            TrackMarksRenderComponent render, TrackMarksComponent trackMarks, int track, Transform[] wheels,
            WheelData[] prevWheelsData, WheelData[] currentWheelsData, WheelData[] tempWheelsData) {
            CheckResetWheels(builder, track, wheels, prevWheelsData);
            CopyWheelsPositionFromTransforms(wheels, currentWheelsData);
            UpdateWheels(builder, chassis, render, trackMarks, track, currentWheelsData, prevWheelsData, tempWheelsData);
            Array.Copy(currentWheelsData, prevWheelsData, currentWheelsData.Length);
        }

        void ResetTracks(TrackMarksBuilderComponent builder) {
            for (int i = 0; i < 2; i++) {
                builder.prevHits[i] = false;
                builder.contiguous[i] = false;
                builder.resetWheels[i] = true;
            }
        }

        public void Clear(TrackMarksBuilderComponent builder, TrackMarksRenderComponent render,
            TrackMarksComponent trackMarks) {
            ResetTracks(builder);
            ClearRender(render, trackMarks);
        }

        [OnEventFire]
        public void StartFadeMaterialAlpha(NodeAddedEvent e, FadeTrackMarkNode trackNode) =>
            trackNode.trackMarks.baseAlpha *= trackNode.trackMarks.material.color.a;

        [OnEventFire]
        public void UpdateFadeMaterialAlpha(UpdateEvent e, FadeTrackMarkNode trackNode) =>
            trackNode.trackMarks.material.SetAlpha(trackNode.transparencyTransition.CurrentAlpha *
                                                   trackNode.trackMarks.baseAlpha);

        sealed class ZSorter : IComparer<Transform> {
            public int Compare(Transform lhs, Transform rhs) => (int)Mathf.Sign(rhs.position.z - lhs.position.z);
        }

        public class CameraNode : Node {
            public BattleCameraComponent battleCamera;

            public CameraComponent camera;
        }

        public class TrackMarksInitNode : Node {
            public AssembledTankComponent assembledTank;

            public ChassisAnimationComponent chassisAnimation;
            public TrackMarksComponent trackMarks;
        }

        public class TrackMarkUpdateNode : Node {
            public ChassisAnimationComponent chassisAnimation;
            public RigidbodyComponent rigidbody;

            public TrackMarksComponent trackMarks;

            public TrackMarksBuilderComponent trackMarksBuilder;

            public TrackMarksRenderComponent trackMarksRender;
        }

        public class FadeTrackMarkNode : Node {
            public TankDeadStateComponent tankDeadState;
            public TrackMarksComponent trackMarks;

            public TransparencyTransitionComponent transparencyTransition;
        }
    }
}