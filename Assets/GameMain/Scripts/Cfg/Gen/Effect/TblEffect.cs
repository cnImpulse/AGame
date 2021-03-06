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
   
public partial class TblEffect
{
    private readonly Dictionary<int, Effect.EffectRes> _dataMap;
    private readonly List<Effect.EffectRes> _dataList;
    
    public TblEffect(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Effect.EffectRes>();
        _dataList = new List<Effect.EffectRes>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Effect.EffectRes _v;
            _v = Effect.EffectRes.DeserializeEffectRes(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<int, Effect.EffectRes> DataMap => _dataMap;
    public List<Effect.EffectRes> DataList => _dataList;

    public Effect.EffectRes GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Effect.EffectRes Get(int key) => _dataMap[key];
    public Effect.EffectRes this[int key] => _dataMap[key];

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