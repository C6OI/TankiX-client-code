using System;
using System.Collections.Generic;
using System.Linq;

namespace MIConvexHull {
    class ConvexHullInternal {
        const int ConnectorTableSize = 2017;

        internal readonly int Dimension;

        readonly bool IsLifted;

        readonly double PlaneDistanceTolerance;

        readonly IVertex[] Vertices;

        IndexBuffer AffectedFaceBuffer;

        internal bool[] AffectedFaceFlags;

        IndexBuffer BeyondBuffer;

        double[] Center;

        SimpleList<DeferredFace> ConeFaceBuffer;

        ConnectorList[] ConnectorTable;

        IndexBuffer ConvexFaces;

        int ConvexHullSize;

        int CurrentVertex;

        IndexBuffer EmptyBuffer;

        internal ConvexFaceInternal[] FacePool;

        int FurthestVertex;

        MathHelper MathHelper;

        double MaxDistance;

        ObjectManager ObjectManager;

        double[] Positions;

        HashSet<int> SingularVertices;

        IndexBuffer TraverseStack;

        FaceList UnprocessedFaces;

        int[] UpdateBuffer;

        int[] UpdateIndices;

        bool[] VertexMarks;

        ConvexHullInternal(IVertex[] vertices, bool lift, ConvexHullComputationConfig config) {
            if (config.PointTranslationType != 0 && config.PointTranslationGenerator == null) {
                throw new InvalidOperationException(
                    "PointTranslationGenerator cannot be null if PointTranslationType is enabled.");
            }

            IsLifted = lift;
            Vertices = vertices;
            PlaneDistanceTolerance = config.PlaneDistanceTolerance;
            Dimension = DetermineDimension();

            if (Dimension < 2) {
                throw new InvalidOperationException("Dimension of the input must be 2 or greater.");
            }

            if (lift) {
                Dimension++;
            }

            InitializeData(config);
        }

        void TagAffectedFaces(ConvexFaceInternal currentFace) {
            AffectedFaceBuffer.Clear();
            AffectedFaceBuffer.Add(currentFace.Index);
            TraverseAffectedFaces(currentFace.Index);
        }

        void TraverseAffectedFaces(int currentFace) {
            TraverseStack.Clear();
            TraverseStack.Push(currentFace);
            AffectedFaceFlags[currentFace] = true;

            while (TraverseStack.Count > 0) {
                ConvexFaceInternal convexFaceInternal = FacePool[TraverseStack.Pop()];

                for (int i = 0; i < Dimension; i++) {
                    int num = convexFaceInternal.AdjacentFaces[i];

                    if (!AffectedFaceFlags[num] &&
                        MathHelper.GetVertexDistance(CurrentVertex, FacePool[num]) >= PlaneDistanceTolerance) {
                        AffectedFaceBuffer.Add(num);
                        AffectedFaceFlags[num] = true;
                        TraverseStack.Push(num);
                    }
                }
            }
        }

        DeferredFace MakeDeferredFace(ConvexFaceInternal face, int faceIndex, ConvexFaceInternal pivot, int pivotIndex,
            ConvexFaceInternal oldFace) {
            DeferredFace deferredFace = ObjectManager.GetDeferredFace();
            deferredFace.Face = face;
            deferredFace.FaceIndex = faceIndex;
            deferredFace.Pivot = pivot;
            deferredFace.PivotIndex = pivotIndex;
            deferredFace.OldFace = oldFace;
            return deferredFace;
        }

        void ConnectFace(FaceConnector connector) {
            uint num = connector.HashCode % 2017;
            ConnectorList connectorList = ConnectorTable[num];

            for (FaceConnector faceConnector = connectorList.First;
                 faceConnector != null;
                 faceConnector = faceConnector.Next) {
                if (FaceConnector.AreConnectable(connector, faceConnector, Dimension)) {
                    connectorList.Remove(faceConnector);
                    FaceConnector.Connect(faceConnector, connector);
                    faceConnector.Face = null;
                    connector.Face = null;
                    ObjectManager.DepositConnector(faceConnector);
                    ObjectManager.DepositConnector(connector);
                    return;
                }
            }

            connectorList.Add(connector);
        }

