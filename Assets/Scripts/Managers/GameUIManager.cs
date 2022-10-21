using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public TextMeshProUGUI Name1;
    public TextMeshProUGUI Name2;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Turn;
    public GameObject GameOverScreen;
    public TextMeshProUGUI Winner;

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

    public void UpdateNameText(string name1,string name2)
    {
        Name1.text = name1;
        Name2.text = name2;
    }

    public void UpdateScore(int score)
    {
        Score.text = score.ToString();
    }
    public void UpdateTurn()
    {
        Turn.text = GameManager.Instance.State == GameManager.GameState.PlayerTurn ? "Your turn" : "Enemy's Turn";
    }

    public void ShowGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void HideGameOver()
    {
        GameOverScreen.SetActive(false);
    }
}
