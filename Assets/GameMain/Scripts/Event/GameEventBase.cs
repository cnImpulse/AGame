using GameFramework;
using GameFramework.Event;

namespace SSRPG
{
    public class GameEventBase : GameEventArgs
    {
        private int m_Id = 0;

        public override int Id => m_Id;
        public string EventName { get; private set; }
        public object UserData { get; private set; }

        public static GameEventBase Create(string eventName, object userData = default)
        {
            GameEventBase e = ReferencePool.Acquire<GameEventBase>();
            e.m_Id = eventName.GetHashCode();
            e.EventName = eventName;
            e.UserData = userData;

            return e;
        }

        public override void Clear()
        {
            m_Id = 0;
            EventName = null;
            UserData = null;
        }
    }
}