using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public string Username;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ChangeState(GameState.Waiting);
    }
    public void ChangeState(GameState newstate)
    {
        State = newstate;
        switch (newstate) { 
            case (GameState.Waiting):
                break;
            case (GameState.PlayerTurn):
                break;
            case (GameState.EnemyTurn):
                break;
            }

    }
    public enum GameState
    {
        Waiting = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
    }
}
