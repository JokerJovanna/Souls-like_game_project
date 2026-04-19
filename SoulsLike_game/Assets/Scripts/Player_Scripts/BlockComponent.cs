using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.E;

    private bool isBlocking = false;

    void Update()
    {
        if (Input.GetKey(blockKey))
        {
            isBlocking = true;
            Debug.Log("Block");
        }
        else isBlocking = false;
    }

    public bool IsBlocking => isBlocking;
}