//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace Cfg.Entity
{
   
public partial class TblEntity
{
    private readonly Dictionary<int, Entity.EntityRes> _dataMap;
    private readonly List<Entity.EntityRes> _dataList;
    
    public TblEntity(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Entity.EntityRes>();
        _dataList = new List<Entity.EntityRes>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Entity.EntityRes _v;
            _v = Entity.EntityRes.DeserializeEntityRes(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Entity.EntityRes> DataMap => _dataMap;
    public List<Entity.EntityRes> DataList => _dataList;

    public Entity.EntityRes GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Entity.EntityRes Get(int key) => _dataMap[key];
    public Entity.EntityRes this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}