using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

[Shared]
[SerialVersionUID(636174034381039231L)]
public class ItemsPackFromConfigComponent : Component {
    public List<long> Pack { get; set; }
}