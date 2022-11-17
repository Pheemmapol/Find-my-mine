using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void SendClickPos(int x,int y)
    {
        using (Packet _packet = new Packet((int)ClientPackets.clickpos))
        {
            _packet.Write(x+","+y);

            SendTCPData(_packet);
        }
    }

    // 
    public static void JoinLobby(int create, int lobbyid)
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobby))
        {
            _packet.Write(create);

            SendTCPData(_packet);
        }
    }

    public static void CreateLobby(int create, int lobbyid,int width,int height,int bombcount, int supermmine,int gamemode)
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobby))
        {
            _packet.Write(create);

            SendTCPData(_packet);
        }
    }
    public static void SendState(int state)
    {
        using (Packet _packet = new Packet((int)ClientPackets.state))
        {
            _packet.Write(state);

            SendTCPData(_packet);
        }
    }

    public static void SendChat(string message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.chat))
        {
            _packet.Write(message);

            SendTCPData(_packet);
        }

    }
    #endregion
}
