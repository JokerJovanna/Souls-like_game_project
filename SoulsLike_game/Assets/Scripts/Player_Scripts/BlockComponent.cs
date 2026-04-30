using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.L;
    private bool isBlocking = false;

    void Update()
    {
        isBlocking = Input.GetKey(blockKey);
    }

    public bool IsBlocking => isBlocking;
}