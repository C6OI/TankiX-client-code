using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1493972686116L)]
    public interface PresetUserItemTemplate : PresetItemTemplate, UserItemTemplate, GarageItemTemplate, Template {
        PresetNameComponent presetName();
    }
}