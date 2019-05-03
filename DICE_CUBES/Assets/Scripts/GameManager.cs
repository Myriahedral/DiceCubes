using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    private int movesLeft;

    [SerializeField] private MovementScript player;
    [SerializeField] private int nbrOfMovesPerTurn;
    private int score;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI movesLeftUI;
    [SerializeField] private TextMeshProUGUI scoreUI;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        scoreUI.text = 0.ToString();
        score = 0;
        movesLeftUI.text = nbrOfMovesPerTurn.ToString();
        StartTurn();
    }

    public void ResolveTurn()
    {

    }

    public void StartTurn()
    {
        //If cannt move
        //EndGame()

        player.SetTurnOver(false);
        print("turn starts");
        //Init nbr of jumps
        movesLeft = nbrOfMovesPerTurn;
        movesLeftUI.text = movesLeft.ToString();

    }

    public void Moved()
    {
        movesLeft--;
        movesLeftUI.text = movesLeft.ToString();

        if (movesLeft <= 0)
        {
            player.SetTurnOver(true);
            print("stop");
            ResolveTurn();
        }
    }

    public void EndGame()
    {

    }
}
