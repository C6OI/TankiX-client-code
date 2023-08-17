using System;

namespace Platform.Library.ClientUnityIntegration.API {
    public class EntityAlreadyExistsException : Exception {
        public EntityAlreadyExistsException(string name)
            : base(string.Format("Entity already exists, name = {0}", name)) { }
    }
}