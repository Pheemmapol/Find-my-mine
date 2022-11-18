using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public TMP_InputField usernameField;
    public TMP_Dropdown gameMode;
    public TMP_Dropdown bomb;
    public TMP_Dropdown superbomb;
    public TMP_Dropdown boardsize;

    public GameObject lobbyMenu;
    public GameObject createlobbymenu;
    public GameObject TutorialPanel;
    public bool ShowedTutorial = false;
    
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

    public void ToggleTutorial()
    {
        ShowedTutorial = !ShowedTutorial;
        TutorialPanel.SetActive(ShowedTutorial);
    }

    public void ConnectToServer()
    {
        usernameField.interactable = false;
        Client.username = usernameField.text;
        Client.instance.ConnectToServer();
    }
    public void ChangeToLobbyUI()
    {
        startMenu.SetActive(false);
        lobbyMenu.SetActive(true);
        createlobbymenu.SetActive(false);
    }

    public void ChangeToCreateLobby()
    {
        startMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        createlobbymenu.SetActive(true);
    }

    public void CreateLobby()
    {
        //TO DO customize width and height
        //ClientSend.CreateLobby(1, 0,width,height,bombcount,supermine,gamemode);
        UpdateBoardInfo();
        ClientSend.CreateLobby(1,0);
    }

    public void JoinLobby()
    {
        ClientSend.JoinLobby(0, 0);
    }

    public void ChangeScene(int scene)
    {
        Debug.Log("Changing scene");
        SceneManager.LoadScene(scene);
    }

    //option board setting
    public void UpdateBoardInfo()
    {
        Client.setBoardInfo(int.Parse(boardsize.captionText.text)
                                , int.Parse(boardsize.captionText.text)
                                , int.Parse(bomb.captionText.text)
                                , int.Parse(superbomb.captionText.text)
                                , gameMode.captionText.text);
    }

}
