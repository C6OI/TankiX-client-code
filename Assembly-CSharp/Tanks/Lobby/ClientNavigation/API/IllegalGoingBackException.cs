using System;

namespace Tanks.Lobby.ClientNavigation.API {
    public class IllegalGoingBackException : Exception {
        public IllegalGoingBackException(string message)
            : base(message) { }
    }
}