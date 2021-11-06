
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;

namespace cfg.Scene
{
   
public sealed class TblScene
{
    private readonly Dictionary<int, Scene.SceneRes> _dataMap;
    private readonly List<Scene.SceneRes> _dataList;
    
    public TblScene(ByteBuf _buf)
    {
        _dataMap = new Dictionary<int, Scene.SceneRes>();
        _dataList = new List<Scene.SceneRes>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            Scene.SceneRes _v;
            _v = Scene.SceneRes.DeserializeSceneRes(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public Dictionary<int, Scene.SceneRes> DataMap => _dataMap;
    public List<Scene.SceneRes> DataList => _dataList;

    public Scene.SceneRes GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Scene.SceneRes Get(int key) => _dataMap[key];
    public Scene.SceneRes this[int key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }

}

}