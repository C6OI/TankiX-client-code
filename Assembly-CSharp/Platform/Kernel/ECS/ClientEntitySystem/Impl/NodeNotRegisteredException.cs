using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeNotRegisteredException : Exception {
        public NodeNotRegisteredException(NodeDescription nodeDescription)
            : base(string.Concat("Node not registered: ", nodeDescription,
                "\npublic void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager, EngineServiceInternal engineService, NodeRegistrator nodeRegistrator) {\n\tbase(...)\n\tnodeRegistrator.Register(typeof(MyNode));\n}\n")) { }
    }
}