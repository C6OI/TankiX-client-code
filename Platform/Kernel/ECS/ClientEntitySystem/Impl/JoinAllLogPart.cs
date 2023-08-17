using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class JoinAllLogPart : LogPart {
        readonly HandlerArgument handlerArgument;
        readonly ICollection<Entity> resolvedEntities;

        public JoinAllLogPart(HandlerArgument handlerArgument, ICollection<Entity> resolvedEntities) {
            this.resolvedEntities = resolvedEntities;
            this.handlerArgument = handlerArgument;
        }

        string GetNodeClassName => handlerArgument.ClassInstanceDescription.NodeClass.Name;

        public Optional<string> GetSkipReason() => resolvedEntities.Count != 0 ? Optional<string>.empty()
                                                       : Optional<string>.of(string.Format(
                                                           "\tMissing JoinAll node={0}, parameter=[{1}]\n\t",
                                                           GetNodeClassName,
                                                           handlerArgument.NodeNumber + 1));
    }
}