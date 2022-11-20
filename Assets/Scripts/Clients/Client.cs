using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;
    public string ip = "192.168.75.156";
    public int port = 26950;
    public int myId = 0;
    public static string username = "";
    public TCP tcp;
    public static BoardInfo boardinfo;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;
    
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
    }

    public void ConnectToServer()
    {
        InitializeClientData();

        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconnect
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                // TODO: disconnect
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.error, ClientHandle.ErrorMesssage },
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.clickpos, ClientHandle.GetClickPos},
            {(int)ServerPackets.genericinfo, ClientHandle.GetGenericInfo },
            {(int) ServerPackets.state, ClientHandle.GetState },
            {(int) ServerPackets.lobby, null },
            {(int) ServerPackets.chat, ClientHandle.GetChat }
        };
        Debug.Log("Initialized packets.");
    }

    public class BoardInfo
    {
        public int width;
        public int height;
        public int bomb;
        public int superbomb;
        public int gamemode;
        public int lobbyid;
        public BoardInfo()
        {
            width = 6;
            height = 6;
        }
        public BoardInfo(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        public BoardInfo(int width, int height, int bomb, int superbomb, int gamemode, int lobbyid) : this(width, height)
        {
            this.bomb = bomb;
            this.superbomb = superbomb;
            this.gamemode = gamemode;
            this.lobbyid = lobbyid;
        }
    }

    //gamemode 1-normal 2-minesweeper 3-reverse 4-battleship
    public static void setBoardInfo(int width, int height, int bomb, int superbomb, string gamemode,int lobbyid)
    {
        int gamemodeint = 0;
        switch (gamemode)
        {
            case "Normal":
                break;
            case "Mine Sweeper":
                gamemodeint = 1;
                break;
            case "Reversed":
                gamemodeint = 2;
                break;
            case "Battleship":
                gamemodeint = 3;
                break;
        }
        boardinfo = new BoardInfo(width, height, bomb, superbomb, gamemodeint,lobbyid);
    }

}
