using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResponsiveCamera : MonoBehaviour
{
    // Arrastra aqu� tu objeto GridBackground desde la escena
    [SerializeField] private SpriteRenderer gridBackground;

    // Un peque�o margen extra para que no quede pegado a los bordes
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
            Debug.LogError("Error: No se ha asignado el GridBackground al script de la c�mara.");
            return;
        }

        // 1. Mide el ancho de la cuadr�cula
        float gridWidth = gridBackground.bounds.size.x + margin;

        // 2. Calcula la proporci�n de la pantalla del dispositivo
        float screenRatio = (float)Screen.width / (float)Screen.height;

        // 3. Calcula el nuevo tama�o ortogr�fico
        // La f�rmula asegura que el ancho de la cuadr�cula quepa en el ancho de la pantalla
        float newOrthographicSize = gridWidth / screenRatio / 2f;

        // 4. Aplica el nuevo tama�o a la c�mara
        mainCamera.orthographicSize = newOrthographicSize;
    }
}
