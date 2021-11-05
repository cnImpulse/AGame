using System;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public static class EventExtension
    {
        public static void Subscribe(this EventComponent eventComponent, string eventName, EventHandler<GameEventArgs> handler)
        {
            eventComponent.Subscribe(eventName.GetHashCode(), handler);
        }

        public static void Fire(this EventComponent eventComponent, object sender, string eventName, object userData = default)
        {
            eventComponent.Fire(sender, GameEventBase.Create(eventName, userData));
        }

        public static void Unsubscribe(this EventComponent eventComponent, string eventName, EventHandler<GameEventArgs> handler)
        {
            eventComponent.Unsubscribe(eventName.GetHashCode(), handler);
        }
    }
}
