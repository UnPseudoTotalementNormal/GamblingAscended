using Interactables;
using UnityEngine;

public class GL_InteractableDescription : MonoBehaviour, GL_IInteractableDescription
{
    [field:SerializeField] public string InteractionDescription { get; private set; }

    public string GetDescription() => InteractionDescription;
    
    public void SetDescription(string newDescription)
    {
        InteractionDescription = newDescription;
    }
}
