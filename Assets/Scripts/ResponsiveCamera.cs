using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResponsiveCamera : MonoBehaviour
{
    // Arrastra aquí tu objeto GridBackground desde la escena
    [SerializeField] private SpriteRenderer gridBackground;

    // Un pequeño margen extra para que no quede pegado a los bordes
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

        // 1. Mide el ancho de la cuadrícula
        float gridWidth = gridBackground.bounds.size.x + margin;

        // 2. Calcula la proporción de la pantalla del dispositivo
        float screenRatio = (float)Screen.width / (float)Screen.height;

        // 3. Calcula el nuevo tamaño ortográfico
        // La fórmula asegura que el ancho de la cuadrícula quepa en el ancho de la pantalla
        float newOrthographicSize = gridWidth / screenRatio / 2f;

        // 4. Aplica el nuevo tamaño a la cámara
        mainCamera.orthographicSize = newOrthographicSize;
    }
}
