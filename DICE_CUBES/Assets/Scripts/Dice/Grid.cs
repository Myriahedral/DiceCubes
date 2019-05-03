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

        for (int j = 0; j < ySize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                SpawnDice(i, j);
            }
        }
    }

    private void Start()
    {
        InitializeGrid();
        GameManager.instance.StartGame();
    }

    public Vector2Int GetNeighbour(int gridX, int gridY, Direction direction)
    {
        Vector2Int d = new Vector2Int(100,100);

        if (direction == Direction.Left || direction == Direction.Right)
        {
            int targetX = gridX + (int)Mathf.Sign((float)direction);
            if (targetX >= 0 && targetX < xSize)
            {
                d = new Vector2Int(targetX, gridY);
            }
        }
        else
        {
            int targetY = gridY - (int)Mathf.Sign((float)direction);
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
        float percentX = (position.x + (xSize-1)*xUnit/ 2) / ((xSize-1) * xUnit); 
        float percentY = (position.z + (ySize-1)*yUnit / 2) / ((ySize-1) * yUnit); 
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((xSize - 1) * percentX);
        int y = Mathf.RoundToInt((ySize - 1) * percentY);

        return new Vector2Int(x,y);
    }
   
    public void SpawnDice(int gridX, int gridY)
    {
        Vector3 position = GetWorldCoordFromGrid(gridX, gridY);
        Dice d = Instantiate(dicePrefab, position, Quaternion.identity).GetComponent<Dice>();
        d.coord = new Vector2Int(gridX, gridY);
        diceGrid[gridX,gridY] = d;
    }

    private Vector3 GetWorldCoordFromGrid(int gridX, int gridY)
    {
        //Calculation of the offset because the position of the spawner must be the center of the grid
        float xOffset = (float)(xSize - 1) / 2;
        float yOffset = (float)(ySize - 1) / 2;

        Vector3 worldPosition = new Vector3(transform.position.x + ((gridX - xOffset) * xUnit), transform.position.y, transform.position.z + ((gridY - yOffset) * yUnit));

        return worldPosition;
    }

    ///////////////////////////GAME RULES
    ///
    public IEnumerator CheckForPatterns()
    {
        for (int  i = 0;  i < xSize;  i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                CheckNeighbours(i, j);
                yield return null;
            }
        }
    }

    public void CheckNeighbours(int gridX, int gridY)
    {
        Dice currentDice = GetDiceFromCoordinates(gridX, gridY);
        Dice[] neighbours = new Dice[4];

        //Coordinates
        Vector2Int up = GetNeighbour(gridX, gridY, Direction.Up);
        Vector2Int down = GetNeighbour(gridX, gridY, Direction.Down);
        Vector2Int left = GetNeighbour(gridX, gridY, Direction.Left);
        Vector2Int right = GetNeighbour(gridX, gridY, Direction.Right);

        neighbours[0] = GetDiceFromCoordinates(up.x, up.y);
        neighbours[1] = GetDiceFromCoordinates(down.x, down.y);
        neighbours[2] = GetDiceFromCoordinates(left.x, left.y);
        neighbours[3] = GetDiceFromCoordinates(right.x, right.y);

        //Not linked to another dice
        if (currentDice.root == null)
        {
            foreach (Dice d in neighbours)
            {
                if (d != null && d.currentFace == currentDice.currentFace && d.root == null)
                {
                    currentDice.isRoot = true;
                    if (currentDice.otherDice.Contains(d) == false)
                    {
                        currentDice.otherDice.Add(d);
                    }
                    d.root = currentDice;
                }
            }
        }
        else //Already linked to another dice
        {
            foreach (Dice d in neighbours)
            {
                if (d != null && d.currentFace == currentDice.currentFace && d != currentDice.root && d.root == null)
                {
                    if (currentDice.root.otherDice.Contains(d) == false)
                    {
                        currentDice.root.otherDice.Add(d);
                    }
                    d.root = currentDice.root;
                }
            }
        }

        bool wonPoints = false;

        foreach (Dice d in diceGrid)
        {
            if (d.isRoot && d.otherDice.Count >= 2)
            {
                d.otherDice.Add(d);
                StartCoroutine(WinningCoroutine(d.otherDice.ToArray(), d.currentFace));
                wonPoints = true;
            }
        }

        if (!wonPoints)
        {
            GameManager.instance.StartTurn();
        }
    }

    public void ClearDice()
    {
        foreach (Dice d in diceGrid)
        {
            d.isRoot = false;
            d.root = null;
            d.otherDice = new List<Dice>();
        }
    }

    public IEnumerator WinningCoroutine(Dice[] diceGroup, int face)
    {
        foreach (Dice d in diceGroup)
        {
            d.anim.Play("Swirl");
        }

        int score = diceGroup.Length * (diceGroup.Length / 2);
        GameManager.instance.AddScore(score);

        yield return new WaitForSeconds(1f);

        foreach (Dice d in diceGroup)
        {
            Destroy(d.gameObject);
        }

        GameManager.instance.StartTurn();

        yield return null;
    }
}
