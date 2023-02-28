using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;

namespace Tanks.Lobby.ClientGarage.API {
    public interface GarageItemsRegistry {
        ICollection<TankPartItem> Hulls { get; }

        ICollection<TankPartItem> Turrets { get; }

        ICollection<ContainerBoxItem> Containers { get; }

        ICollection<VisualItem> Paints { get; }

        ICollection<VisualItem> Coatings { get; }

        ICollection<DetailItem> Details { get; }

        ICollection<ModuleItem> Modules { get; }

        T GetItem<T>(Entity marketEntity) where T : GarageItem, new();

        T GetItem<T>(long marketId) where T : GarageItem, new();
    }
}