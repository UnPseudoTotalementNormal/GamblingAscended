using System;
using GameEvents;
using GameEvents.Enum;
using Interactables;
using TMPro;
using UnityEngine;

public class GL_InteractHUD : MonoBehaviour
{
    [SerializeField] private GameEventEnum _interactEnterEvent = GameEventEnum.InteractPointerEnter;
    [SerializeField] private GameEventEnum _interactExitEvent = GameEventEnum.InteractPointerExit;

    [SerializeField] private TextMeshProUGUI _textFeedback;

    private void Awake()
    {
        _interactEnterEvent.AddListener(OnInteractEnter);
        _interactExitEvent.AddListener(OnInteractExit);
    }

    private void OnInteractEnter(GameEventInfo eventInfo)
    {
        if (!eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            return;
        }
        GameObject interactObject = gameEventGameObject.Value;
        
        _textFeedback.gameObject.SetActive(true);
        
        if (!interactObject.TryGetComponent(out GL_IInteractableDescription interactableDescription))
        {
            return;
        }
        
        _textFeedback.text = $"Appuie sur \"E\" {interactableDescription.InteractionDescription}";
    }
    
    private void OnInteractExit(GameEventInfo eventInfo)
    {
        _textFeedback.gameObject.SetActive(false);
    }
}
