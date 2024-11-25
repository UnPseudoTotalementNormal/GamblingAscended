using GameEvents;
using UnityEngine;

namespace Interactables
{
    public interface GL_IInteract
    {
        public GameEventEnum InteractPointerEnterEvent { get; }
        public GameEventEnum InteractPointerExitEvent { get; }

        public GameEvent<GameEventInfo> InteractionEvent { get; }
    }
}