using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleField.DamageFeedback
{
    public class GL_DamageText : MonoBehaviour
    {
        public Vector3 Direction = Vector3.up;
        public float MoveSpeed = 1f;
        
        private void Update()
        {
            transform.Translate(Direction * (MoveSpeed * Time.deltaTime));
        }
    }
}