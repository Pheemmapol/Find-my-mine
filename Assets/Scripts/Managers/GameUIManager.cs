using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public TextMeshProUGUI NameList;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Turn;

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

    public void UpdateNameList(string newname)
    {
        NameList.text = newname;
    }

    public void UpdateScore(int score)
    {
        Score.text = score.ToString();
    }
    public void UpdateTurn()
    {
        Turn.text = GameManager.Instance.State == GameManager.GameState.PlayerTurn ? "Your turn" : "Enemy's Turn";
    }
}
