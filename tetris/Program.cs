using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace tetris
{
    class Program
    {

        private enum blockType { T1, T2, T3, T4,B2,B1 };

        private enum MoveDir { Left, Right, Down };


        private static Dictionary<blockType, int[,]> map;



        private static List<int[]> COORDS= new List<int[]>();


        private static blockType currentBlock;


        //pozycja bloku w obecnej chwili
        private static int[] posBlock;

        private static int mapWidth = 12;
        private static int mapHeight = 25;

        private static int gametime = 1000;
        private static int score = 0;



        static void Main(string[] args)
        {
            Testing();
            Init();
            DrawMap();
            GenerateBlock();
            DrawBlock();
            ShowScore();

            while (true)
            {


                if(gametime > 500)
                {
                    gametime = 0;


                    if (!CheckCollision(MoveDir.Down))
                    {
                        MoveBlock(MoveDir.Down);
                        DrawMap();
                        DrawBlock();
                    }
                    else
                    {
                        EraseBlock();
                        IfLine();
                        GenerateBlock();
                        score += 10;
                        ShowScore();
                        if (CheckCollision(MoveDir.Down)) break;
                        DrawMap();
                        DrawBlock();
                    }
                }



                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();

                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            if (!CheckCollision(MoveDir.Right))
                            {
                                MoveBlock(MoveDir.Right);
                                DrawMap();
                                DrawBlock();
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            if (!CheckCollision(MoveDir.Left))
                            {
                                MoveBlock(MoveDir.Left);
                                DrawMap();
                                DrawBlock();
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (!CheckCollision(MoveDir.Down))
                                gametime = 1000;
                            break;
                        case ConsoleKey.UpArrow:
                                SpinBlock();
                                DrawMap();
                                DrawBlock();
                            break;
                        case ConsoleKey.Escape:
                            System.Environment.Exit(0);
                            break;
                    }
                }
                if (gametime < 1000) Thread.Sleep(100); 
                gametime += 500;
            }
        }


        public static void SpinBlock()
        {
            switch (currentBlock)
            {
                case blockType.T1:
                    currentBlock = blockType.T2;
                    break;
                case blockType.T2:
                    currentBlock = blockType.T3;
                    if (CheckCollision(MoveDir.Right))
                    {
                        posBlock[0]--;
                    }
                    break;
                case blockType.T3:
                    currentBlock = blockType.T4;
                    break;
                case blockType.T4:
                    currentBlock = blockType.T1;
                    if (CheckCollision(MoveDir.Right))
                    {
                        posBlock[0]--;
                    }
                    break;

                case blockType.B1:
                    currentBlock = blockType.B2;
                    break;

                case blockType.B2:
                    currentBlock = blockType.B1;
                    if (CheckCollision(MoveDir.Right)) posBlock[0]-- ;
                    if (CheckCollision(MoveDir.Left)) posBlock[0]++;
                    break;

            }
        }




        private static void DrawMap()
        {
            Console.BackgroundColor = ConsoleColor.Gray;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }

            Console.BackgroundColor = ConsoleColor.Blue;

            for (int i = 0; i < COORDS.Count; i++)
            {
                int[] coords = COORDS[i];
                Console.SetCursorPosition(coords[0], coords[1]);
                Console.Write(" ");
            }

            Console.ResetColor();

        }


        private static void Init()
        {
            map = new Dictionary<blockType, int[,]>();

            map.Add(blockType.T1, new int[,] {
                {0,0,0,0 },
                {0,0,0,0 },
                {0,1,0,0 },
                {1,1,1,0}, });


            map.Add(blockType.T2, new int[,] {
                {0,0,0,0 },
                {1,0,0,0 },
                {1,1,0,0 },
                {1,0,0,0}, });


            map.Add(blockType.T3, new int[,] {
                {0,0,0,0 },
                {0,0,0,0 },
                {1,1,1,0 },
                {0,1,0,0}, });

            map.Add(blockType.T4, new int[,] {
                {0,0,0,0 },
                {0,1,0,0 },
                {1,1,0,0 },
                {0,1,0,0}, });


            map.Add(blockType.B1, new int[,] {
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,0 },
                {1,1,1,0}, });


            map.Add(blockType.B2, new int[,] {
                {0,0,0,0 },
                {0,1,0,0 },
                {0,1,0,0 },
                {0,1,0,0}, });

            Console.SetWindowSize(100, 35);
            Console.SetBufferSize(100, 35);
        }


        private static void GenerateBlock()
        {

            switch (new Random().Next(6))
            {
                case 0:
                    currentBlock = blockType.T1;
                    break;
                case 1:
                    currentBlock = blockType.T2;
                    break;
                case 2:
                    currentBlock = blockType.T3;
                    break;
                case 3:
                    currentBlock = blockType.T4;
                    break;
                case 4:
                    currentBlock = blockType.B1;
                    break;
                case 5:
                    currentBlock = blockType.B2;
                    break;
            }


            posBlock = new int[2];
            posBlock[0] = new Random((int)DateTime.Now.Second).Next(mapWidth - 4);
            posBlock[1] = 0;

        }

        private static void DrawBlock()
        {

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (map[currentBlock][y, x] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(posBlock[0] + x, posBlock[1] + y);
                        Console.Write(" ");
                    }
                }
            }

            Console.ResetColor();
        }


        private static void MoveBlock(MoveDir move)
        {
            switch (move)
            {
                case MoveDir.Right:
                    posBlock[0]++;
                    break;
                case MoveDir.Down:
                    posBlock[1]++;
                    break;
                case MoveDir.Left:
                    posBlock[0]--;
                    break;
            }
        }


        private static bool CheckCollision(MoveDir move)
        {
            int[] sim = { posBlock[0], posBlock[1] };



            switch (move)
            {
                case MoveDir.Right:
                    sim[0]++;
                    break;
                case MoveDir.Down:
                    sim[1]++;
                    break;
                case MoveDir.Left:
                    sim[0]--;
                    break;
            }



            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (   (map[currentBlock][y, x] == 1)  &&( ((sim[0] + x) > mapWidth-1) ||
                        ((sim[1] + y) > mapHeight-1)  || ((sim[0] + x) < 0)))
                    {
                        return true;
                    }
                }
            }






            for (int i = 0; i < COORDS.Count; i++)
            {

                int[] coords = COORDS[i];

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if(  (map[currentBlock][y,x] == 1) && ((sim[0]+x) == coords[0]) &&
                            ((sim[1]+y) == coords[1]))
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
        }


        private static void EraseBlock()
        {

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                   if(map[currentBlock][y,x] == 1)
                    {
                        COORDS.Add(new int[] { x+posBlock[0], y+posBlock[1] });
                    }
                }
            }
        }


        private static void IfLine()
        {
            int line = 0;
            bool isLine = true;
            while (isLine)
            {
                isLine = false;

                for (int height = mapHeight; height > 0; height--)
                {
                    for (int i = 0; i < COORDS.Count; i++)
                    {
                        int[] coords = COORDS[i];

                        if (coords[1] == height) line++;

                    }

                    if (line == mapWidth)
                    {
                        score += 100;
                        DeleteLine(height);
                        isLine = true;
                    }
                    line = 0;
                }
            }
        }




        private static void DeleteLine(int row)
        {

            for (int i = 0; i < COORDS.Count; i++)
            {
                int[] coords = COORDS[i];
                if (coords[1] == row)
                {
                    COORDS.RemoveAt(i);
                    i--;
                }
            }

            ReorderBlocks(row);
        }




        public static void ReorderBlocks(int row)
        {
            for (int i = 0; COORDS.Count > i; i++)
            {
                int[] coords = COORDS[i];
                if (coords[1] < row) coords[1]++;
            }
        }




        private static void ShowScore( )
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(mapWidth + 10, mapHeight - 20);
            Console.Write("                                                                   ");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(mapWidth + 10, mapHeight - 20);
            Console.Write("SCORE: " +score);
            Console.ResetColor();
        }

        public static void Testing()
        {
            for(int i = 0; i < 11; i++)
            {
                COORDS.Add(new int[] { i,24});
            }
            for (int i = 0; i < 10; i++)
            {
                COORDS.Add(new int[] { i, 23 });
            }
        }



    }

}
