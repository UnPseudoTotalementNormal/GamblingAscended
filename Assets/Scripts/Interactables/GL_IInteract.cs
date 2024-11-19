using GameEvents;
using UnityEngine;

namespace Interactables
{
    public interface GL_IInteract
    {
        public GameEvent<GameEventInfo> InteractPointerEnterEvent { get; }
        public GameEvent<GameEventInfo> InteractPointerExitEvent { get; }

        public GameEvent<GameEventInfo> InteractionEvent { get; }
    }
}