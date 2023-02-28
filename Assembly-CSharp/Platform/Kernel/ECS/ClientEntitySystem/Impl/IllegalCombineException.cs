using System;
using System.Text;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class IllegalCombineException : Exception {
        public IllegalCombineException(Handler handler, ArgumentNode argumentNode)
            : base(string.Format("Expected one entity, but found more:\n handler {0}, argument {1}, entities [{2}]", EcsToStringUtil.ToString(handler),
                handler.Method.GetParameters()[argumentNode.argument.NodeNumber + 1].Name, GetEntities(argumentNode))) { }

        public static string GetEntities(ArgumentNode argumentNode) {
            StringBuilder stringBuilder = new();

            for (int i = 0; i < argumentNode.entityNodes.Count; i++) {
                if (i > 0) {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(argumentNode.entityNodes[i].entity);
            }

            return stringBuilder.ToString();
        }
    }
}