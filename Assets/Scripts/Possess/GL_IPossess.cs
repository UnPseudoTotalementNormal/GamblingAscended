using UnityEngine;

namespace Possess
{
    public interface GL_IPossessable
    {
        public bool IsPossessed { get; }
        
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