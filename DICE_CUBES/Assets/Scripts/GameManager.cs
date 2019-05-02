using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartGame()
    {
        //If cannt move
        //EndGame()
    }

    public void ResolveTurn()
    {

    }

    public void StartTurn()
    {
        player.canJump = true;
        movesLeft = nbrOfMovesPerTurn;
    }

    public void Moved()
    {
        movesLeft--;
        if (movesLeft <= 0)
        {
            player.canJump = false;
            ResolveTurn();
        }
    }

    public void EndGame()
    {

    }
}
