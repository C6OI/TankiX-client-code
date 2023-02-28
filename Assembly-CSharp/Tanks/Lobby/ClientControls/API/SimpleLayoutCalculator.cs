using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class SimpleLayoutCalculator {
        static readonly float EPSILON = 0.001f;

        int elementIndex;

        public List<Element> elements = new();

        public int iterations;

        public int iterationsOuter;

        public int newCount;

        float sizeLeft;

        public float totalFlexible;

        public float totalMax;

        public float totalMin;

        public bool unlimited;

        readonly List<Element> unresolvedElements = new();

        public void Reset(int count) {
            newCount = 0;
            EnsureArraySize(count);
            elementIndex = 0;
            totalMin = 0f;
            totalFlexible = 0f;
            totalMax = 0f;
            unlimited = false;
            iterations = 0;
            iterationsOuter = 0;
            unresolvedElements.Clear();
            unresolvedElements.Capacity = count;
        }

        public void AddElement(float flexible = 0f, float min = 0f, float max = 0f) {
            EnsureArraySize(elementIndex + 1);
            Element element = elements[elementIndex];
            element.i = elementIndex;
            element.Min = min;
            element.Flexible = flexible;
            element.Max = max;
            element.size = 0f;
            element.sizeIfNoLimits = 0f;
            totalMin += element.Min;
            totalFlexible += element.Flexible;
            totalMax += element.Max;
            unlimited |= element.Unlimited();

            if (!element.IsFixed()) {
                unresolvedElements.Add(element);
            }

            elementIndex++;
        }

        public void Calculate(float maxSize) {
            SetArraySize(elementIndex);
            sizeLeft = !unlimited ? Mathf.Min(totalMax, maxSize) : Mathf.Max(totalMin, maxSize);
            DistributeSizeToFixedElements();

            if (totalFlexible == 0f) {
                return;
            }

            while (!DistributedAll()) {
                iterationsOuter++;
                DistributeSizeLeft();
                ClampMinMax();

                if (!DistributedAll()) {
                    if (sizeLeft > 0f) {
                        ResolveElementsAtMax();
                    } else {
                        ResolveElementsAtMin();
                    }

                    RevertSizeFromUnresolvedElements();
                }

                if (iterations >= 100 || unresolvedElements.Count == 0) {
                    break;
                }
            }
        }

        void RevertSizeFromUnresolvedElements() {
            int num = 0;

            while (num < unresolvedElements.Count) {
                Element element = unresolvedElements[num];
                sizeLeft += element.size;
                element.size = 0f;
                num++;
                iterations++;
            }
        }

        void DistributeSizeLeft() {
            float num = sizeLeft;
            int num2 = 0;

            while (num2 < unresolvedElements.Count) {
                Element element = unresolvedElements[num2];
                element.sizeIfNoLimits = num * element.Flexible / totalFlexible;
                float size = element.size;
                element.size = element.sizeIfNoLimits;
                sizeLeft -= element.size - size;
                num2++;
                iterations++;
            }
        }

        bool DistributedAll() => 0f - EPSILON < sizeLeft && sizeLeft < EPSILON;

        void ResolveElementsAtMin() {
            int num = 0;

            while (num < unresolvedElements.Count) {
                Element element = unresolvedElements[num];

                if (element.AtMin()) {
                    unresolvedElements.RemoveAt(num);
                    totalFlexible -= element.Flexible;
                    num--;
                }

                num++;
                iterations++;
            }
        }

        void ResolveElementsAtMax() {
            int num = 0;

            while (num < unresolvedElements.Count) {
                Element element = unresolvedElements[num];

                if (element.AtMax()) {
                    unresolvedElements.RemoveAt(num);
                    totalFlexible -= element.Flexible;
                    num--;
                }

                num++;
                iterations++;
            }
        }

        void ClampMinMax() {
            int num = 0;

            while (num < unresolvedElements.Count) {
                Element element = unresolvedElements[num];
                float size = element.size;

                if (element.sizeIfNoLimits < element.Min) {
                    element.size = element.Min;
                } else if (element.Max > 0f && element.sizeIfNoLimits > element.Max) {
                    element.size = element.Max;
                }

                sizeLeft -= element.size - size;
                num++;
                iterations++;
            }
        }

        void DistributeSizeToFixedElements() {
            int num = 0;

            while (num < elements.Count) {
                Element element = elements[num];

                if (element.IsFixed()) {
                    sizeLeft -= element.size = element.Min;
                }

                num++;
                iterations++;
            }
        }

        void SetArraySize(int count) {
            EnsureArraySize(count);

            while (elements.Count > count) {
                elements.RemoveAt(elements.Count - 1);
            }
        }

        void EnsureArraySize(int count) {
            while (elements.Count < count) {
                elements.Add(new Element());
                newCount++;
            }
        }

        public class Element {
            public float Flexible;

            public float i;

            public float Max;
            public float Min;

            public float size;

            public float sizeIfNoLimits;

            public bool AtMin() => size == Min;

            public bool AtMax() => Max > 0f && size == Max;

            public bool Unlimited() => Flexible > 0f && Max == 0f;

            public bool IsFixed() => Flexible == 0f;

            public override string ToString() =>
                "[i=" + i + " sizeIfNoLimits=" + sizeIfNoLimits + string.Format(" Min: {0}, Flexible: {1}, Max: {2}, Size: {3}]", Min, Flexible, Max, size);
        }
    }
}