using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    private Block[,] allBlocks; // Nuestro mapa de la cuadrícula

    private void Awake()
    {
        instance = this;
        // Inicializamos el array con el tamaño de la cuadrícula
        allBlocks = new Block[gridWidth, gridHeight];
    }

    // grid settings
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 6;
    [SerializeField] private float cellSpacing = 1.1f; // distance between blocks

    [SerializeField] private GameObject blockPrefab; // prefab
    [SerializeField] private List<Sprite> blockSprites; // sprites list for blocks

    [SerializeField] private Transform gridContainer; // gridcontainer

    Vector2 startPosition;

    // --- FUNCIÓN DE INICIO ---
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // 1. Calculate the total size of the grid
        float totalGridWidth = (gridWidth - 1) * cellSpacing;
        float totalGridHeight = (gridHeight - 1) * cellSpacing;

        // 2. Find the real bottom-left corner by starting from the center and moving left/down
        startPosition = new Vector2(-totalGridWidth / 2f, -totalGridHeight / 2f);

        // 3. Loop through each cell
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate the final position for each block
                Vector2 spawnPosition = startPosition + new Vector2(x * cellSpacing, y * cellSpacing);

                GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, gridContainer);
                newBlock.transform.localPosition = spawnPosition; // Use localPosition for children


                Block blockScript = newBlock.GetComponent<Block>();
                blockScript.x = x;
                blockScript.y = y;
                allBlocks[x, y] = blockScript; // Esta línea ya la tenías, ahora usa la nueva variable


                // Guardamos la referencia del script del nuevo bloque en nuestro mapa
                allBlocks[x, y] = newBlock.GetComponent<Block>();

                int randomIndex = Random.Range(0, blockSprites.Count);
                newBlock.GetComponent<SpriteRenderer>().sprite = blockSprites[randomIndex];
            }
        }
    }

    public IEnumerator OnBlockClicked(Block block)
    {
        List<Block> connectedBlocks = FindConnectedBlocks(block);

        // La mayoría de juegos de este tipo requieren al menos 2 bloques para puntuar.
        // Si solo hay 1, no hacemos nada.
        if (connectedBlocks.Count < 2)
        {
            yield break;
        }

        // 1. Llama al GameManager para que actualice el puntaje y los movimientos
        GameManager.instance.AddScore(connectedBlocks.Count);
        GameManager.instance.UseMove();

        // 2. Itera sobre la lista de bloques conectados para destruirlos
        foreach (Block blockToDestroy in connectedBlocks)
        {
            // Marca la celda como vacía en nuestro mapa lógico
            allBlocks[blockToDestroy.x, blockToDestroy.y] = null;

            // Destruye el GameObject del bloque en la escena
            Destroy(blockToDestroy.gameObject);
        }

        // 1. Aquí pausamos la función por 1 segundo
        yield return new WaitForSeconds(1f);

        // 2. Aplicamos la gravedad
        ApplyGravity();

        // 3. Rellenamos los espacios vacíos de arriba
        RefillGrid();
    }

    private List<Block> FindConnectedBlocks(Block startBlock)
    {
        List<Block> connectedBlocks = new List<Block>();
        Queue<Block> blocksToProcess = new Queue<Block>();

        Sprite targetSprite = startBlock.GetComponent<SpriteRenderer>().sprite;

        blocksToProcess.Enqueue(startBlock);
        // Usamos una lista para saber cuáles ya hemos procesado y no repetirlos
        List<Block> processedBlocks = new List<Block>();
        processedBlocks.Add(startBlock);

        while (blocksToProcess.Count > 0)
        {
            Block currentBlock = blocksToProcess.Dequeue();
            connectedBlocks.Add(currentBlock);

            // Coordenadas del bloque actual
            int x = currentBlock.x;
            int y = currentBlock.y;

            // --- REVISAR VECINOS ---
            // Vecino de la Derecha (x+1)
            if (x + 1 < gridWidth) // ¿Está dentro de la cuadrícula?
            {
                Block rightNeighbor = allBlocks[x + 1, y];
                // ¿Es del mismo color y no lo hemos procesado ya?
                if (rightNeighbor != null && rightNeighbor.GetComponent<SpriteRenderer>().sprite == targetSprite && !processedBlocks.Contains(rightNeighbor))
                {
                    blocksToProcess.Enqueue(rightNeighbor);
                    processedBlocks.Add(rightNeighbor);
                }
            }

            // Vecino de la Izquierda (x-1)
            if (x - 1 >= 0)
            {
                Block leftNeighbor = allBlocks[x - 1, y];
                if (leftNeighbor != null && leftNeighbor.GetComponent<SpriteRenderer>().sprite == targetSprite && !processedBlocks.Contains(leftNeighbor))
                {
                    blocksToProcess.Enqueue(leftNeighbor);
                    processedBlocks.Add(leftNeighbor);
                }
            }

            // Vecino de Arriba (y+1)
            if (y + 1 < gridHeight)
            {
                Block topNeighbor = allBlocks[x, y + 1];
                if (topNeighbor != null && topNeighbor.GetComponent<SpriteRenderer>().sprite == targetSprite && !processedBlocks.Contains(topNeighbor))
                {
                    blocksToProcess.Enqueue(topNeighbor);
                    processedBlocks.Add(topNeighbor);
                }
            }

            // Vecino de Abajo (y-1)
            if (y - 1 >= 0)
            {
                Block bottomNeighbor = allBlocks[x, y - 1];
                if (bottomNeighbor != null && bottomNeighbor.GetComponent<SpriteRenderer>().sprite == targetSprite && !processedBlocks.Contains(bottomNeighbor))
                {
                    blocksToProcess.Enqueue(bottomNeighbor);
                    processedBlocks.Add(bottomNeighbor);
                }
            }
        }

        return connectedBlocks;
    }

    private void Update()
    {
        // Pointer.current es un puntero genérico (ratón, dedo, lápiz, etc.)
        if (Pointer.current == null) return;

        // "press" es la acción principal (clic izquierdo o toque en la pantalla)
        if (Pointer.current.press.wasPressedThisFrame)
        {
            // Obtenemos la posición en la pantalla desde el puntero genérico
            Vector2 screenPosition = Pointer.current.position.ReadValue();

            // El resto del código para lanzar el rayo es exactamente el mismo
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                //OnBlockClicked(hit.collider.gameObject.GetComponent<Block>());
                StartCoroutine(OnBlockClicked(hit.collider.gameObject.GetComponent<Block>()));
            }
        }
    }

    private void ApplyGravity()
    {
        // Recorremos cada columna
        for (int x = 0; x < gridWidth; x++)
        {
            // Empezamos desde abajo hacia arriba
            for (int y = 0; y < gridHeight; y++)
            {
                // Si encontramos un espacio vacío...
                if (allBlocks[x, y] == null)
                {
                    // ...buscamos el primer bloque que esté por encima.
                    for (int y2 = y + 1; y2 < gridHeight; y2++)
                    {
                        if (allBlocks[x, y2] != null)
                        {
                            // ¡Encontramos un bloque! Lo movemos hacia abajo.
                            Block blockToMove = allBlocks[x, y2];

                            // Actualizamos su posición en el mapa lógico
                            allBlocks[x, y] = blockToMove;
                            allBlocks[x, y2] = null;

                            // Actualizamos sus coordenadas internas
                            blockToMove.y = y;

                            // Movemos el GameObject a su nueva posición visual
                            Vector2 targetPosition = startPosition + new Vector2(x * cellSpacing, y * cellSpacing);
                            blockToMove.transform.localPosition = targetPosition;

                            // Salimos del bucle interior para seguir buscando espacios vacíos
                            break;
                        }
                    }
                }
            }
        }
    }

    private void RefillGrid()
    {
        // Recorremos la cuadrícula para encontrar los espacios que quedaron vacíos arriba
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (allBlocks[x, y] == null)
                {
                    // Si hay un espacio vacío, creamos un nuevo bloque
                    Vector2 spawnPosition = startPosition + new Vector2(x * cellSpacing, y * cellSpacing);
                    GameObject newBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity, gridContainer);
                    newBlock.transform.localPosition = spawnPosition;

                    int randomIndex = Random.Range(0, blockSprites.Count);
                    newBlock.GetComponent<SpriteRenderer>().sprite = blockSprites[randomIndex];

                    Block blockScript = newBlock.GetComponent<Block>();
                    blockScript.x = x;
                    blockScript.y = y;
                    allBlocks[x, y] = blockScript;
                }
            }
        }
    }
}
