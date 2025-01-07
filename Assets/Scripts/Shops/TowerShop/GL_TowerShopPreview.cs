using System;
using UnityEngine;

public class GL_TowerShopPreview : MonoBehaviour
{
    private new Transform transform;
    
    [SerializeField] private float _rotationSpeed = 30;

    [SerializeField] private float _floatSpeed = 1f;
    [SerializeField] private float _floatHeight = 0.1f;
    
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.up * (Mathf.Sin(Time.time * _floatSpeed) * _floatHeight);
    }
}
