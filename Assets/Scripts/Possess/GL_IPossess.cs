using UnityEngine;

namespace Possess
{
    public interface GL_IPossessable
    {
        protected void OnPossess();

        protected void OnUnpossess();

        public void Possess() => OnPossess();
        public void Unpossess() => OnUnpossess();
    }
}