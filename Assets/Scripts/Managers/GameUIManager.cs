using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public TextMeshProUGUI Name1;
    public TextMeshProUGUI Name2;
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;
    public TextMeshProUGUI Turn;
    public GameObject GameOverScreen;
    public TextMeshProUGUI Winner;
    public GameObject WaitingScreen;
    public TextMeshProUGUI playerWaitText;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

    }

    private void RestartGame()
    {

    }

    private void Start()
    {
        UpdateWaitingScreen();
        UpdateTurn();
    }

    public void UpdateNameText(string name1,string name2)
    {
        Name1.text = name1;
        Name2.text = name2;
    }

    public void UpdateScore(string score1,string score2)
    {
        Score1.text = score1;
        Score2.text = score2;
    }
    public void UpdateTurn()
    {
        string text  = "Waiting...";
        switch (GameManager.Instance.State)
        {
            case (GameManager.GameState.Waiting):
                text = "Waiting for another player.";
                break;
            case (GameManager.GameState.PlayerTurn):
                text = "Your turn";
                break;
            case (GameManager.GameState.EnemyTurn):
                text = "Enemy's Turn";
                break;
            case (GameManager.GameState.GameOver):
                text = "Game Over!";
                break;
        }
        Turn.text = text;
    }

    public void UpdateWaitingScreen()
    {
        if (GameManager.Instance.State == GameManager.GameState.Waiting)
        {
            WaitingScreen.SetActive(true);
            playerWaitText.text = $"Hello {Client.instance.name}! Welcome to the game.";
        }
    }
    public void HideWaitingScreen()
    {
        WaitingScreen.SetActive(false);
    }
    public void ShowGameOver(string name)
    {
        Winner.text = name+" wins!";
        GameOverScreen.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverScreen.SetActive(false);
    }
}
