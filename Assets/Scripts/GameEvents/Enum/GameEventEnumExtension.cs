using System;

namespace GameEvents.Enum
{
    public static class GameEventEnumExtension
    {
        public static void AddListener(this GameEventEnum gameEvent, Action<GameEventInfo> action)
        {
            GameEventHolder.GetGameEvent(gameEvent).AddListener(action);
        }
        
        public static void RemoveListener(this GameEventEnum gameEvent, Action<GameEventInfo> action)
        {
            GameEventHolder.GetGameEvent(gameEvent).RemoveListener(action);
        }
        
        public static void Invoke(this GameEventEnum gameEvent, GameEventInfo eventInfo)
        {
            GameEventHolder.GetGameEvent(gameEvent).Invoke(eventInfo);
        }
    }
}