using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerCallContextChangedException : Exception {
        public HandlerCallContextChangedException() { }

        public HandlerCallContextChangedException(Handler handler, HandlerArgument argument, Entity entity)
            : base(CreateMessage(handler, argument, entity)) { }

        public HandlerCallContextChangedException(Handler handler, NodeClassInstanceDescription nodeDesc, Entity entity)
            : base(CreateMessage(handler, nodeDesc, entity)) { }

        static string CreateMessage(Handler handler, HandlerArgument argument, Entity entity) {
            string text = "\nMethod: " +
                          handler.GetHandlerName() +
                          "\nNodeClass: " +
                          argument.ClassInstanceDescription.NodeClass.Name +
                          " Node: " +
                          argument.NodeDescription;

            if (entity != null) {
                text = text + "\nEntity: " + (entity as EntityInternal).ToStringWithComponentsClasses();
            }

            return text;
        }

        static string CreateMessage(Handler handler, NodeClassInstanceDescription nodeDesc, Entity entity) {
            object[] obj = new object[6] {
                "\nMethod: ",
                handler.GetHandlerName(),
                "\nNodeClass: ",
                nodeDesc == null ? null : nodeDesc.NodeClass.Name,
                " Node: ",
                null
            };

            object obj2;

            if (nodeDesc != null) {
                NodeDescription nodeDescription = nodeDesc.NodeDescription;
                obj2 = nodeDescription;
            } else {
                obj2 = null;
            }

            obj[5] = obj2;
            string text = string.Concat(obj);

            if (entity != null) {
                text = text + "\nEntity: " + (entity as EntityInternal).ToStringWithComponentsClasses();
            }

            return text;
        }
    }
}