        bool CreateCone() {
            int currentVertex = CurrentVertex;
            ConeFaceBuffer.Clear();

            for (int i = 0; i < AffectedFaceBuffer.Count; i++) {
                int num = AffectedFaceBuffer[i];
                ConvexFaceInternal convexFaceInternal = FacePool[num];
                int num2 = 0;

                for (int j = 0; j < Dimension; j++) {
                    int num3 = convexFaceInternal.AdjacentFaces[j];

                    if (!AffectedFaceFlags[num3]) {
                        UpdateBuffer[num2] = num3;
                        UpdateIndices[num2] = j;
                        num2++;
                    }
                }

                for (int k = 0; k < num2; k++) {
                    ConvexFaceInternal convexFaceInternal2 = FacePool[UpdateBuffer[k]];
                    int pivotIndex = 0;
                    int[] adjacentFaces = convexFaceInternal2.AdjacentFaces;

                    for (int l = 0; l < adjacentFaces.Length; l++) {
                        if (num == adjacentFaces[l]) {
                            pivotIndex = l;
                            break;
                        }
                    }

                    int num4 = UpdateIndices[k];
                    int face = ObjectManager.GetFace();
                    ConvexFaceInternal convexFaceInternal3 = FacePool[face];
                    int[] vertices = convexFaceInternal3.Vertices;

                    for (int m = 0; m < Dimension; m++) {
                        vertices[m] = convexFaceInternal.Vertices[m];
                    }

                    int num5 = vertices[num4];
                    int num6;

                    if (currentVertex < num5) {
                        num6 = 0;
                        int num7 = num4 - 1;

                        while (num7 >= 0) {
                            if (vertices[num7] > currentVertex) {
                                vertices[num7 + 1] = vertices[num7];
                                num7--;
                                continue;
                            }

                            num6 = num7 + 1;
                            break;
                        }
                    } else {
                        num6 = Dimension - 1;

                        for (int n = num4 + 1; n < Dimension; n++) {
                            if (vertices[n] < currentVertex) {
                                vertices[n - 1] = vertices[n];
                                continue;
                            }

                            num6 = n - 1;
                            break;
                        }
                    }

                    vertices[num6] = CurrentVertex;

                    if (!MathHelper.CalculateFacePlane(convexFaceInternal3, Center)) {
                        return false;
                    }

                    ConeFaceBuffer.Add(MakeDeferredFace(convexFaceInternal3,
                        num6,
                        convexFaceInternal2,
                        pivotIndex,
                        convexFaceInternal));
                }
            }

            return true;
        }

        void CommitCone() {
            for (int i = 0; i < ConeFaceBuffer.Count; i++) {
                DeferredFace deferredFace = ConeFaceBuffer[i];
                ConvexFaceInternal face = deferredFace.Face;
                ConvexFaceInternal pivot = deferredFace.Pivot;
                ConvexFaceInternal oldFace = deferredFace.OldFace;
                int faceIndex = deferredFace.FaceIndex;
                face.AdjacentFaces[faceIndex] = pivot.Index;
                pivot.AdjacentFaces[deferredFace.PivotIndex] = face.Index;

                for (int j = 0; j < Dimension; j++) {
                    if (j != faceIndex) {
                        FaceConnector connector = ObjectManager.GetConnector();
                        connector.Update(face, j, Dimension);
                        ConnectFace(connector);
                    }
                }

                if (pivot.VerticesBeyond.Count == 0) {
                    FindBeyondVertices(face, oldFace.VerticesBeyond);
                } else if (pivot.VerticesBeyond.Count < oldFace.VerticesBeyond.Count) {
                    FindBeyondVertices(face, pivot.VerticesBeyond, oldFace.VerticesBeyond);
                } else {
                    FindBeyondVertices(face, oldFace.VerticesBeyond, pivot.VerticesBeyond);
                }

                if (face.VerticesBeyond.Count == 0) {
                    ConvexFaces.Add(face.Index);
                    UnprocessedFaces.Remove(face);
                    ObjectManager.DepositVertexBuffer(face.VerticesBeyond);
                    face.VerticesBeyond = EmptyBuffer;
                } else {
                    UnprocessedFaces.Add(face);
                }

                ObjectManager.DepositDeferredFace(deferredFace);
            }

            for (int k = 0; k < AffectedFaceBuffer.Count; k++) {
                int num = AffectedFaceBuffer[k];
                UnprocessedFaces.Remove(FacePool[num]);
                ObjectManager.DepositFace(num);
            }
        }

