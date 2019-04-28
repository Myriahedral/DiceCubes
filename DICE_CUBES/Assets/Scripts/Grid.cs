using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up = -1, Down = 2, Left = -3, Right = 4 };

public class Grid : MonoBehaviour {

    [SerializeField] private int columns;
    [SerializeField] private int rows;
    [SerializeField] private GameObject dicePrefab;

    private Dice[][] diceGrid;

    private void InitializeGrid()
    {

    }

    public Dice GetNeighbour(Vector2 gridCoordinates, Direction direction)
    {
        return diceGrid[0][0];
    }

    public Dice GetDiceFromCoordinates(Vector2 gridCoordinates)
    {
        return diceGrid[0][0];
    }

    public Vector3 GetPositionFromCoordinates(Vector2 gridCoordinates)
    {
        return Vector3.zero;
    }

}
