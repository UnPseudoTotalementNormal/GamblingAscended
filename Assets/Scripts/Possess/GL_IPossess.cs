using GameEvents;
using UnityEngine;

namespace Possess
{
    public interface GL_IPossessable
    {
        public bool IsPossessed { get; }
        public GameEvent<GameEventInfo> OnPossessedEvent { get; }
        public GameEvent<GameEventInfo> OnUnpossessedEvent { get; }
        
        protected void OnPossess();

        protected void OnUnpossess();

        public void Possess()
        {
            if (IsPossessed) return;
            
            OnPossess();
        }
        public void Unpossess()
        {
            if (!IsPossessed) return;
            
            OnUnpossess();
        }
    }
}