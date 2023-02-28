using System;
using System.Collections.Generic;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public static class SkipInfoBuilder {
        [Inject] public static EngineService Engine { get; set; }

        public static string GetSkipReasonDetails(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Optional<JoinType> join) {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendFormat("{0} was skiped, because {1} not found ", handler.Name, toArgumentNode.argument.ClassInstanceDescription.NodeClass.FullName);
            GroupComponent groupComponent = null;

            if (join.IsPresent() && !(join.Get() is JoinAllType) && fromArgumentNode.entityNodes.Count > 0) {
                Entity entity = fromArgumentNode.entityNodes[0].entity;
                Type type = join.Get().ContextComponent.Get();

                if (entity.HasComponent(type)) {
                    groupComponent = (GroupComponent)entity.GetComponent(type);
                }
            }

            Entity entity2 = null;
            List<Type> list = new();
            List<Type> list2 = new();
            int num = 0;

            foreach (Entity allEntity in ((EngineServiceImpl)Engine).EntityRegistry.GetAllEntities()) {
                int num2 = 0;
                List<Type> list3 = new();
                List<Type> list4 = new();
                list3.Clear();
                list4.Clear();

                foreach (Type component2 in toArgumentNode.argument.NodeDescription.Components) {
                    if (allEntity.HasComponent(component2)) {
                        Component component = allEntity.GetComponent(component2);

                        if (groupComponent != null && component.GetType() == groupComponent.GetType()) {
                            if (!((GroupComponent)component).Key.Equals(groupComponent.Key)) {
                                continue;
                            }

                            num2++;
                        }

                        num2++;
                        list3.Add(component2);
                    } else {
                        list4.Add(component2);
                    }
                }

                if (num2 > num) {
                    num = num2;
                    entity2 = allEntity;
                    list = new List<Type>(list3);
                    list2 = new List<Type>(list4);
                }
            }

            if (entity2 != null) {
                stringBuilder.AppendFormat("\n Best node was {0} , presentComponents {1}, absentComponents {2} ", entity2, EcsToStringUtil.ToString(list),
                    EcsToStringUtil.ToString(list2));
            }

            return stringBuilder.ToString();
        }
    }
}