        void IsBeyond(ConvexFaceInternal face, IndexBuffer beyondVertices, int v) {
            double vertexDistance = MathHelper.GetVertexDistance(v, face);

            if (!(vertexDistance >= PlaneDistanceTolerance)) {
                return;
            }

            if (vertexDistance > MaxDistance) {
                if (vertexDistance - MaxDistance < PlaneDistanceTolerance) {
                    if (LexCompare(v, FurthestVertex) > 0) {
                        MaxDistance = vertexDistance;
                        FurthestVertex = v;
                    }
                } else {
                    MaxDistance = vertexDistance;
                    FurthestVertex = v;
                }
            }

            beyondVertices.Add(v);
        }

        void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond, IndexBuffer beyond1) {
            IndexBuffer beyondBuffer = BeyondBuffer;
            MaxDistance = double.NegativeInfinity;
            FurthestVertex = 0;

            for (int i = 0; i < beyond1.Count; i++) {
                VertexMarks[beyond1[i]] = true;
            }

            VertexMarks[CurrentVertex] = false;

            for (int j = 0; j < beyond.Count; j++) {
                int num = beyond[j];

                if (num != CurrentVertex) {
                    VertexMarks[num] = false;
                    IsBeyond(face, beyondBuffer, num);
                }
            }

            for (int k = 0; k < beyond1.Count; k++) {
                int num = beyond1[k];

                if (VertexMarks[num]) {
                    IsBeyond(face, beyondBuffer, num);
                }
            }

            face.FurthestVertex = FurthestVertex;
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            face.VerticesBeyond = beyondBuffer;

            if (verticesBeyond.Count > 0) {
                verticesBeyond.Clear();
            }

            BeyondBuffer = verticesBeyond;
        }

        void FindBeyondVertices(ConvexFaceInternal face, IndexBuffer beyond) {
            IndexBuffer beyondBuffer = BeyondBuffer;
            MaxDistance = double.NegativeInfinity;
            FurthestVertex = 0;

            for (int i = 0; i < beyond.Count; i++) {
                int num = beyond[i];

                if (num != CurrentVertex) {
                    IsBeyond(face, beyondBuffer, num);
                }
            }

            face.FurthestVertex = FurthestVertex;
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            face.VerticesBeyond = beyondBuffer;

            if (verticesBeyond.Count > 0) {
                verticesBeyond.Clear();
            }

            BeyondBuffer = verticesBeyond;
        }

        void UpdateCenter() {
            for (int i = 0; i < Dimension; i++) {
                Center[i] *= ConvexHullSize;
            }

            ConvexHullSize++;
            double num = 1.0 / ConvexHullSize;
            int num2 = CurrentVertex * Dimension;

            for (int j = 0; j < Dimension; j++) {
                Center[j] = num * (Center[j] + Positions[num2 + j]);
            }
        }

        void RollbackCenter() {
            for (int i = 0; i < Dimension; i++) {
                Center[i] *= ConvexHullSize;
            }

            ConvexHullSize--;
            double num = ConvexHullSize <= 0 ? 0.0 : 1.0 / ConvexHullSize;
            int num2 = CurrentVertex * Dimension;

            for (int j = 0; j < Dimension; j++) {
                Center[j] = num * (Center[j] - Positions[num2 + j]);
            }
        }

        void HandleSingular() {
            RollbackCenter();
            SingularVertices.Add(CurrentVertex);

            for (int i = 0; i < AffectedFaceBuffer.Count; i++) {
                ConvexFaceInternal convexFaceInternal = FacePool[AffectedFaceBuffer[i]];
                IndexBuffer verticesBeyond = convexFaceInternal.VerticesBeyond;

                for (int j = 0; j < verticesBeyond.Count; j++) {
                    SingularVertices.Add(verticesBeyond[j]);
                }

                ConvexFaces.Add(convexFaceInternal.Index);
                UnprocessedFaces.Remove(convexFaceInternal);
                ObjectManager.DepositVertexBuffer(convexFaceInternal.VerticesBeyond);
                convexFaceInternal.VerticesBeyond = EmptyBuffer;
            }
        }

