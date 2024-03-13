using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;


namespace Network_Labyrinth.Classes;

public class WallFollow
{
    private static int toMove = 0;

    private static int lastDir = -1;

    private static bool didCheckAllRot = false;
    private static bool resetMoveDir = false;
    
    private static bool isValidMove = true;

    private static bool isDirSaved = true;
    
    /*
     *Prüfen oben, links, unten rechts
     * gehen in eine freie richtung ( nach den oben genanten muster)
     * wenn an eine wand stoßen wieder prüfen wo man hinn kann mit ausname von da wo man herkommt,
     * wenn sackgasse jeden schritt neu prüfen und andere richtung einschlagen
     * 
     */
    
    public static void Walk(Socket socket, string status)
    {
        
        
        if (status.Contains("DONE"))
        {
            lastDir = toMove;
            
            //kann hin gehen 
            resetMoveDir = false;
            
            
            //Console.WriteLine("last dir test owo test: " +lastDir);
        }
        else
        {
            if (!resetMoveDir)
            {
                Console.WriteLine("RESET MOVE");
                toMove = -1; //so that it will become a 0 when it is +1 
                resetMoveDir = true;
            }

            if (resetMoveDir && toMove == 3)
            {
                Console.WriteLine("IM IN HERE SO IT SHOULD WORK");
                toMove = LastDirLookUP(lastDir);
                isValidMove = true;
            }
            else
            {
                toMove = (toMove+ 1 ) % 4; // 0,1,2,3 -> 0
            
                //lastDir = toMove;
                isValidMove = true;
                if (LastDirLookUP(lastDir) == toMove)
                {
                    isValidMove = false; // 0,1,2,3 -> 0
                }    
            }
            
        }
        if (isValidMove)
        {
            Setup.SendData(socket, "ENTER");
            switch(toMove)
            {
                case 0:
                    MoveThePlayer(socket, "UP", 1);
                    break;
                case 1:
                    MoveThePlayer(socket, "DOWN", 0);
                    break;
                case 2:
                    MoveThePlayer(socket, "LEFT", 3);
                    break;
                case 3:
                    MoveThePlayer(socket, "RIGHT", 2);
                    break;

                default:
                    Console.WriteLine("im not walking");
                    break;
            }
        }
    }

    static int LastDirLookUP(int dir)
    {
        switch (dir)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
        }

        return 5;
    }
    
    static void MoveThePlayer(Socket socket, string directionString, int backTrackInt)
    {

        int lookUpInt = 0;
        switch (directionString)
        {
            case "UP":
                break;
            case "DOWN":
                break;
            case "LEFT":
                break;
            case "RIGHT":
                break;
        }
        
        Setup.SendData(socket, directionString);
        
        
        Console.WriteLine(directionString);
    }

    
    
    static Vector2 GetPlayerPos(string map)
    {
        Vector2 clensedPlayerPos = Vector2.Zero;
        
        Console.WriteLine(map);

        var s = map.Split('\n')[1];
   
        // -5 man zieht die start pos ab und löscht die letzten beiden charactere ] >
        s =s.Substring(3, s.Length  -5);


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
                
        }
        
        return clensedPlayerPos;
    }
}