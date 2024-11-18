using UnityEngine;

namespace GameEvents
{
    public class GameEventInfo
    {
        public T To<T>() where T : GameEventInfo
        {
            if (this is T)
            {
                return (T)this;
            }

            Debug.LogWarning("Wrong game event info !!!");
            return null;
        }
        
        public bool TryTo<T>(out T result) where T : GameEventInfo
        {
            result = To<T>();
            return result != null;
        }
    }

    #region baseInfos
    
    public class GameEventVector2 : GameEventInfo
    {
        public Vector2 Value;
    }
    
    public class GameEventVector3 : GameEventInfo
    {
        public Vector3 Value;
    }
    
    public class GameEventFloat : GameEventInfo
    {
        public float Value;
    }
    
    public class GameEventInt : GameEventInfo
    {
        public int Value;
    }
    
    public class GameEventBool : GameEventInfo
    {
        public bool Value;
    }
    
    #endregion

    #region UnityInfos

    public class GameEventGameObject : GameEventInfo
    {
        public GameObject Value;
    }

    #endregion
}