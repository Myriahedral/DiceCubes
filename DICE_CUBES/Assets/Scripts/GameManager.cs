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

    public void StartGame()
    {
        scoreUI.text = 0.ToString();
        score = 0;
        movesLeftUI.text = nbrOfMovesPerTurn.ToString();
        StartTurn();
    }

    public void ResolveTurn()
    {
        StartCoroutine(Grid.instance.CheckForPatterns());
    }

    public void StartTurn()
    {
        //If cannt move
        //EndGame()

        //Reset
        Grid.instance.ClearDice();
        player.SetTurnOver(false);

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
            Invoke("ResolveTurn", 1f);
        }
    }

    public void AddScore(int score)
    {
        score += score;
        scoreUI.text = score.ToString();
    }

    public void EndGame()
    {

    }
}
