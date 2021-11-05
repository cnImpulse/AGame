namespace SSRPG
{
    public static class EventName
    {
        public const string EnsureReward = "EnsureReward";

        #region 战斗

        public const string BattleUnitActionEnd = "BattleUnitActionEnd";
        public const string BattleUnitActionCancel = "BattleUnitActionCancel";
        public const string PointerDownGridMap  = "PointerDownGridMap";
        public const string GridUnitDamage      = "GridUnitDamage";

        #endregion
    }
}
