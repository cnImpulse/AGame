
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;


namespace cfg.Entity
{

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class EntityRes :  Bright.Config.BeanBase 
    {
        public EntityRes(ByteBuf _buf) 
        {
            Id = _buf.ReadInt();
            Name = _buf.ReadString();
            AssetName = _buf.ReadString();
            Priority = _buf.ReadInt();
        }

        public static EntityRes DeserializeEntityRes(ByteBuf _buf)
        {
            return new Entity.EntityRes(_buf);
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
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; private set; }

        public const int ID = -1578301070;
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
            + "Priority:" + Priority + ","
            + "}";
        }
    }
}