        void FindConvexHull() {
            InitConvexHull();

            while (UnprocessedFaces.First != null) {
                ConvexFaceInternal first = UnprocessedFaces.First;
                CurrentVertex = first.FurthestVertex;
                UpdateCenter();
                TagAffectedFaces(first);

                if (!SingularVertices.Contains(CurrentVertex) && CreateCone()) {
                    CommitCone();
                } else {
                    HandleSingular();
                }

                int count = AffectedFaceBuffer.Count;

                for (int i = 0; i < count; i++) {
                    AffectedFaceFlags[AffectedFaceBuffer[i]] = false;
                }
            }
        }

        void InitializeData(ConvexHullComputationConfig config) {
            UnprocessedFaces = new FaceList();
            ConvexFaces = new IndexBuffer();
            FacePool = new ConvexFaceInternal[(Dimension + 1) * 10];
            AffectedFaceFlags = new bool[(Dimension + 1) * 10];
            ObjectManager = new ObjectManager(this);
            Center = new double[Dimension];
            TraverseStack = new IndexBuffer();
            UpdateBuffer = new int[Dimension];
            UpdateIndices = new int[Dimension];
            EmptyBuffer = new IndexBuffer();
            AffectedFaceBuffer = new IndexBuffer();
            ConeFaceBuffer = new SimpleList<DeferredFace>();
            SingularVertices = new HashSet<int>();
            BeyondBuffer = new IndexBuffer();
            ConnectorTable = new ConnectorList[2017];

            for (int i = 0; i < 2017; i++) {
                ConnectorTable[i] = new ConnectorList();
            }

            VertexMarks = new bool[Vertices.Length];
            InitializePositions(config);
            MathHelper = new MathHelper(Dimension, Positions);
        }

        void InitializePositions(ConvexHullComputationConfig config) {
            Positions = new double[Vertices.Length * Dimension];
            int num = 0;

            if (IsLifted) {
                int num2 = Dimension - 1;
                Func<double> pointTranslationGenerator = config.PointTranslationGenerator;

                switch (config.PointTranslationType) {
                    case PointTranslationType.None: {
                        IVertex[] vertices2 = Vertices;

                        foreach (IVertex vertex2 in vertices2) {
                            double num5 = 0.0;

                            for (int l = 0; l < num2; l++) {
                                double num6 = vertex2.Position[l];
                                Positions[num++] = num6;
                                num5 += num6 * num6;
                            }

                            Positions[num++] = num5;
                        }

                        break;
                    }

                    case PointTranslationType.TranslateInternal: {
                        IVertex[] vertices = Vertices;

                        foreach (IVertex vertex in vertices) {
                            double num3 = 0.0;

                            for (int j = 0; j < num2; j++) {
                                double num4 = vertex.Position[j] + pointTranslationGenerator();
                                Positions[num++] = num4;
                                num3 += num4 * num4;
                            }

                            Positions[num++] = num3;
                        }

                        break;
                    }
                }

                return;
            }

            Func<double> pointTranslationGenerator2 = config.PointTranslationGenerator;

            switch (config.PointTranslationType) {
                case PointTranslationType.None: {
                    IVertex[] vertices4 = Vertices;

                    foreach (IVertex vertex4 in vertices4) {
                        for (int num8 = 0; num8 < Dimension; num8++) {
                            Positions[num++] = vertex4.Position[num8];
                        }
                    }

                    break;
                }

                case PointTranslationType.TranslateInternal: {
                    IVertex[] vertices3 = Vertices;

                    foreach (IVertex vertex3 in vertices3) {
                        for (int n = 0; n < Dimension; n++) {
                            Positions[num++] = vertex3.Position[n] + pointTranslationGenerator2();
                        }
                    }

                    break;
                }
            }
        }

