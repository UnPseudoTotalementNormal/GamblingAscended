using System;
using GameEvents;
using Interactables;
using TMPro;
using UnityEngine;

public class GL_InteractHUD : MonoBehaviour
{
    [SerializeField] private GameEvent<GameObject> _interactEnterEvent;
    [SerializeField] private GameEvent<GameObject> _interactExitEvent;

    [SerializeField] private TextMeshProUGUI _textFeedback;

    private void Awake()
    {
        _interactEnterEvent.AddListener(OnInteractEnter);
        _interactExitEvent.AddListener(OnInteractExit);
    }

    private void OnInteractEnter(int[] ids, GameObject interactObject)
    {
        _textFeedback.gameObject.SetActive(true);
        if (!interactObject.TryGetComponent(out GL_IInteractableDescription interactableDescription))
        {
            return;
        }
        
        _textFeedback.text = $"Appuie sur \"E\" {interactableDescription.InteractionDescription}";
    }
    
    private void OnInteractExit(int[] ids, GameObject interactObject)
    {
        _textFeedback.gameObject.SetActive(false);
    }
}
