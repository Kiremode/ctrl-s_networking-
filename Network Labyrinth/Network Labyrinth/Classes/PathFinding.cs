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
    //TODO THIS NEEDS TO BE CHANGED BACK TO 512X512
    static bool[,] hasBeenVisitet = new bool[32,32];
    private static int YPos = 0;
    private static int XPos = 0;

    //saves  the walked way so that it can backtrack
    private static List<int> move = new List<int>();
    private static List<int> backTrack = new List<int>();
    
    
    
    /*
     *
     * list bla <left, left, lef>
     * <left, left, left,right, up, > 
     * 
     */

    
    
    private static bool isFirstMove = true;
    
    /*
     *DFS IDEE
     * 1  save in einem Vector 2 die lauf pos dir vectror Vector2()
     * 2 wenn wand setze die lauf pos auf 0
     * 3 drehe nach (o,u,l,r)
     * 4 wenn da nichts ist lauf da lang ansonsten backtrack 
     */
    
    
    /*
     * DFS mit der liste um die bewegungen einzuspeichern in die move liste  also z.b. wenn man nach links läuft wird links eingetragen und das gegenteil wird in backtrack eingespeichert
     * das map array in einen bool um zu checken ob wir da schon mal waren **A
     * dann  wenn man backtrackt in die 3 anderen richtungen schauen und dann die abzweigung nehmen
     *
     * DFS = DEPTH FIRST SEARCH 
     */
  
    /*                         **A
    *  false, false,false,false
    *  false, true, ture, false
    *  false, false,true,false
    *  true, true, ture, flase
    */
    
    
    static int toMove = 0;


    private static bool isBacktracking = false;
    private static Vector2 FuturePlayerPos = Vector2.Zero;
    
    private static bool checkFirstSide = true;
    
    public static void Walk(Socket socket, string status)
    {
        Vector2 savePlayerPos = GetPlayerPos(status);


        if (backTrack.Count != 0 &&!isFirstMove && isBacktracking )
        {
            Console.WriteLine("me Is backtracking");
            
            toMove = backTrack.Last() ;
            backTrack.RemoveAt(backTrack.Count -1);

            
            if (backTrack.Count != 0&& toMove == 0 || toMove == 1)
            {
                Console.WriteLine("LEFT RIGTH");
                if (checkFirstSide)
                {
                    toMove = 2;
                    checkFirstSide = false;
                }
                else
                {
                    toMove = 3;
                    checkFirstSide = true;
                    isBacktracking = false;
                }
            }
            else
            {
                Console.WriteLine("UP DOWN");
                if (checkFirstSide)
                {
                    toMove = 0;
                    checkFirstSide = false;
                }
                else
                {
                    toMove = 1;
                  
                    checkFirstSide = true;
                }
            }

        }
      
        if (backTrack.Count > 0 && !isBacktracking &&toMove == backTrack.Last() &&  !isFirstMove )
        {
            toMove = (toMove+ 1 ) % 4; // 0,1,2,3 -> 0
        }
        

        if (isFirstMove)
        {
            isFirstMove = false;
            return;
        }
        
        switch(toMove)
        {
            case 0:
                MoveThePlayer(socket, "UP", 1,savePlayerPos);
                break;
            case 1:
                MoveThePlayer(socket, "DOWN", 0,savePlayerPos);
                break;
            case 2:
                MoveThePlayer(socket, "LEFT", 3,savePlayerPos);
                break;
            case 3:
                MoveThePlayer(socket, "RIGHT", 2,savePlayerPos);
                break;

            default:
                Console.WriteLine("im not walking");
                break;
        }

        if (savePlayerPos.X < 0|| savePlayerPos.Y < 0|| savePlayerPos.X > 512 || savePlayerPos.Y > 512)
        {
            
            Console.WriteLine($"owo you are throwen out : X : {savePlayerPos.X}\n{savePlayerPos.Y}");
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

            hasBeenVisitet[(int)savePlayerPos.X, (int)savePlayerPos.Y] = true;
            toMove = 0;
            Console.WriteLine("im sorry" + hasBeenVisitet +"test");
            isBacktracking = false;

        }
        else
        {
            
            toMove = (toMove+ 1 ) % 4; // 0,1,2,3 -> 0

            if (toMove == 0)
            {
                isBacktracking = true;
            }
            
            Console.WriteLine($"the move dir: {toMove}");
            hasBeenVisitet[(int)savePlayerPos.X, (int)savePlayerPos.Y + YPos] = true;
            //move.RemoveAt(move.Count - 1);
            backTrack.RemoveAt(backTrack.Count - 1);
            
        }

    }
    
    /*
     *T
     *WW
     *  WW WWW WW
     *WW        W
     *  XWWWWWWWWW
     *
     *
     * 
     */

    
    static void MoveThePlayer(Socket socket, string directionString, int backTrackInt, Vector2 PlayerPos)
    {

        int lookUpInt = 0;
        switch (directionString)
        {
            case "UP":
                FuturePlayerPos.X = PlayerPos.X;
                FuturePlayerPos.Y = PlayerPos.Y -1;                
                break;
            case "DOWN":
                FuturePlayerPos.X = PlayerPos.X;
                FuturePlayerPos.Y = PlayerPos.Y +1;
                break;
            case "LEFT":
                FuturePlayerPos.X = PlayerPos.X -1;
                FuturePlayerPos.Y = PlayerPos.Y;
                break;
            case "RIGHT":
                FuturePlayerPos.X = PlayerPos.X +1;
                FuturePlayerPos.Y = PlayerPos.Y;
                break;
        }
        
        Setup.SendData(socket, directionString);
        
        //move.Add(0);
        backTrack.Add(backTrackInt);
        Console.WriteLine(directionString);
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