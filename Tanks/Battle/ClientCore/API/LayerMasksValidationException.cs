using System;

namespace Tanks.Battle.ClientCore.API {
    public class LayerMasksValidationException : ArgumentOutOfRangeException {
        public LayerMasksValidationException(string message)
            : base(message) { }
    }
}