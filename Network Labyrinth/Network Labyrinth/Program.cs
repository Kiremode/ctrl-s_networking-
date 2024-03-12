﻿using System;
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
        Console.WriteLine("hello world");
        Client();
        //make a while here so that i can remove the while in path  finder
        // the while will  run until  the end is reached 
        //  the map will be printed here and be given to the path finder 
        Console.ReadKey();
    }

    public static void Client()
    {
        byte[] bytes = new byte[1024];

        try
        {
            IPHostEntry host = Dns.GetHostEntry("labyrinth.ctrl-s.de"); //Use the url
            IPAddress ipAddress = host.AddressList[0]; //get the first ip of the labyrinth.ctrl-s.de server 
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 50000); //get the endpoint of the ip and link it to the port 50k
            
            //create the TCP socket.
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            //connect the socket to the end point
            sender.Connect(ipEndPoint);
            
            GetMSG(sender, bytes);

            Thread.Sleep(1000); // this time out is here as a test for the send data ( because it could be that it sends the code to fast ) 

            Setup.MapSetup(sender);
     
            int byteRecord = sender.Receive(bytes);
            
            var print  = Encoding.ASCII.GetString(bytes, 0, byteRecord);
            
            //Console.WriteLine(print);
            //
            //File.WriteAllText("hi.txt", print);

            PathFinding.Walk(sender);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public static string PrintMap(Socket sender, byte[] bytes)
    {
        //get  the  map  
        //remove the first line and the last line so that i get rid of the numbers
        //remove the first letter in the line to remove the letters  
        // make a array out of it 
        int byteRecord = sender.Receive(bytes);
            
        string print  = Encoding.ASCII.GetString(bytes, 0, byteRecord);
        return print;
    }
    
    
    public static void GetMSG(Socket sender, byte[] bytes)
    {
        Console.WriteLine("im in the client");
        try
        {
            //sender.Connect(ipEndPoint);
            Console.WriteLine("socket is connected  {0}",sender.RemoteEndPoint);//check  if connected
                
            //get the responce
            int byteRecord = sender.Receive(bytes);
            Console.WriteLine("Rec data {0}", Encoding.ASCII.GetString(bytes,0,byteRecord));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    static string CleanUpMapInput(string map)
    {
        string cleanMap ="";
        // make a regex script that removes [X:311;Y:132;Z:0]> print T
        return cleanMap;
    }
    
    
}