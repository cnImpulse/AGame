namespace SSRPG
{
    /// <summary>
    /// 网格地形类型
    /// </summary>
    public enum GridType
    {
        None,
        Land,
        Obstacle,
    }

    /// <summary>
    /// 网格单位类型
    /// </summary>
    public enum GridUnitType
    {
        None,
        BattleUnit,
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

    /// <summary>
    /// 行动指令类型
    /// </summary>
    public enum ActionType
    {
        None,
        Attack = 1, // 攻击
        Await = 2,  // 待机
        Skill = 3,  // 释放技能
    }

    /// <summary>
    /// 邻居网格类型
    /// </summary>
    public enum NeighborType
    {
        None,
        CanAcross,
        CanArrive,
        CanAttack,
    }
}