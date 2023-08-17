using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Platform.Library.ClientUnityIntegration.API {
    [DisallowMultipleComponent]
    public class EntityBehaviour : MonoBehaviour {
        const string INVALID_TEMPLATE_TYPE_FORMAT = "Invalid Type {0} on object {1}";

        [SerializeField] [Obsolete] public string template;

        [SerializeField] int templateIdLow;

        [SerializeField] int templateIdHigh;

        public string configPath;

        public bool handleAutomaticaly;

        readonly List<Component> components = new();

        [Inject] public static GroupRegistry GroupRegistry { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public long TemplateId {
            get {
                if (templateIdHigh == 0 && templateIdLow == 0 && !string.IsNullOrEmpty(template)) {
                    Type type = Type.GetType(template);

                    if (type == null) {
                        Debug.LogError(string.Format("Invalid Type {0} on object {1}", type, name), this);
                    } else {
                        TemplateId = TemplateRegistry.GetTemplateInfo(type).TemplateId;
                    }
                }

                return (long)templateIdHigh << 32 | (uint)templateIdLow;
            }
            set {
                templateIdLow = (int)(value & 0xFFFFFFFFu);
                templateIdHigh = (int)(value >> 32);
            }
        }

        public Entity Entity { get; private set; }

        void Awake() {
            if (GetComponents<EntityBehaviour>().Length > 1) {
                throw new GameObjectAlreadyContainsEntityBehaviour(gameObject);
            }
        }

        void OnEnable() {
            if (handleAutomaticaly) {
                CreateEntity();
            }
        }

        void OnDisable() {
            if (handleAutomaticaly &&
                Entity != null &&
                ((EntityInternal)Entity).Alive &&
                ClientUnityIntegrationUtils.HasEngine()) {
                DestroyEntity();
            }
        }

        void OnDestroy() {
            if (!handleAutomaticaly &&
                Entity != null &&
                ((EntityInternal)Entity).Alive &&
                ClientUnityIntegrationUtils.HasEngine()) {
                CollectComponents(gameObject, RemoveComponent);
            }
        }

        void OnApplicationQuit() => Entity = null;

        void CreateEntity() {
            if (Flow.CurrentFlowExist) {
                DoCreateEntity(EngineService.Engine);
                return;
            }

            Flow flow = EngineService.NewFlow().StartWith(delegate(Engine engine) {
                DoCreateEntity(engine);
            });

            EngineService.ExecuteFlow(flow);
        }

        void DoCreateEntity(Engine engine) {
            long templateId = TemplateId;
            Entity entity2;

            if (templateId != 0L) {
                Entity entity = engine.CreateEntity(templateId, configPath);
                entity2 = entity;
            } else {
                entity2 = engine.CreateEntity(name);
            }

            Entity entity3 = entity2;
            BuildEntity(entity3);
        }

        public void DestroyEntity() {
            if (Flow.CurrentFlowExist) {
                EngineService.Engine.DeleteEntity(Entity);
            } else {
                Flow flow = EngineService.NewFlow().StartWith(delegate {
                    EngineService.Engine.DeleteEntity(Entity);
                });

                EngineService.ExecuteFlow(flow);
            }

            Entity = null;
        }

        void RemoveComponent(Kernel.ECS.ClientEntitySystem.API.Component component) =>
            ClientUnityIntegrationUtils.ExecuteInFlow(delegate {
                Type type = component.GetType();

                if (Entity.HasComponent(type)) {
                    Entity.RemoveComponent(type);
                }
            });

        public void BuildEntity(Entity entity) {
            if (Entity != null) {
                throw new EntityAlreadyExistsException(Entity.Name);
            }

            Entity = entity;
            CollectComponents(gameObject, AddComponent);
        }

        void AddComponent(Kernel.ECS.ClientEntitySystem.API.Component component) {
            ComponentInstanceDataUpdater.Update((EntityInternal)Entity, component);
            Entity.AddComponent(component);
        }

        public void Join<T>(Entity key) where T : GroupComponent, new() {
            AddJoinComponent<T>(key);
            JoinChildren<T>(gameObject.transform, key);
        }

        void JoinChildren<T>(Transform transform, Entity key) where T : GroupComponent, new() {
            foreach (Transform item in transform) {
                EntityBehaviour component = item.GetComponent<EntityBehaviour>();

                if (component != null) {
                    component.Join<T>(key);
                } else {
                    JoinChildren<T>(item, key);
                }
            }
        }

        void AddJoinComponent<T>(Entity key) where T : GroupComponent {
            GroupComponent component = GroupRegistry.FindOrCreateGroup<T>(key.Id);
            Entity.AddComponent(component);
        }

        void CollectComponents(GameObject gameObject, Action<Kernel.ECS.ClientEntitySystem.API.Component> handler) {
            gameObject.GetComponents(typeof(Kernel.ECS.ClientEntitySystem.API.Component), components);

            foreach (Component component in components) {
                handler((Kernel.ECS.ClientEntitySystem.API.Component)component);
            }

            foreach (Transform item in gameObject.transform) {
                if (!(item.GetComponent<EntityBehaviour>() != null)) {
                    CollectComponents(item.gameObject, handler);
                }
            }
        }
    }
}