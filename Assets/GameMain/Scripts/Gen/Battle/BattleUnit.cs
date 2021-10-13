
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace cfg.Battle
{

/// <summary>
/// 
/// </summary>
public sealed partial class BattleUnit :  Bright.Config.BeanBase 
{
    public BattleUnit(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        Name = _buf.ReadString();
        Desc = _buf.ReadString();
        MaxHP = _buf.ReadInt();
        MaxMP = _buf.ReadInt();
        ATK = _buf.ReadInt();
        AtkRange = _buf.ReadInt();
        MOV = _buf.ReadInt();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);SkillList = new System.Collections.Generic.List<int>(n);for(var i = 0 ; i < n ; i++) { int _e;  _e = _buf.ReadInt(); SkillList.Add(_e);}}
    }

    public static BattleUnit DeserializeBattleUnit(ByteBuf _buf)
    {
        return new Battle.BattleUnit(_buf);
    }

    /// <summary>
    /// 战斗单位编号
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 战斗单名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 战斗单位简介
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaxHP { get; private set; }
    /// <summary>
    /// 最大法力值
    /// </summary>
    public int MaxMP { get; private set; }
    /// <summary>
    /// 攻击力
    /// </summary>
    public int ATK { get; private set; }
    /// <summary>
    /// 攻击范围
    /// </summary>
    public int AtkRange { get; private set; }
    /// <summary>
    /// 移动力
    /// </summary>
    public int MOV { get; private set; }
    /// <summary>
    /// 技能
    /// </summary>
    public System.Collections.Generic.List<int> SkillList { get; private set; }

    public const int ID = -1364440750;
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
        + "Desc:" + Desc + ","
        + "MaxHP:" + MaxHP + ","
        + "MaxMP:" + MaxMP + ","
        + "ATK:" + ATK + ","
        + "AtkRange:" + AtkRange + ","
        + "MOV:" + MOV + ","
        + "SkillList:" + Bright.Common.StringUtil.CollectionToString(SkillList) + ","
        + "}";
    }
    }

}