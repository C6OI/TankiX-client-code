using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientResources.API {
    public class ResourceDataListComponent : Component {
        public ResourceDataListComponent() { }

        public ResourceDataListComponent(List<Object> dataList) => DataList = dataList;

        public List<Object> DataList { get; set; }
    }
}