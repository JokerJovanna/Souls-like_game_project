using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField] private Collider2D outerCollider;   
    [SerializeField] private Collider2D innerTrigger;    
    [SerializeField] private Transform destination;      
    [SerializeField] private GameObject areaToActivate;  
    [SerializeField] private GameObject areaToDeactivate;
    [SerializeField] private bool IsActiveAtStart = false;

    private void Start()
    {
        if (outerCollider != null)
            outerCollider.enabled = !IsActiveAtStart;
    }

    public void Activate()
    {
        if (outerCollider != null)
            outerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (outerCollider != null && outerCollider.enabled) return;

        other.transform.position = destination.position;

        if (areaToDeactivate != null) areaToDeactivate.SetActive(false);
        if (areaToActivate != null) areaToActivate.SetActive(true);
    }
}