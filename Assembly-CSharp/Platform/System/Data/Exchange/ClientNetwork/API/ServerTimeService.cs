using System;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public interface ServerTimeService {
        long InitialServerTime { get; }

        event Action<long> OnInitServerTime;
    }
}