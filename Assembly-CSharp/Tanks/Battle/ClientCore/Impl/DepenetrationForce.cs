using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class DepenetrationForce {
        public static float ABSORTION_KOEF = 1f;

        public static float VISUAL_DELTA = 5f;

        public static float DEP_VELOCITY = 2f;

        public static float VERT_DEP_VELOCITY = 0f;

        static Vector3 forcePoint = Vector3.zero;

        static Vector3 forceDir = Vector3.zero;

        static Vector3 vertforceDir = Vector3.zero;

        static readonly List<Edge> edges1 = new();

        static readonly List<Edge> edges2 = new();

        static readonly List<Vector2> contacts = new();

        static float largestSectionSq;

        static Vector2 SectionFrom = Vector2.zero;

        static Vector2 SectionTo = Vector2.zero;

        public static bool ApplyDepenetrationForce(Rigidbody body1, BoxCollider collider1, Rigidbody body2, BoxCollider collider2) {
            if (CalculateForcePointAndDir(body1, collider1, body2, collider2)) {
                float num = body2.mass / body1.mass;
                Vector3 vector = Vector3.zero;
                Vector3 vector2 = Vector3.zero;
                Vector3 pointVelocity = body1.GetPointVelocity(forcePoint);
                Vector3 vector3 = Vector3.Project(-pointVelocity, forceDir.normalized);

                if (Vector3.Dot(vector3, forceDir) > 0f) {
                    vector = vector3 * 0.5f * ABSORTION_KOEF * num;
                    vector2 = -vector3 * 0.5f * ABSORTION_KOEF / num;
                }

                float magnitude = vector3.magnitude;
                float num2 = num * DEP_VELOCITY;

                if (magnitude < num2) {
                    vector += forceDir * (num2 - magnitude);
                }

                body1.AddForceAtPositionSafe(vector * body1.mass / Time.fixedDeltaTime, forcePoint);
                body2.AddForceAtPositionSafe(vector2 * body2.mass / Time.fixedDeltaTime, forcePoint);
                pointVelocity = body1.GetPointVelocity(forcePoint);
                vector3 = Vector3.Project(-pointVelocity, vertforceDir.normalized);

                if (Vector3.Dot(vector3, vertforceDir) > 0f) {
                    vector = vector3 * 0.5f * 0.1f;
                    body1.AddForceAtPositionSafe(vector * body1.mass / Time.fixedDeltaTime, forcePoint);
                } else {
                    magnitude = vector3.magnitude;

                    if (magnitude < VERT_DEP_VELOCITY) {
                        vector = vertforceDir * (VERT_DEP_VELOCITY - magnitude);
                        body1.AddForceAtPositionSafe(vector * body1.mass / Time.fixedDeltaTime, forcePoint);
                    }
                }

                return true;
            }

            return false;
        }

        static bool CalculateForcePointAndDir(Rigidbody body1, BoxCollider collider1, Rigidbody body2, BoxCollider collider2) {
            edges1.Clear();
            edges2.Clear();
            CollectBoxColliderEdges(collider1, edges1);
            CollectBoxColliderEdges(collider2, edges2);

            if (!FindSectionXZSpace()) {
                return false;
            }

            Vector3 vector = new(SectionFrom.x, VISUAL_DELTA, SectionFrom.y);
            Vector3 vector2 = new(SectionTo.x, VISUAL_DELTA, SectionTo.y);
            forceDir = Vector3.Cross((vector2 - vector).normalized, Vector3.up);
            forcePoint = (vector2 + vector) * 0.5f;

            if (!FindSectionXYSpace()) {
                return false;
            }

            if (!FindSectionZYSpace()) {
                return false;
            }

            Vector3 normalized = (forcePoint - body1.position).normalized;

            if (Vector3.Dot(normalized, forceDir) > 0f) {
                forceDir = -forceDir;
            }

            forcePoint.y = (SectionFrom.y + SectionTo.y) * 0.5f;
            normalized = (forcePoint - body1.position).normalized;
            vector = new Vector3(0f, SectionFrom.y, SectionFrom.x);
            vector2 = new Vector3(0f, SectionTo.y, SectionTo.x);
            vertforceDir = Vector3.Cross((vector2 - vector).normalized, Vector3.left);

            if (Vector3.Dot(normalized, vertforceDir) > 0f) {
                vertforceDir = -vertforceDir;
            }

            return true;
        }

        static bool FindSectionXZSpace() {
            Find2DSection();
            return largestSectionSq > 0f;
        }

        static bool FindSectionZYSpace() {
            foreach (Edge item in edges1) {
                item.ToZYSpace();
            }

            foreach (Edge item2 in edges2) {
                item2.ToZYSpace();
            }

            Find2DSection();
            return largestSectionSq > 0f;
        }

        static bool FindSectionXYSpace() {
            foreach (Edge item in edges1) {
                item.ToXYSpace();
            }

            foreach (Edge item2 in edges2) {
                item2.ToXYSpace();
            }

            Find2DSection();
            return largestSectionSq > 0f;
        }

        static void Find2DSection() {
            contacts.Clear();

            foreach (Edge item in edges1) {
                foreach (Edge item2 in edges2) {
                    Vector2 intersection;

                    if (LineSegementsIntersect(item.from2D, item.to2D, item2.from2D, item2.to2D, out intersection)) {
                        contacts.Add(intersection);
                        float num = 0.1f;
                    }
                }
            }

            largestSectionSq = 0f;
            SectionFrom = Vector2.zero;
            SectionTo = Vector2.zero;

            foreach (Vector2 contact in contacts) {
                foreach (Vector2 contact2 in contacts) {
                    if (!(contact == contact2)) {
                        float sqrMagnitude = (contact - contact2).sqrMagnitude;

                        if (sqrMagnitude > largestSectionSq) {
                            largestSectionSq = sqrMagnitude;
                            SectionFrom = contact;
                            SectionTo = contact2;
                        }
                    }
                }
            }
        }

        public static void CollectBoxColliderEdges(BoxCollider collider, List<Edge> edges) {
            Vector3 vector = collider.size * 0.5f * 0.9f;
            vector.y *= 0.7f;
            Vector3 vector2 = collider.center - vector;
            Vector3 vector3 = collider.center + vector;
            Transform transform = collider.transform;
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector3.y, vector2.z), TransfromPoint(transform, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector3.y, vector2.z), TransfromPoint(transform, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector3.y, vector3.z), TransfromPoint(transform, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector3.y, vector3.z), TransfromPoint(transform, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector2.y, vector2.z), TransfromPoint(transform, vector2.x, vector2.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector2.y, vector2.z), TransfromPoint(transform, vector3.x, vector2.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector2.y, vector3.z), TransfromPoint(transform, vector3.x, vector2.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector2.y, vector3.z), TransfromPoint(transform, vector2.x, vector2.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector2.y, vector2.z), TransfromPoint(transform, vector2.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector2.x, vector2.y, vector3.z), TransfromPoint(transform, vector2.x, vector3.y, vector3.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector2.y, vector2.z), TransfromPoint(transform, vector3.x, vector3.y, vector2.z)));
            edges.Add(new Edge(TransfromPoint(transform, vector3.x, vector2.y, vector3.z), TransfromPoint(transform, vector3.x, vector3.y, vector3.z)));
        }

        public static Vector3 TransfromPoint(Transform t, float x, float y, float z) => t.TransformPoint(new Vector3(x, y, z));

        public static float Cross2D(Vector2 v1, Vector2 v2) => v1.x * v2.y - v1.y * v2.x;

        public static bool LineSegementsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false) {
            intersection = default;
            Vector2 vector = p2 - p;
            Vector2 vector2 = q2 - q;
            float num = Cross2D(vector, vector2);
            float a = Cross2D(q - p, vector);

            if (Mathf.Approximately(num, 0f) && Mathf.Approximately(a, 0f)) {
                if (considerCollinearOverlapAsIntersect && (0f <= Vector2.Dot(q - p, vector) && Vector2.Dot(q - p, vector) <= Vector2.Dot(vector, vector) ||
                                                            0f <= Vector2.Dot(p - q, vector2) && Vector2.Dot(p - q, vector2) <= Vector2.Dot(vector2, vector2))) {
                    return true;
                }

                return false;
            }

            if (Mathf.Approximately(num, 0f) && !Mathf.Approximately(a, 0f)) {
                return false;
            }

            float num2 = Cross2D(q - p, vector2) / num;
            float num3 = Cross2D(q - p, vector) / num;

            if (!Mathf.Approximately(num, 0f) && 0f <= num2 && num2 <= 1f && 0f <= num3 && num3 <= 1f) {
                intersection = p + num2 * vector;
                return true;
            }

            return false;
        }

        public class Edge {
            public Vector2 from2D;
            public Vector3 from3D;

            public Vector2 to2D;

            public Vector3 to3D;

            public Edge(Vector3 from, Vector3 to) {
                from3D = from;
                to3D = to;
                ToXZSpace();
            }

            public void ToXZSpace() {
                from2D = new Vector2(from3D.x, from3D.z);
                to2D = new Vector2(to3D.x, to3D.z);
            }

            public void ToZYSpace() {
                from2D = new Vector2(from3D.z, from3D.y);
                to2D = new Vector2(to3D.z, to3D.y);
            }

            public void ToXYSpace() {
                from2D = new Vector2(from3D.x, from3D.y);
                to2D = new Vector2(to3D.x, to3D.y);
            }
        }
    }
}