using UnityEngine;

namespace Edelweiss.DecalSystem {
    class GeometryUtilities {
        const float c_Epsilon = 1E-10f;

        public static bool IsQuadrangleConvex(Vector3 a_Vertex1, Vector3 a_Vertex2, Vector3 a_Vertex3, Vector3 a_Vertex4) {
            Vector3 a_IntersectionPoint;
            return LineIntersection(a_Vertex1, a_Vertex3, a_Vertex2, a_Vertex4, out a_IntersectionPoint);
        }

        public static bool LineIntersection(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start,
            Vector3 a_Line2End, out Vector3 a_IntersectionPoint) {
            bool a_IsUnique;

            return LineIntersection(a_Line1Start,
                a_Line1End,
                a_Line2Start,
                a_Line2End,
                out a_IntersectionPoint,
                out a_IsUnique);
        }

        public static bool LineIntersection(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start,
            Vector3 a_Line2End, out Vector3 a_IntersectionPoint, out bool a_IsUnique) {
            bool result = false;
            a_IntersectionPoint = Vector3.zero;
            a_IsUnique = false;
            Vector3 vector = a_Line1End - a_Line1Start;
            Vector3 vector2 = a_Line2End - a_Line2Start;
            Vector3 lhs = a_Line2Start - a_Line1Start;
            Vector3 rhs = Vector3.Cross(vector, vector2);
            float a = Vector3.Dot(lhs, rhs);

            if (Mathf.Approximately(a, 0f)) {
                float sqrMagnitude = rhs.sqrMagnitude;

                if (Mathf.Approximately(sqrMagnitude, 0f)) {
                    if (Vector3Extension.Approximately(vector, Vector3.zero)) {
                        if (IsPointOnLine(a_Line1Start, a_Line2Start, a_Line2End)) {
                            result = true;
                            a_IsUnique = true;
                            a_IntersectionPoint = a_Line1Start;
                        }
                    } else if (Vector3Extension.Approximately(vector2, Vector3.zero)) {
                        if (IsPointOnLine(a_Line2Start, a_Line1Start, a_Line1End)) {
                            result = true;
                            a_IsUnique = true;
                            a_IntersectionPoint = a_Line2Start;
                        }
                    } else {
                        float num = FactorOfPointOnLine(a_Line2Start, a_Line1Start, a_Line1End);
                        float num2 = FactorOfPointOnLine(a_Line2End, a_Line1Start, a_Line1End);

                        if (0f <= num && num <= 1f) {
                            result = true;
                            a_IntersectionPoint = a_Line2Start;
                        } else if (0f <= num2 && num2 <= 1f) {
                            result = true;
                            a_IntersectionPoint = a_Line2End;
                        } else if (num < 0f && num2 > 1f || num2 < 0f && num > 1f) {
                            result = true;
                            a_IntersectionPoint = a_Line1Start;
                        }
                    }
                } else {
                    float num3 = Vector3.Dot(Vector3.Cross(lhs, vector2), rhs) / sqrMagnitude;

                    if (0f <= num3 && num3 <= 1f) {
                        result = true;
                        a_IntersectionPoint = a_Line1Start + num3 * vector;
                        a_IsUnique = true;
                    }
                }
            }

            return result;
        }

        static float FactorOfPointOnLine(Vector3 a_Point, Vector3 a_LineStart, Vector3 a_LineEnd) {
            float num = 0f;
            Vector3 vector = a_LineEnd - a_LineStart;
            Vector3 vector2 = vector;
            vector2.x = Mathf.Abs(vector2.x);
            vector2.y = Mathf.Abs(vector2.y);
            vector2.z = Mathf.Abs(vector2.z);
            float num2;
            float num3;
            float num4;

            if (vector2.x > vector2.y && vector2.x > vector2.z) {
                num2 = a_Point.x;
                num3 = a_LineStart.x;
                num4 = vector.x;
            } else if (vector2.y > vector2.x && vector2.y > vector2.z) {
                num2 = a_Point.y;
                num3 = a_LineStart.y;
                num4 = vector.y;
            } else {
                num2 = a_Point.z;
                num3 = a_LineStart.z;
                num4 = vector.z;
            }

            return (num2 - num3) / num4;
        }

        public static bool IsPointOnLine(Vector3 a_Point, Vector3 a_LineStart, Vector3 a_LineEnd) {
            bool result = false;
            Vector3 lhs = a_Point - a_LineStart;
            Vector3 rhs = a_Point - a_LineEnd;
            float sqrMagnitude = Vector3.Cross(lhs, rhs).sqrMagnitude;

            if (sqrMagnitude < 1E-10f) {
                float num = Vector3.Dot(lhs, rhs);

                if (num <= 0f) {
                    float sqrMagnitude2 = (a_LineEnd - a_LineStart).sqrMagnitude;

                    if (num <= sqrMagnitude2) {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static Vector3 TriangleNormal(Vector3 a_Vertex1, Vector3 a_Vertex2, Vector3 a_Vertex3) {
            Vector3 lhs = a_Vertex2 - a_Vertex1;
            Vector3 rhs = a_Vertex3 - a_Vertex2;
            lhs.Normalize();
            rhs.Normalize();
            Vector3 result = Vector3.Cross(lhs, rhs);
            result.Normalize();
            return result;
        }

        public static bool AreLinesParallel(Vector3 a_Line1Start, Vector3 a_Line1End, Vector3 a_Line2Start,
            Vector3 a_Line2End) {
            bool result = false;
            Vector3 lhs = a_Line1End - a_Line1Start;
            Vector3 rhs = a_Line2End - a_Line2Start;
            lhs.Normalize();
            rhs.Normalize();
            float f = Vector3.Dot(lhs, rhs);
            f = Mathf.Abs(f);

            if (Mathf.Approximately(f, 0f)) {
                result = true;
            }

            return result;
        }
    }
}