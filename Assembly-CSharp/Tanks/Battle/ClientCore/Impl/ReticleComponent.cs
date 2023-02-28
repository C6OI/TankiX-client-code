using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientCore.Impl {
    public class ReticleComponent : Component {
        const string objectName = "reticle";

        Transform parent;

        Object prefabData;

        readonly List<Reticle> reticles = new();

        public bool Hammer { get; set; }

        public bool CanHeal { get; set; }

        public long TeamKey { get; set; }

        public Vector2 CanvasSize { get; set; }

        public void Create(Object prefabData, Transform parent) {
            this.prefabData = prefabData;
            this.parent = parent;
            reticles.Add(new Reticle(prefabData, parent, CanvasSize));
        }

        Dictionary<Entity, TargetData> GetHammerTargets(List<TargetData> targets) {
            return (from x in (from x in targets
                               where x.PriorityWeakeningCount == 0
                               group x by x.TargetEntity).Select(Enumerable.First)
                    where isValidAndVisible(x)
                    select x).ToDictionary(x => x.TargetEntity, x => x);
        }

        bool isValidAndVisible(TargetData targetData) => targetData.ValidTarget && !targetData.TargetEntity.HasComponent<TankInvisibilityEffectWorkingStateComponent>() &&
                                                         !targetData.TargetEntity.HasComponent<TankInvisibilityEffectActivationStateComponent>();

        void SetHammerTargets(List<TargetData> targets, Vector2 canvasSize) {
            Dictionary<Entity, TargetData> hammerTargets = GetHammerTargets(targets);

            foreach (Reticle reticle2 in reticles) {
                if (reticle2.Entity != null && hammerTargets.ContainsKey(reticle2.Entity)) {
                    reticle2.SetEnemy(hammerTargets[reticle2.Entity]);
                    hammerTargets.Remove(reticle2.Entity);
                } else {
                    reticle2.SetFree();
                }
            }

            foreach (KeyValuePair<Entity, TargetData> item in hammerTargets) {
                Reticle reticle = reticles.FirstOrDefault(x => x.Entity == null);

                if (reticle == null) {
                    reticle = new Reticle(prefabData, parent, CanvasSize);
                    reticles.Add(reticle);
                }

                reticle.Entity = item.Key;
                reticle.SetEnemy(hammerTargets[reticle.Entity]);
            }
        }

        public void SetTargets(List<TargetData> targets, Vector2 canvasSize) {
            if (Hammer) {
                SetHammerTargets(targets, canvasSize);
            } else {
                if (!reticles.Any()) {
                    return;
                }

                Reticle reticle = reticles.First();
                TargetData targetData = targets.FirstOrDefault(x => x.PriorityWeakeningCount == 0 && isValidAndVisible(x));

                if (targetData != null) {
                    if (CanHeal && targetData.TargetEntity.GetComponent<TeamGroupComponent>().Key == TeamKey) {
                        reticle.SetTeammate(targetData);
                    } else {
                        reticle.SetEnemy(targetData);
                    }
                } else {
                    reticle.SetFree();
                }
            }
        }

        public void SetFree() {
            foreach (Reticle reticle in reticles) {
                reticle.SetFree();
            }
        }

        public void Reset() {
            foreach (Reticle reticle in reticles) {
                reticle.Reset();
            }
        }

        public void Deactivate() {
            foreach (Reticle reticle in reticles) {
                reticle.Deactivate();
            }
        }

        public void Destroy() {
            foreach (Reticle reticle in reticles) {
                reticle.GameObject.RecycleObject();
            }

            reticles.Clear();
        }

        class ReticleTimer : MonoBehaviour {
            Action action;

            Action end;

            float time;

            void Update() {
                if (Time.realtimeSinceStartup < time) {
                    action();
                } else if (end != null) {
                    end();
                }
            }

            public void SetAction(Action action, float time, Action end) {
                this.action = action;
                this.end = end;
                this.time = Time.realtimeSinceStartup + time;
            }

            public void Break() {
                time = 0f;
                end = null;
            }
        }

        class Reticle {
            const string nameOfDefaultAnimation = "Hide";

            const string nameOfHideEnemyAnimation = "HideEnemy";

            const string nameOfHideTeammateAnimation = "HideTeammate";

            const float reticleSize = 0.12f;

            const float freeAnimationTime = 1f;

            const float reticleHeightCorrection = 1.1f;

            bool active = true;

            Animator animator;

            readonly Vector2 canvasSize;

            public Entity Entity;

            GameObject gameObject;

            Vector3 lastWorldPosition = Vector2.zero;

            State state;

            readonly ReticleTimer timer;

            Transform transform;

            public Reticle(Object prefabData, Transform parent, Vector2 canvasSize) {
                this.canvasSize = canvasSize;
                GameObject gameObject = Object.Instantiate(prefabData) as GameObject;
                gameObject.name = "reticle";
                gameObject.transform.SetParent(parent);
                gameObject.transform.SetAsFirstSibling();
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = Vector3.zero;
                RectTransform component = gameObject.GetComponent<RectTransform>();
                Vector2 anchorMin = component.anchorMin;
                anchorMin.y = 0.44f;
                component.anchorMin = anchorMin;
                Vector2 anchorMax = component.anchorMax;
                anchorMax.y = 0.56f;
                component.anchorMax = anchorMax;
                component.offsetMax = Vector2.zero;
                component.offsetMin = Vector2.zero;
                timer = gameObject.AddComponent<ReticleTimer>();
                GameObject = gameObject;
            }

            public GameObject GameObject {
                get => gameObject;
                private set {
                    gameObject = value;
                    animator = value.GetComponent<Animator>();
                    transform = value.transform;
                }
            }

            public void Reset() {
                state = State.Hiden;
                animator.Play("Hide");
                active = true;
            }

            Vector2 GetPoint(Vector3 originWithOffset) {
                Vector2 vector = Camera.main.WorldToScreenPoint(originWithOffset);
                Vector2 vector2 = new(vector.x / Screen.width * canvasSize.x, vector.y / Screen.height * canvasSize.y);
                return vector2 - canvasSize / 2f;
            }

            void SetLastPoint() {
                transform.localPosition = GetPoint(lastWorldPosition);
            }

            public void SetEnemy(TargetData targetData) {
                SetTarget(targetData, State.HighlightEnemy);
            }

            public void SetTeammate(TargetData targetData) {
                SetTarget(targetData, State.HighlightTeammate);
            }

            void SetTarget(TargetData targetData, State state) {
                if (active) {
                    Vector2 point = GetPoint(
                        lastWorldPosition = targetData.TargetPosition + targetData.TargetEntity.GetComponent<HullInstanceComponent>().HullInstance.transform.up * 1.1f);

                    if (this.state != state) {
                        animator.Play(state.ToString());
                        this.state = state;
                        timer.Break();
                    }

                    transform.localPosition = point;
                }
            }

            public void Deactivate() {
                animator.Play("Hide");
                active = false;
                Detach();
            }

            void Detach() {
                Entity = null;
                timer.Break();
            }

            public void SetFree() {
                if (active && state != 0) {
                    if (state == State.HighlightEnemy) {
                        animator.Play("HideEnemy");
                    } else {
                        animator.Play("HideTeammate");
                    }

                    state = State.Hiden;
                    timer.SetAction(SetLastPoint, 1f, Detach);
                }
            }

            enum State {
                Hiden = 0,
                HighlightEnemy = 1,
                HighlightTeammate = 2
            }
        }
    }
}