        double GetCoordinate(int v, int i) => Positions[v * Dimension + i];

        int DetermineDimension() {
            Random random = new();
            int maxValue = Vertices.Length;
            List<int> list = new();

            for (int i = 0; i < 10; i++) {
                list.Add(Vertices[random.Next(maxValue)].Position.Length);
            }

            int num = list.Min();

            if (num != list.Max()) {
                throw new ArgumentException("Invalid input data (non-uniform dimension).");
            }

            return num;
        }

        int[] CreateInitialHull(List<int> initialPoints) {
            int[] array = new int[Dimension + 1];

            for (int i = 0; i < Dimension + 1; i++) {
                int[] array2 = new int[Dimension];
                int j = 0;
                int num = 0;

                for (; j <= Dimension; j++) {
                    if (i != j) {
                        array2[num++] = initialPoints[j];
                    }
                }

                ConvexFaceInternal convexFaceInternal = FacePool[ObjectManager.GetFace()];
                convexFaceInternal.Vertices = array2;
                Array.Sort(array2);
                MathHelper.CalculateFacePlane(convexFaceInternal, Center);
                array[i] = convexFaceInternal.Index;
            }

            for (int k = 0; k < Dimension; k++) {
                for (int l = k + 1; l < Dimension + 1; l++) {
                    UpdateAdjacency(FacePool[array[k]], FacePool[array[l]]);
                }
            }

            return array;
        }

        void UpdateAdjacency(ConvexFaceInternal l, ConvexFaceInternal r) {
            int[] vertices = l.Vertices;
            int[] vertices2 = r.Vertices;
            int i;

            for (i = 0; i < vertices.Length; i++) {
                VertexMarks[vertices[i]] = false;
            }

            for (i = 0; i < vertices2.Length; i++) {
                VertexMarks[vertices2[i]] = true;
            }

            for (i = 0; i < vertices.Length && VertexMarks[vertices[i]]; i++) { }

            if (i == Dimension) {
                return;
            }

            for (int j = i + 1; j < vertices.Length; j++) {
                if (!VertexMarks[vertices[j]]) {
                    return;
                }
            }

            l.AdjacentFaces[i] = r.Index;

            for (i = 0; i < vertices.Length; i++) {
                VertexMarks[vertices[i]] = false;
            }

            for (i = 0; i < vertices2.Length && !VertexMarks[vertices2[i]]; i++) { }

            r.AdjacentFaces[i] = l.Index;
        }

        void InitSingle() {
            int[] array = new int[Dimension];

            for (int i = 0; i < Vertices.Length; i++) {
                array[i] = i;
            }

            ConvexFaceInternal convexFaceInternal = FacePool[ObjectManager.GetFace()];
            convexFaceInternal.Vertices = array;
            Array.Sort(array);
            MathHelper.CalculateFacePlane(convexFaceInternal, Center);

            if (convexFaceInternal.Normal[Dimension - 1] >= 0.0) {
                for (int j = 0; j < Dimension; j++) {
                    convexFaceInternal.Normal[j] *= -1.0;
                }

                convexFaceInternal.Offset = 0.0 - convexFaceInternal.Offset;
                convexFaceInternal.IsNormalFlipped = !convexFaceInternal.IsNormalFlipped;
            }

            ConvexFaces.Add(convexFaceInternal.Index);
        }

        void InitConvexHull() {
            if (Vertices.Length < Dimension) {
                return;
            }

            if (Vertices.Length == Dimension) {
                InitSingle();
                return;
            }

            List<int> extremes = FindExtremes();
            List<int> list = FindInitialPoints(extremes);

            foreach (int item in list) {
                int num = CurrentVertex = item;
                UpdateCenter();
                VertexMarks[num] = true;
            }

            int[] array = CreateInitialHull(list);
            int[] array2 = array;

            foreach (int num2 in array2) {
                ConvexFaceInternal convexFaceInternal = FacePool[num2];
                FindBeyondVertices(convexFaceInternal);

                if (convexFaceInternal.VerticesBeyond.Count == 0) {
                    ConvexFaces.Add(convexFaceInternal.Index);
                } else {
                    UnprocessedFaces.Add(convexFaceInternal);
                }
            }

            foreach (int item2 in list) {
                VertexMarks[item2] = false;
            }
        }

