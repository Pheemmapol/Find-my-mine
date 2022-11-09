using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        UIManager.instance.ChangeToLobbyUI();
        ClientSend.WelcomeReceived();
    }

    public static void GetClickPos(Packet _packet)
    {
        string _msg = _packet.ReadString();
        string[] pos =  _msg.Split(',');
        int x = int.Parse(pos[0]);
        int y = int.Parse(pos[1]);
        if (x == -1 && y == -1) return;
        TileManager.RevealTile(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), int.Parse(pos[2]) == 1 ? true:false);
        Debug.Log($"Message from server: {_msg}");

    }

    public static void GetGenericInfo(Packet _packet)
    {
        string _msg = _packet.ReadString();
        string[] info = _msg.Split(",");
        if (GameManager.Instance.State != GameManager.GameState.Waiting) {
            GameUIManager.instance.UpdateNameText(info[0], info[2]);
            GameUIManager.instance.UpdateScore(info[1], info[3]);
        }
        Debug.Log($"Message from server: {_msg}");
    }

    public static void GetState(Packet _packet)
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Message from server: {_msg}");
        string[] message = _msg.Split(',');
        switch (int.Parse(message[0]))
        {
            case (0):
                //reset board
                TileManager.ResetBoard();
                break;
            case (1):
                if(int.Parse(message[1]) == Client.instance.myId)
                {
                    //My turn
                    GameManager.Instance.ChangeState(GameManager.GameState.PlayerTurn);
                }
                else
                {
                    //Enemy Turn
                    GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);

                }
                GameUIManager.instance.UpdateTurn();
                break;
            case (2):
                GameManager.Instance.ChangeState(GameManager.GameState.Waiting);
                GameUIManager.instance.UpdateTurn();
                break;
            case (3):
                GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
                GameUIManager.instance.UpdateTurn();
                GameUIManager.instance.ShowGameOver(message[1]);
                break;
        }

    }
}
