using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static void UpdateName(Packet _packet)
    {
        string _msg = _packet.ReadString();
        GameUIManager.instance.UpdateNameList(_msg);
    }

    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();
    }

    public static void GetClickPos(Packet _packet)
    {
        string _msg = _packet.ReadString();
        string[] pos =  _msg.Split(',');
        TileManager.RevealTile(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), int.Parse(pos[2]) == 1 ? true:false);
        Debug.Log($"Message from server: {_msg}");
        if (int.Parse(pos[3]) == Client.instance.myId)
        {
            GameUIManager.instance.UpdateScore(int.Parse(pos[4]));
        }


    }

    public static void GetGenericInfo(Packet _packet)
    {
        string _msg = _packet.ReadString();
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
        }

    }
}
