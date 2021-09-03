namespace SSRPG
{
    /// <summary>
    /// 网格地形类型
    /// </summary>
    public enum GridType
    {
        None,
        Normal,
        Wall,
    }

    /// <summary>
    /// 网格单位类型
    /// </summary>
    public enum GridUnitType
    {
        None,
        BattleUnit,
        BirthPlace,
    }

    /// <summary>
    /// 战斗阵营类型
    /// </summary>
    public enum CampType
    {
        None,
        Player,
        Enemy,
        Neutral,
    }
}