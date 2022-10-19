using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static void UpdateName(Packet _packet)
    {
        string _msg = _packet.ReadString();
        GameUIManager.instance.UpdateName(_msg);
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


    }
}
