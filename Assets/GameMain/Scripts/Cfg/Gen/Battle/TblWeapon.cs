//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace Cfg.Battle
{
   
public partial class TblWeapon
{
    private readonly Dictionary<int, Battle.Weapon> _dataMap;
    private readonly List<Battle.Weapon> _dataList;
    
    public TblWeapon(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Battle.Weapon>();
        _dataList = new List<Battle.Weapon>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Battle.Weapon _v;
            _v = Battle.Weapon.DeserializeWeapon(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Battle.Weapon> DataMap => _dataMap;
    public List<Battle.Weapon> DataList => _dataList;

    public Battle.Weapon GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Battle.Weapon Get(int key) => _dataMap[key];
    public Battle.Weapon this[int key] => _dataMap[key];

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