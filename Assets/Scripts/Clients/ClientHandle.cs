using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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

    public static void ErrorMesssage(Packet _packet)
    {
        int _error = _packet.ReadInt();
        switch (_error)
        {
            case 0:
                UIManager.instance.ShowError("Lobby not found, please try again.");
                break;
            case 1:
                break;
        }

    }

    //get click info data [posx,poy,isbomb] should be [posx,poy,tiletype,surroundingBomb]
    public static void GetClickPos(Packet _packet)
    {
        string _msg = _packet.ReadString();
        string[] pos =  _msg.Split(',');
        int x = int.Parse(pos[0]);
        int y = int.Parse(pos[1]);
        if (x == -1 && y == -1) return;
        if (pos.Length <= 3)
        {
            TileManager.RevealTile(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), int.Parse(pos[2]));
        }
        else
        {
            TileManager.RevealTile(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), int.Parse(pos[2]), int.Parse(pos[3]));
        }
        Debug.Log($"Message from server: {_msg}");

    }

    //get info data [name1,score1,name2,score2,char1,char2]
    public static void GetGenericInfo(Packet _packet)
    {
        string _msg = _packet.ReadString();
        string[] info = _msg.Split(",");
        if (GameManager.Instance.State != GameManager.GameState.Waiting) {
            GameUIManager.instance.UpdateNameText(info[0], info[2]);
            GameUIManager.instance.UpdateScore(info[1], info[3]);
            GameUIManager.instance.UpdateCharacter(int.Parse(info[4]),int.Parse(info[5]));
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
            case (-1):
                GameUIManager.instance.BackToMenu();
                break;
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
                // 4,{lobbyid},{width},{height},{mine},{supermine},{(int)gamemode}
                Client.setBoardInfo(int.Parse(message[2]), int.Parse(message[3]), 
                                    int.Parse(message[4]), int.Parse(message[5]), 
                                    int.Parse(message[6]), int.Parse(message[1]));
                UIManager.instance.ChangeScene(1);

                break;
        }
    }
        public static void GetChat(Packet _packet)
        {
            string _msg = _packet.ReadString();
        GameManager.Instance.SendMessageToChat(_msg,Message.MessageType.playerMessage);
        }

}
