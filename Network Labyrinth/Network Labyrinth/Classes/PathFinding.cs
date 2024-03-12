using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Network_Labyrinth.Classes;

public class PathFinding
{
    //make a funktion that saves/ updates the map data  
    // make a small algorythm that walks the path maby flood search  or something simmelar
    // make it so that it walkes to the place with the most empty spaces

    
    //make a small a* path finding algorythm and get the map every step and add the new places in it 
    //make  it so that it searches for a random point and walk to there ( it needs to  be  a  space)  
    // make a prototype in js ( because it is easyer to test it there)
    public static void Walk(Socket socket)
    {
        byte[] bytes = new byte[1024];
        //23 uhr 
        Random rnd = new Random();
        while (true)
        {
            int toMove = rnd.Next(0, 4);
            switch(toMove)
            {
                case 0:
                    Setup.SendData(socket, "UP");
                    Console.WriteLine("Up");
                    break;
                case 1:
                    Setup.SendData(socket, "DOWN");
                    Console.WriteLine("down");
                    break;
                case 2:
                    Setup.SendData(socket, "LEFT");
                    Console.WriteLine("left");
                    break;
                case 3:
                    Setup.SendData(socket, "RIGHT");
                    Console.WriteLine("right");
                    break;

            }

            Console.WriteLine(Program.PrintMap(socket, bytes));
            
            Thread.Sleep(1000);
        }
        
        
    }
    
}