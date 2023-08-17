using System;
using System.Collections.Generic;

namespace Lobby.ClientControls.API.List {
    public interface ListDataProvider {
        IList<object> Data { get; }

        event Action<ListDataProvider> DataChanged;
    }
}