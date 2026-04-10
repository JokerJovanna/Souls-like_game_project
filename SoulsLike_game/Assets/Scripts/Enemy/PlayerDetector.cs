using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private string TargetTag = "Player";

    private Action<GameObject> OnPlayerDetected;
    private Action<GameObject> OnPlayerLost;

    private GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TargetTag) && target == null)
        {
            target = other.gameObject;
            OnPlayerDetected?.Invoke(target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TargetTag) && other.gameObject == target)
        {
            target = null;
            OnPlayerLost?.Invoke(target);
        }
    }
}
