﻿using System.Collections;
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

    public void StartGame()
    {

    }

    public void ResolveTurn()
    {

    }

    public void StartTurn()
    {

    }

    public void EndGame(bool won)
    {

    }
}