        void FindBeyondVertices(ConvexFaceInternal face) {
            IndexBuffer verticesBeyond = face.VerticesBeyond;
            MaxDistance = double.NegativeInfinity;
            FurthestVertex = 0;
            int num = Vertices.Length;

            for (int i = 0; i < num; i++) {
                if (!VertexMarks[i]) {
                    IsBeyond(face, verticesBeyond, i);
                }
            }

            face.FurthestVertex = FurthestVertex;
        }

        List<int> FindInitialPoints(List<int> extremes) {
            List<int> list = new();
            int item = -1;
            int item2 = -1;
            double num = 0.0;
            double[] array = new double[Dimension];

            for (int i = 0; i < extremes.Count - 1; i++) {
                int num2 = extremes[i];

                for (int j = i + 1; j < extremes.Count; j++) {
                    int num3 = extremes[j];
                    MathHelper.SubtractFast(num2, num3, array);
                    double num4 = MathHelper.LengthSquared(array);

                    if (num4 > num) {
                        item = num2;
                        item2 = num3;
                        num = num4;
                    }
                }
            }

            list.Add(item);
            list.Add(item2);

            for (int k = 2; k <= Dimension; k++) {
                double num5 = double.NegativeInfinity;
                int num6 = -1;

                for (int l = 0; l < extremes.Count; l++) {
                    int num7 = extremes[l];

                    if (!list.Contains(num7)) {
                        double squaredDistanceSum = GetSquaredDistanceSum(num7, list);

                        if (squaredDistanceSum > num5) {
                            num5 = squaredDistanceSum;
                            num6 = num7;
                        }
                    }
                }

                if (num6 >= 0) {
                    list.Add(num6);
                    continue;
                }

                int num8 = Vertices.Length;

                for (int m = 0; m < num8; m++) {
                    if (!list.Contains(m)) {
                        double squaredDistanceSum2 = GetSquaredDistanceSum(m, list);

                        if (squaredDistanceSum2 > num5) {
                            num5 = squaredDistanceSum2;
                            num6 = m;
                        }
                    }
                }

                if (num6 >= 0) {
                    list.Add(num6);
                } else {
                    ThrowSingular();
                }
            }

            return list;
        }

        double GetSquaredDistanceSum(int pivot, List<int> initialPoints) {
            int count = initialPoints.Count;
            double num = 0.0;

            for (int i = 0; i < count; i++) {
                int v = initialPoints[i];

                for (int j = 0; j < Dimension; j++) {
                    double num2 = GetCoordinate(v, j) - GetCoordinate(pivot, j);
                    num += num2 * num2;
                }
            }

            return num;
        }

        int LexCompare(int u, int v) {
            int num = u * Dimension;
            int num2 = v * Dimension;

            for (int i = 0; i < Dimension; i++) {
                double num3 = Positions[num + i];
                double value = Positions[num2 + i];
                int num4 = num3.CompareTo(value);

                if (num4 != 0) {
                    return num4;
                }
            }

            return 0;
        }

        List<int> FindExtremes() {
            List<int> list = new(2 * Dimension);
            int num = Vertices.Length;

            for (int i = 0; i < Dimension; i++) {
                double num2 = double.MaxValue;
                double num3 = double.MinValue;
                int num4 = 0;
                int num5 = 0;

                for (int j = 0; j < num; j++) {
                    double coordinate = GetCoordinate(j, i);
                    double num6 = num2 - coordinate;

                    if (num6 >= 0.0) {
                        if (num6 < PlaneDistanceTolerance) {
                            if (LexCompare(j, num4) < 0) {
                                num2 = coordinate;
                                num4 = j;
                            }
                        } else {
                            num2 = coordinate;
                            num4 = j;
                        }
                    }

                    num6 = coordinate - num3;

                    if (!(num6 >= 0.0)) {
                        continue;
                    }

                    if (num6 < PlaneDistanceTolerance) {
                        if (LexCompare(j, num5) > 0) {
                            num3 = coordinate;
                            num5 = j;
                        }
                    } else {
                        num3 = coordinate;
                        num5 = j;
                    }
                }

                if (num4 != num5) {
                    list.Add(num4);
                    list.Add(num5);
                } else {
                    list.Add(num4);
                }
            }

            HashSet<int> hashSet = new(list);

            if (hashSet.Count <= Dimension) {
                for (int k = 0; k < num; k++) {
                    if (hashSet.Count > Dimension) {
                        break;
                    }

                    hashSet.Add(k);
                }
            }

            return hashSet.ToList();
        }

