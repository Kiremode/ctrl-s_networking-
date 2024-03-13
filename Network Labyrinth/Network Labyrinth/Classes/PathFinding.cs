using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Numerics;

namespace Network_Labyrinth.Classes;

public class PathFinding
{
    //
    //0 unerkundet 1 lauf -1 wand 2 ausgang
    // x,y +/- 1 
    /*
     *[
     *000,
     *010,
     *111
     * ]
     * 
     */
    static int[,] map = new int[512,512];
    private static int YPos = 0;
    private static int XPos = 0;

    //saves  the walked way so that it can backtrack
    private static List<string> move;
    private static List<string> backTrack;
    
    private static bool isFirstMove = true;
    
    /*
     *DFS IDEE
     * 1  save in einem Vector 2 die lauf pos dir vectror Vector2()
     * 2 wenn wand setze die lauf pos auf 0
     * 3 drehe nach (o,u,l,r)
     * 4 wenn da nichts ist lauf da lang ansonsten backtrack 
     */
    
    
    /*
     *
     *
     * DFS mit der liste um die bewegungen einzuspeichern in die move liste  also z.b. wenn man nach links läuft wird links eingetragen und das gegenteil wird in backtrack eingespeichert
     * das map array in einen bool um zu checken ob wir da schon mal waren **A
     * dann  wenn man backtrackt in die 3 anderen richtungen schauen und dann die abzweigung nehmen
     *
     * DFS = DEPTH FIRST SEARCH 
     * 
     *
     * 
     */
  
    /*                         **A
    *  false, false,false,false
    *  false, true, ture, false
    *  false, false,true,false
    *  true, true, ture, flase
    */
    
    public static void Walk(Socket socket, string status)
    {
        //Setup.SendData(socket, "ENTER");
        //23 uhr 
        Random rnd = new Random();
        Vector2 savePlayerPos = GetPlayerPos(status);

        int toMove = rnd.Next(0, 4);
        
        switch(toMove)
        {
            case 0:
                Setup.SendData(socket, "UP");
                Console.WriteLine("Up");
                XPos = 0;
                YPos= -1;
                break;
            case 1:
                Setup.SendData(socket, "DOWN");
                Console.WriteLine("down");
                XPos = 0;
                YPos = 1;
                break;
            case 2:
                Setup.SendData(socket, "LEFT");
                Console.WriteLine("left");
                XPos = 1;
                YPos = 0;
                break;
            case 3:
                Setup.SendData(socket, "RIGHT");
                Console.WriteLine("right");
                XPos = -1;
                YPos = 0;
                break;

            default:
                Console.WriteLine("im not walking");
                break;
        }

        if (isFirstMove)
        {
            isFirstMove = false;
            return;
        }

        if (savePlayerPos.X == -1 || savePlayerPos.Y == -1 || savePlayerPos.X < 512 || savePlayerPos.Y < 512)
        {
            //reverse the curr dir
            return;
        }
        if (status.Contains("DONE"))
        {
            //dir [X:245, Y:2] isWall[245,1] = flase 
            /*
             *  false, false,false,false
             *  false, true, ture, false
             *  false, false,true,false
             *  true, true, ture, flase
             */

            map[(int)savePlayerPos.X, (int)savePlayerPos.Y] = 1;
            Console.WriteLine("im sorry" + map +"test");
        }
        else
        {
            Console.WriteLine(savePlayerPos);
            map[(int)savePlayerPos.X , (int)savePlayerPos.Y ] = -1;
        }

    }
    
    

    static Vector2 GetPlayerPos(string map)
    {
        Vector2 clensedPlayerPos = Vector2.Zero;
        
        Console.WriteLine(map);

        var s = map.Split('\n')[1];
        
        //build a  substing to get rid of the 9 
        //string neinLessS 
        //WTF -5 da man die länge der Charactere  die man in den substing haben will und nicht den index bei den man aufhören will WTF!!!!
        // und die 3 da man ja die [] loswerden will 
        // -5 man zieht die start pos ab und löscht die letzten beiden charactere ] >
        s =s.Substring(3, s.Length  -5);
        
        //lösche die letzte klammer  und das  >
        //s =s.Substring(0,  s.Length -2 );

        int x = 0;
        int y = 0;
        string[] relativeCleanPlayerPosString = s.Split(";");
        string num = "";
        foreach (var wario in relativeCleanPlayerPosString)
        {
            
            if (wario.Contains("X"))
            {
                num =wario.Substring(2, wario.Length -2);
                clensedPlayerPos.X = Int32.Parse(num);
            }
            if (wario.Contains("Y"))
            {
                
                num =wario.Substring(2, wario.Length -2);
                clensedPlayerPos.Y = Int32.Parse(num);
            }
            Console.WriteLine("owo " + wario +"ddr");    
        }
        
        return clensedPlayerPos;
    }

    public static bool DidWin(bool state)
    {
        return state;
    }
    
    
}