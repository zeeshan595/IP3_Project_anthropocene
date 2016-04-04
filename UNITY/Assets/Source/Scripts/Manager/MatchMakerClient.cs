using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MatchMakerPacket;
using System.Collections.Generic;

public delegate void ConnectCallback(Socket socket);
public delegate void DisconnectCallback(Socket socket);
public delegate void ListRoomsCallback(List<Room> rooms);

public class MatchMakerClient
{
    public static string ip = "213.107.103.102";
    public static int port = 7776;
    public static int bufferSize = 1024;
    public static ConnectCallback connectCallback;
    public static DisconnectCallback disconnectCallback;
    public static ListRoomsCallback listRoomsCallback;

    private static Socket socket;
    private static Thread pingpong;
    
    public static void StartMatchMaker()
    {
        new Thread(new ThreadStart(() =>
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPAddress.Parse(ip), port);
                Packet p = new Packet(PacketType.RegisterPlayer, Settings.username);
                socket.Send(p.Tobytes());
                byte[] buffer = new byte[bufferSize];
                int rec = socket.Receive(buffer);
                if (rec > 0)
                {
                    Array.Resize(ref buffer, rec);
                    p = new Packet(buffer);
                    if (p.message != "[CONFIRMED]")
                    {
                        throw new Exception("Server did not confirm player registeration");
                    }
                    else
                    {
                        connectCallback(socket);
                    }
                }
                else
                    throw new Exception("Got no responce from server");
            }
            catch (Exception)
            {
                pingpong.Abort();
                pingpong = null;
                socket.Disconnect(false);
                socket.Close();
                socket = null;
                throw new Exception("Couldn't register player");
            }
        })).Start();

        pingpong = new Thread(new ThreadStart(pingPongThread));
        //pingpong.Start();
    }

    private static void pingPongThread()
    {
        while (true)
        {
            byte[] buffer = new byte[bufferSize];
            int rec = socket.Receive(buffer);
            if (rec > 0)
            {
                Array.Resize(ref buffer, rec);
                Packet p = new Packet(buffer);
                if (p.type == PacketType.PingPong)
                {
                    socket.Send(new Packet(PacketType.PingPong, Settings.username).Tobytes());
                }
            }
        }
    }

    public static void StopMatchMaker()
    {
        pingpong.Abort();
        pingpong = null;
        socket.Disconnect(false);
        socket.Close();
        socket = null;
    }

    public static void CreateRoom(string name, string password, int maxPlayers)
    {
        if (!socket.Connected)
        {
            throw new Exception("You are not connected to any server");
        }

        new Thread(new ThreadStart(() =>
        {
            Room r = new Room(Settings.username, name, maxPlayers);
            r.password = password;
            Packet p = new Packet(PacketType.CreateRoom, Settings.username);
            p.roomList.Add(r);
            socket.Send(p.Tobytes());

            byte[] buffer = new byte[bufferSize];
            int rec = socket.Receive(buffer);
            if (rec > 0)
            {
                Array.Resize(ref buffer, rec);
                p = new Packet(buffer);
                if (p.message != "[ROOM CREATED]")
                {
                    throw new Exception("Room could not be created " + p.message);
                }
            }
            else
                throw new Exception("Got no responce from server");
        })).Start();
    }

    public static void ListRoom()
    {
        if (!socket.Connected)
        {
            throw new Exception("You are not connected to any server");
        }

        new Thread(new ThreadStart(() =>
        {
            Packet p = new Packet(PacketType.ListRooms, Settings.username);
            socket.Send(p.Tobytes());

            byte[] buffer = new byte[bufferSize];
            int rec = socket.Receive(buffer);
            if (rec > 0)
            {
                Array.Resize(ref buffer, rec);
                p = new Packet(buffer);
                if (p.message != "[ROOM LIST]")
                {
                    throw new Exception("Room could not be created " + p.message);
                }
                else if (listRoomsCallback != null)
                    listRoomsCallback(p.roomList);

            }
            else
                throw new Exception("Got no responce from server");
        })).Start();
    }
}