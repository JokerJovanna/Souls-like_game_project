using System;
using UnityEngine;

public class PlayerDetectorComponent : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private LayerMask obstacle;

    private bool wasVisible;
    
    public Action<GameObject> OnPlayerDetected;
    public Action OnPlayerLost;

    private GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && target == null)
        {
            target = other.gameObject;
            if (IsVisible())
            {
                wasVisible = true;
                OnPlayerDetected?.Invoke(target);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && other.gameObject == target)
        {
            var nowVisible = IsVisible();
            if (nowVisible && !wasVisible)
            {
                wasVisible = true;
                OnPlayerDetected?.Invoke(target);
            }
            else if (!nowVisible && wasVisible)
            {
                wasVisible = false;
                OnPlayerLost?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && other.gameObject == target)
        {
            target = null;
            wasVisible = false;
            OnPlayerLost?.Invoke();
        }
    }

    private bool IsVisible()
    {
        if (target == null) return false;

        var origin = transform.position;
        var direction = (target.transform.position - transform.position).normalized;
        var distance = Vector2.Distance(transform.position, target.transform.position);

        var hit = Physics2D.Raycast(origin, direction, distance, obstacle);
        return hit.collider == null;
    }
}
