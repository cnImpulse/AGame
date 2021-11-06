namespace SSRPG
{
    public static class EventName
    {
        public const string EnsureReward = "EnsureReward";

        #region 战斗

        public const string PointerDownGridMap      = "PointerDownGridMap";
        public const string GridUnitDead            = "GridUnitDead";
        public const string GridUnitDamage = "GridUnitDamage";
        public const string BattleUnitActionEnd     = "BattleUnitActionEnd";
        public const string BattleUnitActionCancel  = "BattleUnitActionCancel";

        #endregion
    }
}
