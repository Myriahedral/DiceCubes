using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up = -1, Down = 2, Left = -3, Right = 4 };

public class Grid : MonoBehaviour {
    public static Grid instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    [SerializeField] private int ySize;
    [SerializeField] private int xSize;
    [SerializeField] private float xUnit;
    [SerializeField] private float yUnit;

    [SerializeField] private GameObject dicePrefab;

    private Dice[,] diceGrid;

    private void InitializeGrid()
    {
        diceGrid = new Dice[xSize,ySize];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                SpawnDice(i, j);
            }
        }
    }

    private void Start()
    {
        InitializeGrid();
    }

    public Vector2Int GetNeighbour(int gridX, int gridY, Direction direction)
    {
        Vector2Int d = new Vector2Int(100,100);

        if (direction == Direction.Up || direction == Direction.Down)
        {
            int targetX = gridX + (int)Mathf.Sign((float)direction);
            if (targetX >= 0 && targetX < xSize)
            {
                d = new Vector2Int(targetX, gridY);
            }
        }
        else
        {
            int targetY = gridY + (int)Mathf.Sign((float)direction);
            if (targetY >= 0 && targetY < ySize)
            {
                d = new Vector2Int(gridX, targetY);
            }
        }

        return d;
    }

    public Dice GetDiceFromCoordinates(int gridX, int gridY)
    {
        if (gridX >= 0 && gridX < xSize && gridY >= 0 && gridY < ySize)
        {
            return diceGrid[gridX, gridY];
        }
        else
        {
            return null;
        }
        
    }

    public Vector2Int GetClosestCoordinates(Vector3 position)
    {
        float percentX = (position.x + xSize*xUnit/ 2) / xSize * xUnit;
        float percentY = (position.y + ySize*yUnit / 2) / ySize * yUnit;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((xSize - 1) * percentX);
        int y = Mathf.RoundToInt((xSize - 1) * percentY);

        return new Vector2Int(x,y);
    }
   
    public void SpawnDice(int gridX, int gridY)
    {
        Vector3 position = GetWorldCoordFromGrid(gridX, gridY);
        Dice d = Instantiate(dicePrefab, position, Quaternion.identity).GetComponent<Dice>();
        diceGrid[gridX,gridY] = d;
    }

    private Vector3 GetWorldCoordFromGrid(int gridX, int gridY)
    {
        //Calculation of the offset because the position of the spawner must be the center of the grid
        float xOffset = (float)(ySize - 1) / 2;
        float yOffset = (float)(xSize - 1) / 2;

        Vector3 worldPosition = new Vector3(transform.position.x + ((gridX - xOffset) * xUnit), transform.position.y, transform.position.z + ((gridY - yOffset) * yUnit));

        return worldPosition;
    }
}
