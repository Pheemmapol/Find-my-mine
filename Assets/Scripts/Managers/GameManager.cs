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
    public float timer = 10.0f;
    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;
    public ScrollRect scrollRect;

    public Color playerMessage, info;

    public TextMeshProUGUI TimerText;

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
                    var text = Username + ": " + chatBox.text;
                    SendMessageToChat(text, Message.MessageType.playerMessage);
                    ClientSend.SendChat(text);
                    chatBox.text = "";
                }
            }
            else
            {
                if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
                    chatBox.ActivateInputField();
            }

        }
        if (Instance.State == GameState.PlayerTurn)
        {
            UpdateTime();
        }
        if(timer < 0)
        {
            Debug.Log("Out of time");
            ClientSend.SendClickPos(-1, -1);
            ChangeState(GameState.EnemyTurn);
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
                ResetTime();
                break;
            case (GameState.EnemyTurn):
                ResetTime();
                break;
            case (GameState.GameOver):
                break;
            }

    }


    public void UpdateTime()
    {
        timer -= Time.deltaTime;
        TimerText.text = (Mathf.Round(timer * 100f) / 100f).ToString();
    }

    public void ResetTime()
    {
        timer = 10.0f;
        TimerText.text = timer.ToString();
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

        scrollRect.normalizedPosition = new Vector2(0, 0);


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
