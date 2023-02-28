using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class JoinByType : JoinType {
        readonly Type contextComponent;

        public JoinByType(Type contextComponent) => this.contextComponent = contextComponent;

        public Optional<Type> ContextComponent => Optional<Type>.of(contextComponent);

        public ICollection<Entity> GetEntities(NodeCollectorImpl nodeCollector, NodeDescription nodeDescription, Entity key) {
            GroupComponent groupComponent;

            if ((groupComponent = (GroupComponent)((EntityUnsafe)key).GetComponentUnsafe(contextComponent)) != null) {
                return groupComponent.GetGroupMembers(nodeDescription);
            }

            return Collections.EmptyList<Entity>();
        }
    }
}