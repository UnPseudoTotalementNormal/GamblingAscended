using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Enums;
using GameEvents;
using GameEvents.Enum;
using TMPro;
using UnityEngine;

namespace BattleField.DamageFeedback
{
    public class GL_DamageAmountTextFeedback : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] private Vector3 _spawnOffset;

        [SerializeField] private float _duration;

        [SerializeField] private float _textMoveSpeed = 1;

        [SerializeField] private float _minTextSize = 1;
        [SerializeField] private float _maxTextSize = 6;
        [SerializeField] private float _damageToMinTextSize = 1f;
        [SerializeField] private float _damageToMaxTextSize = 20f;
        
        [SerializeField] private SerializedDictionary<DamageType, Color> _damageTypeToColor = new()
        {
            { DamageType.Aucun, Color.white },
            { DamageType.Physique, Color.red },
            { DamageType.Distance, Color.yellow },
            { DamageType.Special, Color.green },
        };
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            GameEventEnum.OnDamageTaken.AddListener(OnDamageTaken);
        }

        private void OnDamageTaken(GameEventInfo eventInfo)
        {
            if (!eventInfo.TryTo(out GameEventDamage gameEventDamage))
            {
                return;
            }
            
            float damageAmount = gameEventDamage.Damage;

            GameObject newObject = new GameObject("damageText", 
                typeof(TextMeshPro), typeof(GL_Billboard), typeof(GL_DamageText));
            newObject.transform.SetParent(_transform);
            newObject.transform.position = gameEventDamage.Sender.transform.position + _spawnOffset;
            var newText = newObject.GetComponent<TextMeshPro>();
            newText.text = damageAmount % 1 == 0 ? ((int)damageAmount).ToString() : damageAmount.ToString("F");

            newText.fontSize = Mathf.LerpUnclamped(_minTextSize, _maxTextSize, damageAmount / _damageToMaxTextSize);
            
            newText.color = _damageTypeToColor[gameEventDamage.DamageType];
            
            newText.alignment = TextAlignmentOptions.Center;

            newObject.GetComponent<GL_DamageText>().MoveSpeed = _textMoveSpeed;

            newText.DOFade(0, _duration).SetEase(Ease.InQuint);
            Destroy(newObject, _duration);
        }
    }
}