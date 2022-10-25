using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class GameManager : MonoBehaviour
{
    
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;

    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    public static GameManager Instance;
    public GameState State = GameState.Waiting;
    public string Username;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ChangeState(GameState.Waiting);
    }

    private void Update()
    {
        if (!chatBox.isFocused)
        {
            if(chatBox.text != "")
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SendMessageToChat(Username +": "+ chatBox.text, Message.MessageType.playerMessage);
                    chatBox.text = "";
                }
            }
            else
            {
                if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
                    chatBox.ActivateInputField();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed the space key!", Message.MessageType.info);
                Debug.Log("Space");
            }
        }
        
    }
    public void ChangeState(GameState newstate)
    {
        if(State == GameState.Waiting && newstate != GameState.Waiting)
        {
            GameUIManager.instance.HideWaitingScreen();
        }

        State = newstate;
        switch (newstate) { 
            case (GameState.Waiting):
                break;
            case (GameState.PlayerTurn):
                break;
            case (GameState.EnemyTurn):
                break;
            case (GameState.GameOver):
                break;
            }

    }
    public enum GameState
    {
        Waiting = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        GameOver= 3
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
        
    }
}
