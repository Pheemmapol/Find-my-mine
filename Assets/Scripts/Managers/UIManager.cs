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

    public GameObject lobbyMenu;

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

    public void ConnectToServer()
    {
        usernameField.interactable = false;
        Client.instance.name = usernameField.text;
        Client.instance.ConnectToServer();
    }
    public void ChangeToLobbyUI()
    {
        startMenu.SetActive(false);
        lobbyMenu.SetActive(true);
    }
    public void CreateLobby()
    {
        ClientSend.JoinLobby(true,0);
        ChangeScene(1);
    }

    public void JoinLobby()
    {
        ClientSend.JoinLobby(false, 0);
        ChangeScene(1);
    }

    public void ChangeScene(int scene)
    {
        Debug.Log("Changing scene");
        SceneManager.LoadScene(scene);
    }
}
