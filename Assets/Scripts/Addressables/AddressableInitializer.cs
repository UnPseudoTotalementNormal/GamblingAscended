using System;
using Breezorio;
using UnityEngine;

public class AddressableInitializer : MonoBehaviour
{
    private void Awake()
    {
        _ = AddressablesManager.Initialize();
    }
}