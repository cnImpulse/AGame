//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace Cfg.Effect
{
   
public partial class TblGridMapEffect
{
    private readonly Dictionary<int, Effect.GridMapEffectRes> _dataMap;
    private readonly List<Effect.GridMapEffectRes> _dataList;
    
    public TblGridMapEffect(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Effect.GridMapEffectRes>();
        _dataList = new List<Effect.GridMapEffectRes>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Effect.GridMapEffectRes _v;
            _v = Effect.GridMapEffectRes.DeserializeGridMapEffectRes(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Effect.GridMapEffectRes> DataMap => _dataMap;
    public List<Effect.GridMapEffectRes> DataList => _dataList;

    public Effect.GridMapEffectRes GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Effect.GridMapEffectRes Get(int key) => _dataMap[key];
    public Effect.GridMapEffectRes this[int key] => _dataMap[key];

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