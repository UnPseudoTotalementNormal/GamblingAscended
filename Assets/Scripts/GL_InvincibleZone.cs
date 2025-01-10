using System;
using Extensions;
using UnityEngine;

public class GL_InvincibleZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponentInParents<GL_Health>(out var health))
        {
            health.IsInvincible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponentInParents<GL_Health>(out var health))
        {
            health.IsInvincible = false;
        }
    }
}