        void ThrowSingular() => throw new InvalidOperationException(
                                    "Singular input data (i.e. trying to triangulate a data that contain a regular lattice of points) detected. Introducing some noise to the data might resolve the issue.");

        internal static ConvexHull<TVertex, TFace> GetConvexHull<TVertex, TFace>(IList<TVertex> data,
            ConvexHullComputationConfig config) where TVertex : IVertex where TFace : ConvexFace<TVertex, TFace>, new() {
            config = config ?? new ConvexHullComputationConfig();
            IVertex[] array = new IVertex[data.Count];

            for (int i = 0; i < data.Count; i++) {
                array[i] = data[i];
            }

            ConvexHullInternal convexHullInternal = new(array, false, config);
            convexHullInternal.FindConvexHull();
            TVertex[] hullVertices = convexHullInternal.GetHullVertices(data);
            ConvexHull<TVertex, TFace> convexHull = new();
            convexHull.Points = hullVertices;
            convexHull.Faces = convexHullInternal.GetConvexFaces<TVertex, TFace>();
            return convexHull;
        }

        TVertex[] GetHullVertices<TVertex>(IList<TVertex> data) {
            int count = ConvexFaces.Count;
            int num = 0;
            int num2 = Vertices.Length;

            for (int i = 0; i < num2; i++) {
                VertexMarks[i] = false;
            }

            for (int j = 0; j < count; j++) {
                int[] vertices = FacePool[ConvexFaces[j]].Vertices;

                foreach (int num3 in vertices) {
                    if (!VertexMarks[num3]) {
                        VertexMarks[num3] = true;
                        num++;
                    }
                }
            }

            TVertex[] array = new TVertex[num];

            for (int l = 0; l < num2; l++) {
                if (VertexMarks[l]) {
                    array[--num] = data[l];
                }
            }

            return array;
        }

        TFace[] GetConvexFaces<TVertex, TFace>() where TVertex : IVertex where TFace : ConvexFace<TVertex, TFace>, new() {
            IndexBuffer convexFaces = ConvexFaces;
            int count = convexFaces.Count;
            TFace[] array = new TFace[count];

            for (int i = 0; i < count; i++) {
                ConvexFaceInternal convexFaceInternal = FacePool[convexFaces[i]];
                TVertex[] array2 = new TVertex[Dimension];

                for (int j = 0; j < Dimension; j++) {
                    array2[j] = (TVertex)Vertices[convexFaceInternal.Vertices[j]];
                }

                array[i] = new TFace {
                    Vertices = array2,
                    Adjacency = new TFace[Dimension],
                    Normal = !IsLifted ? convexFaceInternal.Normal : null
                };

                convexFaceInternal.Tag = i;
            }

            for (int k = 0; k < count; k++) {
                ConvexFaceInternal convexFaceInternal2 = FacePool[convexFaces[k]];
                TFace val = array[k];

                for (int l = 0; l < Dimension; l++) {
                    if (convexFaceInternal2.AdjacentFaces[l] >= 0) {
                        val.Adjacency[l] = array[FacePool[convexFaceInternal2.AdjacentFaces[l]].Tag];
                    }
                }

                if (convexFaceInternal2.IsNormalFlipped) {
                    TVertex val2 = val.Vertices[0];
                    val.Vertices[0] = val.Vertices[Dimension - 1];
                    val.Vertices[Dimension - 1] = val2;
                    TFace val3 = val.Adjacency[0];
                    val.Adjacency[0] = val.Adjacency[Dimension - 1];
                    val.Adjacency[Dimension - 1] = val3;
                }
            }

            return array;
        }
    }
}