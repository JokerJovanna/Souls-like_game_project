using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (target && target.gameObject.activeInHierarchy)
        {
            transform.position = target.position + offset;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
