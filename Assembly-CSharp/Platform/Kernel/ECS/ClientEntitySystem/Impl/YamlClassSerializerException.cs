using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class YamlClassSerializerException : Exception {
        public YamlClassSerializerException(string className, Exception e)
            : base("className=" + className, e) { }
    }
}