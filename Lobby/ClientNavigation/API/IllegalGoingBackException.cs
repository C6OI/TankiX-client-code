using System;

namespace Lobby.ClientNavigation.API {
    public class IllegalGoingBackException : Exception {
        public IllegalGoingBackException(string message)
            : base(message) { }
    }
}