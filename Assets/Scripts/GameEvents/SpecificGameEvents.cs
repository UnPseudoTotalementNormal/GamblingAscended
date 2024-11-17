using System;
using UnityEngine;

namespace GameEvent
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/BasicTypes/GameEvent_bool")]
    public class GameEventBool : GameEvent<bool> {}
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/BasicTypes/GameEvent_float")]
    public class GameEventFloat : GameEvent<float> {}
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/BasicTypes/GameEvent_int")]
    public class GameEventInt : GameEvent<int> {}
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/UnityTypes/GameEvent_GameObject")]
    public class GameEventGameObject : GameEvent<GameObject> {}
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/Vectors/GameEvent_Vector2")]
    public class GameEventVector2 : GameEvent<Vector2> {}
    
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/T1/Vectors/GameEvent_Vector3")]
    public class GameEventVector3 : GameEvent<Vector3> {}
}