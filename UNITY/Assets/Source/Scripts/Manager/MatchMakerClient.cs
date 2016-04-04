using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MatchMakerPacket;

public delegate void ConnectCallBack(Socket socket);
public delegate void DisconnectCallBack(Socket socket);
public delegate void ResponceCallBack(Socket socket, Packet p);

public class MatchMakerClient
{
    public static string ip = "";
    public static int port = 7776;
    public static int bufferSize = 1024;

    private static Socket socket;
    
    public static void StartMatchMaker()
    {
        new Thread(new ThreadStart(() =>
        {
            socket.Connect(IPAddress.Parse(ip), port);
            if (socket.Connected)
            {
                Packet p = new Packet(PacketType.RegisterPlayer, Settings.username);
                socket.Send(p.Tobytes());
                byte[] buffer = new byte[bufferSize];
                int rec = socket.Receive(buffer);
                if (rec > 0)
                {
                    Array.Resize(ref buffer, rec);

                }
            }
        })).Start();
    }
}