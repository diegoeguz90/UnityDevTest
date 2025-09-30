using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResponsiveCamera : MonoBehaviour
{
    // reference to grid
    [SerializeField] private SpriteRenderer gridBackground;

    // block separation
    [SerializeField] private float margin = 1.0f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        if (gridBackground == null)
        {
            Debug.LogError("Error: No se ha asignado el GridBackground al script de la cámara.");
            return;
        }

        // Grid width
        float gridWidth = gridBackground.bounds.size.x + margin;

        // Screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;

        // Make the grid in the center of camera
        float newOrthographicSize = gridWidth / screenRatio / 2f;

        // Size for camera
        mainCamera.orthographicSize = newOrthographicSize;
    }
}
