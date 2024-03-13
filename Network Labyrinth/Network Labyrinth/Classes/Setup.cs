using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Network_Labyrinth.Classes;

public class Setup
{
    //this will set the  map the 500x500 and starts the game 
    public static void MapSetup(Socket sender)
    {
        // set up the strings for the setup with out the \n it didnt work
        //TODO THIS NEEDS TO BE CHANGED BACK TO 512X512
        string width = "WIDTH 32";
        string height  = "HEIGHT 32";
        string depth = "DEPTH 1";
        string start = "START";
        string print = "Print";

        
        SendData(sender, width);
        Thread.Sleep(1000);
        SendData(sender, height);
        Thread.Sleep(1000);
        SendData(sender, depth);
        Thread.Sleep(1000);
        SendData(sender, start);
        Thread.Sleep(1000);
        SendData(sender, print);

    }

    
    public static string SendData(Socket sender, string msg)
    {
        byte[] bytes = new byte[1024];
        sender.Send(Encoding.ASCII.GetBytes(msg +" \n"));
        int byteRecord = sender.Receive(bytes);
        var recData = Encoding.ASCII.GetString(bytes, 0, byteRecord);
        
        Console.WriteLine(recData);
        return recData;
    }
    
    public static string SendMap(Socket sender, string msg)
    {
        byte[] bytes = new byte[1024];
        sender.Send(Encoding.ASCII.GetBytes(msg +"\n"));
        int byteRecord = sender.Receive(bytes);
        var recData = Encoding.ASCII.GetString(bytes, 0, byteRecord);
        
        Console.WriteLine(recData);

        return recData;
    }
    
}