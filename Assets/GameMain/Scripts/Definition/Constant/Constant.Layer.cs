using UnityEngine;

namespace SSRPG
{
    public static partial class Constant
    {
        /// <summary>
        /// 图层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayer = "Default";
            public static readonly int DefaultLayerId = LayerMask.NameToLayer(DefaultLayer);

            public const string UILayer = "UI";
            public static readonly int UILayerId = LayerMask.NameToLayer(UILayer);

            public const string GridUnitLayer = "GridUnit";
            public static readonly int GridUnitLayerId = LayerMask.NameToLayer(GridUnitLayer);

            public const string GridMapLayer = "GridMap";
            public static readonly int GridMapLayerId = LayerMask.NameToLayer(GridMapLayer);
        }
    }
}