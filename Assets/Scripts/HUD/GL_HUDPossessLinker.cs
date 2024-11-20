using GameEvents;
using Possess;
using UnityEngine;
using UnityEngine.Events;

public class GL_HUDPossessLinker : MonoBehaviour, GL_IPossessable
{
    public bool IsPossessed { get; private set; }
    [field:SerializeField] public GameEvent<GameEventInfo> OnPossessedEvent { get; private set;}
    [field:SerializeField] public GameEvent<GameEventInfo> OnUnpossessedEvent { get; private set;}

    public UnityEvent<GameObject> OnPossessed, OnUnpossessed;
    
    void GL_IPossessable.OnPossess()
    {
        OnPossessedEvent?.Invoke(new GameEventGameObject {Value = gameObject});
        IsPossessed = true;
    }

    void GL_IPossessable.OnUnpossess()
    {
        OnUnpossessedEvent?.Invoke(new GameEventGameObject {Value = gameObject});
        IsPossessed = false;
    }
}
