using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up = -1, Down = 2, Left = -3, Right = 4 };

public class Grid : MonoBehaviour {

    [SerializeField] private int columns;
    [SerializeField] private int rows;
    [SerializeField] private float xUnit;
    [SerializeField] private float yUnit;

    [SerializeField] private GameObject dicePrefab;

    private Dice[,] diceGrid;

    private void InitializeGrid()
    {
        diceGrid = new Dice[rows,columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                SpawnDice(i, j);
            }
        }
    }

    private void Start()
    {
        InitializeGrid();
    }

    public Dice GetNeighbour(int gridX, int gridY, Direction direction)
    {
        Dice d = null;

        if (direction == Direction.Up || direction == Direction.Down)
        {
            int targetX = gridX + (int)Mathf.Sign((float)direction);
            if (targetX >= 0 && targetX < rows)
            {
                d = diceGrid[targetX, gridY];
            }
        }
        else
        {
            int targetY = gridY + (int)Mathf.Sign((float)direction);
            if (targetY >= 0 && targetY < columns)
            {
                d = diceGrid[gridX, targetY];
            }
        }

        return d;
    }

    public Dice GetDiceFromCoordinates(int gridX, int gridY)
    {
        return diceGrid[gridX,gridY];
    }

    public Dice GetClosestDice(Vector3 position)
    {
        return diceGrid[0, 0];
    }
   
    private void SpawnDice(int gridX, int gridY)
    {
        Vector3 position = GetWorldCoordFromGrid(gridX, gridY);
        Dice d = Instantiate(dicePrefab, position, Quaternion.identity).GetComponent<Dice>();
        diceGrid[gridX,gridY] = d;
    }

    private Vector3 GetWorldCoordFromGrid(int gridX, int gridY)
    {
        //Calculation of the offset because the position of the spawner must be the center of the grid
        float xOffset = (float)(columns - 1) / 2;
        float yOffset = (float)(rows - 1) / 2;

        Vector3 worldPosition = new Vector3(transform.position.x + ((gridX - xOffset) * xUnit), transform.position.y, transform.position.z + ((gridY - yOffset) * yUnit));

        return worldPosition;
    }
}
