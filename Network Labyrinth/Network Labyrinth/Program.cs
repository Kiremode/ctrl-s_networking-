using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Network_Labyrinth.Classes;

// See https://aka.ms/new-console-template for more information
namespace Network_Labyrinth;

public static class Program
{
    public static void Main()
    {
        Client();
        Console.ReadKey();
    }

    public static void Client()
    {
        try
        {
            IPHostEntry host = Dns.GetHostEntry("labyrinth.ctrl-s.de"); //Use the url
            IPAddress ipAddress = host.AddressList[0]; //get the first ip of the labyrinth.ctrl-s.de server 
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 50000); //get the endpoint of the ip and link it to the port 50k
            
            //create the TCP socket.
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            //connect the socket to the end point
            sender.Connect(ipEndPoint);
            
            //this function sets up the play  field
            Setup.MapSetup(sender);

            GameRun(sender);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    //run the Game.
    static void GameRun(Socket sender){
        while (true)
        {
            var map = Setup.SendMap(sender ,"PRINT");
            
            WallFollow.Walk(sender, map);
            Thread.Sleep(400);
        } 
    }
    

}