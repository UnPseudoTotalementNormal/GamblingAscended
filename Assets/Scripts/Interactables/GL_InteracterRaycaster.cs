using GameEvents;
using UnityEngine;

namespace Interactables
{
    public class GL_InteracterRaycaster : MonoBehaviour, GL_IInteract
    {
        [field:SerializeField] public GameEvent<GameObject> InteractPointerEnterEvent { get; private set; }
        [field:SerializeField] public GameEvent<GameObject> InteractPointerExitEvent { get; private set; }
        [field:SerializeField] public GameEvent InteractionEvent { get; private set; }
    }
}