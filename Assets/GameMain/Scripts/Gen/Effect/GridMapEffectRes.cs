
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Effect
{

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class GridMapEffectRes :  Bright.Config.BeanBase 
    {
        public GridMapEffectRes(ByteBuf _buf) 
        {
            Id = _buf.ReadInt();
            Name = _buf.ReadString();
            AssetName = _buf.ReadString();
        }

        public static GridMapEffectRes DeserializeGridMapEffectRes(ByteBuf _buf)
        {
            return new Effect.GridMapEffectRes(_buf);
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetName { get; private set; }

        public const int ID = 929444982;
        public override int GetTypeId() => ID;

        public  void Resolve(Dictionary<string, object> _tables)
        {
        }

        public  void TranslateText(System.Func<string, string, string> translator)
        {
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "Name:" + Name + ","
            + "AssetName:" + AssetName + ","
            + "}";
        }
    }
}