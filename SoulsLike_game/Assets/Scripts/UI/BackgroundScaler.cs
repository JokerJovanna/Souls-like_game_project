using UnityEngine;

[ExecuteAlways] 
public class BackgroundScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("BackgroundScaler: требуется SpriteRenderer!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (mainCamera == null || spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        var spriteSize = spriteRenderer.sprite.bounds.size;
        if (spriteSize.x <= 0 || spriteSize.y <= 0) return;

        var cameraHeight = mainCamera.orthographicSize * 2f;
        var cameraWidth = cameraHeight * mainCamera.aspect;

        var scaleX = cameraWidth / spriteSize.x;
        var scaleY = cameraHeight / spriteSize.y;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        var camPos = mainCamera.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, 0f);
    }
}