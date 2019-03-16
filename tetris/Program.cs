using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



namespace tetris
{
    class Program
    {

        private enum blockType { T1, T2, T3, T4 };

        private enum MoveDir { Left, Right, Down };


        private static Dictionary<blockType, int[,]> map;


        //ten sam index w obu listach daje info o konkretym obiekcie
        private static List<int[,]> blockTypeContainer = new List<int[,]>();
        private static List<int[]> blocksCOORDS = new List<int[]>();

        private static blockType currentBlock;
        //private static int currentBlockOrientation = 0;
        //private static int[,] currentBlockStructure;


        //pozycja bloku w obecnej chwili
        private static int[] posBlock;

        private static int mapWidth = 12;
        private static int mapHeight = 25;


        //a tak sobie dalem
        private static Boolean erased = false;



        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                DrawMap();
                DrawBlock();

                if (!CheckCollision(MoveDir.Down))
                {
                    EraseBlock();
                    GenerateBlock();
                    erased = true;
                    IfLine();

                }

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();

                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            if (CheckCollision(MoveDir.Right))
                                MoveBlock(MoveDir.Right);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (CheckCollision(MoveDir.Left))
                                MoveBlock(MoveDir.Left);
                            break;
                        case ConsoleKey.DownArrow:
                            if (CheckCollision(MoveDir.Down))
                                MoveBlock(MoveDir.Down);
                            break;
                        case ConsoleKey.UpArrow:
                            SpinBlock();
                            break;
                        case ConsoleKey.Escape:
                            System.Environment.Exit(0);
                            break;
                    }
                }
                if (!erased && CheckCollision(MoveDir.Down)) MoveBlock(MoveDir.Down);
                else erased = false;

                Thread.Sleep(69);
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
                    break;
                case blockType.T3:
                    currentBlock = blockType.T4;
                    break;
                case blockType.T4:
                    currentBlock = blockType.T1;
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

            for (int i = 0; i < blocksCOORDS.Count; i++)
            {

                int[] coords = blocksCOORDS[i];

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (blockTypeContainer[i][y, x] == 1)
                        {
                            Console.SetCursorPosition(coords[0] + x, y + coords[1]);
                            Console.Write(" ");
                        }

                    }
                }
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
                {1,1,1, 0}, });


            map.Add(blockType.T2, new int[,] {
                {0,0,0,0 },
                {0,1,0,0 },
                {0,1,1,0 },
                {0,1,0,0}, });


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


            int letsRoll = new Random().Next(4);

            if (letsRoll == 0) currentBlock = blockType.T1;
            if (letsRoll == 1) currentBlock = blockType.T2;
            if (letsRoll == 2) currentBlock = blockType.T3;
            if (letsRoll == 3) currentBlock = blockType.T3;


            posBlock = new int[2];
            posBlock[0] = new Random((int)DateTime.Now.Second).Next(mapWidth - 4);
            posBlock[1] = 0;


            Console.SetWindowSize(100, 35);
            Console.SetBufferSize(100, 35);
        }


        private static void GenerateBlock()
        {
            int letsRoll = new Random().Next(3);

            if (letsRoll == 0) currentBlock = blockType.T1;
            if (letsRoll == 1) currentBlock = blockType.T2;
            if (letsRoll == 2) currentBlock = blockType.T3;


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




            switch (currentBlock)
            {
                case blockType.T1:

                    if (sim[0] + 3 > mapWidth || sim[0] < 0 || sim[1] + 4 > mapHeight)
                        return false;
                    break;
                case blockType.T2:
                    if (sim[0] + 3 > mapWidth || sim[0] < -1 || sim[1] + 4 > mapHeight)
                        return false;
                    break;
                case blockType.T3:

                    if (sim[0] + 3 > mapWidth || sim[0] < 0 || sim[1] + 4 > mapHeight)
                        return false;
                    break;
                case blockType.T4:
                    if (sim[0] + 2 > mapWidth || sim[0] < 0 || sim[1] + 4 > mapHeight)
                        return false;
                    break;
            }






            for (int i = 0; i < blocksCOORDS.Count; i++)
            {

                int[] coordsBlock = blocksCOORDS[i];

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y1 = 0; y1 < 4; y1++)
                        {
                            for (int x1 = 0; x1 < 4; x1++)
                            {

                                if ((coordsBlock[0] + x1 == sim[0] + x) &&
                                    (coordsBlock[1] + y1 == sim[1] + y))
                                {
                                    if (map[currentBlock][y, x] == 1 &&
                                        blockTypeContainer[i][y1, x1] == 1)
                                    {
                                        return false;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return true;
        }


        private static void EraseBlock()
        {
            int[,] structure = new int[4, 4];


            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    structure[y, x] = map[currentBlock][y, x];
                }
            }

            blockTypeContainer.Add(structure);
            blocksCOORDS.Add(posBlock);
        }


        private static void IfLine()
        {
            int line = 0;

            for (int height = mapHeight; height > 0; height--)
            {
                for (int i = 0; i < blocksCOORDS.Count; i++)
                {

                    int[] pos = blocksCOORDS[i];
                    int[,] structure = blockTypeContainer[i];

                    for (int y = 0; y < 4; y++)
                    {

                        if ((pos[1] + y == height))
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                if (structure[y, x] == 1)
                                {
                                    line++;
                                }
                            }
                        }
                    }

                }
                if (line == mapWidth)
                {
                    DeleteLine(height);
                }
                line = 0;
            }

        }




        private static void DeleteLine(int row)
        {
            HashSet<int> list = new HashSet<int>();

            for (int i = 0; i < blocksCOORDS.Count; i++)
            {

                int[] coords = blocksCOORDS[i];
                int[,] structure = blockTypeContainer[i];

                for (int y = 0; y < 4; y++)
                {

                    if (coords[1] + y == row)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (structure[y, x] == 1)
                            {
                                structure[y, x] = 0;
                                list.Add(i);
                            }
                        }
                    }
                }
            }
            ClearBlock(list.ToArray());
            ReorderBlocks(row);
        }


        private static void ClearBlock(int[] index)
        {
            bool shallWe = true;

            int deletedObjects = 0;


            for (int loop = 0; loop < index.Length; loop++)
            {


                int[] coords = blocksCOORDS[index[loop] - deletedObjects];
                int[,] structure = blockTypeContainer[index[loop] - deletedObjects];

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (structure[y, x] == 1)
                        {
                            shallWe = false;
                        }
                    }
                }
                if (shallWe)
                {
                    blocksCOORDS.RemoveAt(index[loop] - deletedObjects);
                    blockTypeContainer.RemoveAt(index[loop] - deletedObjects);
                    deletedObjects++;
                }

                shallWe = true;

            }


        }



        public static void ReorderBlocks(int row)
        {
            for (int i = 0; blocksCOORDS.Count > i; i++)
            {
                int Y = blocksCOORDS[i][1];
                if (Y > 20 && Y < 24) blocksCOORDS[i][1] = blocksCOORDS[i][1] + 1;
            }
        }




        private static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(mapWidth + 10, mapHeight - 20);
            Console.Write("                                                                   ");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(mapWidth + 10, mapHeight - 20);
            Console.Write(message);
            Console.ResetColor();
        }


        private static void CheckStructure()
        {
            String coords = "";
            for (int i = 0; blockTypeContainer.Count > i; i++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        coords += blockTypeContainer[i][y, x];
                    }
                    coords += "  ";
                }
                Debug(coords);
                coords += " NEXT  ";
            }
        }



    }

}
