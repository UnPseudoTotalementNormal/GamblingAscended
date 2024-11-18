using GameEvents;
using UnityEngine;

namespace Interactables
{
    public interface GL_IInteract
    {
        public GameEvent<GameObject> InteractPointerEnterEvent { get; }
        public GameEvent<GameObject> InteractPointerExitEvent { get; }

        public GameEvent<GameObject> InteractionEvent { get; }
    }
}