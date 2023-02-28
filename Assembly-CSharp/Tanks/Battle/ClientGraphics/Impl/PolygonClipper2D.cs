using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class PolygonClipper2D {
        public List<ClipPointData> inputList = new(10);

        public List<ClipPointData> GetIntersectedPolygon(List<ClipPointData> polygonPoints, ClipEdge2D[] clipEdges) {
            List<ClipPointData> list = polygonPoints.ToList();

            foreach (ClipEdge2D clipEdge2D in clipEdges) {
                inputList.Clear();

                for (int j = 0; j < list.Count; j++) {
                    inputList.Add(list[j]);
                }

                list.Clear();

                if (inputList.Count == 0) {
                    break;
                }

                ClipPointData fromPoint = inputList[inputList.Count - 1];
                ClipPointData clipPointData;

                for (int k = 0; k < inputList.Count; fromPoint = clipPointData, k++) {
                    clipPointData = inputList[k];

                    if (IsInside(clipEdge2D, clipPointData.point2D)) {
                        if (!IsInside(clipEdge2D, fromPoint.point2D)) {
                            ClipPointData resultPoint;

                            if (!GetIntersect(fromPoint, clipPointData, clipEdge2D.from, clipEdge2D.to, out resultPoint)) {
                                goto IL_0161;
                            }

                            list.Add(resultPoint);
                        }

                        list.Add(clipPointData);
                        continue;
                    }

                    if (!IsInside(clipEdge2D, fromPoint.point2D)) {
                        continue;
                    }

                    ClipPointData resultPoint2;

                    if (GetIntersect(fromPoint, clipPointData, clipEdge2D.from, clipEdge2D.to, out resultPoint2)) {
                        list.Add(resultPoint2);
                        continue;
                    }

                    IL_0161:
                    list.Clear();
                    return list;
                }
            }

            return list;
        }

        bool GetIntersect(ClipPointData fromPoint, ClipPointData toPoint, Vector2 clipEdgeFrom, Vector2 clipEdgeTo, out ClipPointData resultPoint) {
            Vector2 vector = toPoint.point2D - fromPoint.point2D;
            Vector2 vector2 = clipEdgeTo - clipEdgeFrom;
            float num = vector.x * vector2.y - vector.y * vector2.x;

            if (Mathf.Approximately(num, 0f)) {
                resultPoint = default;
                return false;
            }

            Vector2 vector3 = clipEdgeFrom - fromPoint.point2D;
            float lerpFactor = (vector3.x * vector2.y - vector3.y * vector2.x) / num;
            resultPoint = ClipPointData.Lerp(fromPoint, toPoint, lerpFactor);
            return true;
        }

        bool IsInside(ClipEdge2D edge, Vector2 test) => !new bool?(IsLeftOf(edge, test)).Value;

        bool IsLeftOf(ClipEdge2D edge, Vector2 test) {
            Vector2 vector = edge.to - edge.from;
            Vector2 vector2 = test - edge.to;
            double num = vector.x * vector2.y - vector.y * vector2.x;

            if (num < 0.0) {
                return false;
            }

            if (num > 0.0) {
                return true;
            }

            return true;
        }
    }
}