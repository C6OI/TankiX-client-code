using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientCommunicator.Impl;

[SerialVersionUID(636451756485532125L)]
public interface GeneralChatTemplate : ChatTemplate, Template {
    GeneralChatComponent generalChat();
}