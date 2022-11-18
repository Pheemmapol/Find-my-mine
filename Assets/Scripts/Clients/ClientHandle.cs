using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static Client.BoardInfo boardInfo;
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        UIManager.instance.ChangeToLobbyUI();
        ClientSend.WelcomeReceived();
    }

    //get click info data [posx,poy,isbomb] should be [posx,poy,tiletype,surroundingBomb]
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

    //get info data [name1,score1,name2,score2]
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


    /* Get state
     *  0 -> reset board    data [0]
     *  1 -> Change turn    data [1,turns]
     *  2 -> Waiting        data [2]
     *  3 -> Game over      data [3,winner]
     *  4 -> Into game scene data [4,width,height]
     */

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
                if (int.Parse(message[1]) == Client.instance.myId)
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
            case (4):
                //put to game scene
                UIManager.instance.ChangeScene(1);
                boardInfo = new Client.BoardInfo(int.Parse(message[1]), int.Parse(message[2]), 11);
                break;
        }
    }
        public static void GetChat(Packet _packet)
        {
            string _msg = _packet.ReadString();
        GameManager.Instance.SendMessageToChat(_msg,Message.MessageType.playerMessage);
        }
}
