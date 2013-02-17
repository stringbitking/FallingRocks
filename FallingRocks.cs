using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FallingRocks
{
    class FallingRocks
    {
        static int playerPosition = Console.WindowWidth / 2;
        static int fallingRocksNumber = 30;
        static int score = 0;
        static bool quitGame = false;
        static Random randomGenerator = new Random();

        static void RemoveScrollBars()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        static void InitialiseX(int[,] rocks, int i)
        {
                int randomX = randomGenerator.Next(0, Console.WindowHeight - 1);
                rocks[0, i] = randomX;
        }

        static void InitialiseY(int[,] rocks, int j)
        {
                int randomY = randomGenerator.Next(0, Console.WindowWidth - 1);
                rocks[1, j] = randomY;
                rocks[2, j] = (randomY % 3) + 1; //The number of symbols in the rock
        }

        //Generates initial positions for the rocks
        static void InitialPositions(int[,] rocks)
        {
            for (int i = 0; i < fallingRocksNumber; i++)
            {
                InitialiseX(rocks, i);
            }
            for (int j = 0; j < fallingRocksNumber; j++)
            {
                InitialiseY(rocks, j);
            }
        }

        static void MovePlayerRight()
        {
            if (playerPosition < Console.WindowWidth - 2)
            {
                playerPosition++;
            }
        }

        static void MovePlayerLeft()
        {
            if (playerPosition > 1)
            {
                playerPosition--;
            }
        }

        static void PrintAtPosition(int x, int y, char symbol)
        {
            Console.SetCursorPosition(y, x);
            Console.Write(symbol);
        }

        static void DrawPlayer()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            PrintAtPosition(Console.WindowHeight - 2, playerPosition - 1, '(');
            PrintAtPosition(Console.WindowHeight - 2, playerPosition, '0');
            PrintAtPosition(Console.WindowHeight - 2, playerPosition + 1, ')');
        }

        static char GetSymbol(int i)
        {
            switch (i)
            {
                case 1:
                    return '#';
                    break;
                case 2:
                    return '%';
                    break;
                case 3:
                    return '+';
                    break;
                case 4:
                    return '*';
                    break;
                case 5:
                    return '$';
                    break;
                default:
                    return '*';
            }
        }

        static void DrawRocks(int[,] rocks)
        {
            for (int i = 0; i < fallingRocksNumber; i++)
            {
                char symbol = GetSymbol(rocks[1, i] % 5 + 1);
                if (symbol == '$')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    rocks[2, i] = 1;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (rocks[1, i] > Console.WindowWidth - rocks[2, i])
                {
                    for (int j = 0; j < rocks[2, i]; j++)
                    {
                        PrintAtPosition(rocks[0, i], rocks[1, i] - j, symbol);
                    }
                }
                else
                {
                    for (int j = 0; j < rocks[2, i]; j++)
                    {
                        PrintAtPosition(rocks[0, i], rocks[1, i] + j, symbol);
                    }
                }
            }
        }

        static void MoveRocks(int[,] rocks)
        {
            for (int i = 0; i < fallingRocksNumber; i++)
            {
                if (rocks[0, i] < Console.WindowHeight - 2)
                    rocks[0, i]++;
                else
                {
                    rocks[0, i] = 0;
                    InitialiseY(rocks, i);
                }
            }
        }

        static void CrashDetection(int[,] rocks)
        {
            for (int i = 0; i < fallingRocksNumber; i++)
            {
                if (rocks[0, i] == Console.WindowHeight - 2)
                {
                    if ((rocks[1, i] > playerPosition - 2 && rocks[1, i] < playerPosition + 2)
                        || (rocks[1, i] + rocks[2, i] < playerPosition + 2 && rocks[1, i] + rocks[2, i] - 1 > playerPosition - 2))
                    {
                        if (GetSymbol(rocks[1, i] % 5 + 1) == '$')
                        {
                            score++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            score = 0;
                            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 1);
                            Console.Write("GAME OVER!");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }

        static void DisplayScore()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintAtPosition(Console.WindowHeight - 1, 0, '|');
            Console.Write("Money: " + score);
            Console.Write('|');
        }

        static void RefreshMenu(int menuCenter, int menuFirstRow)
        {
            Console.ForegroundColor = ConsoleColor.White;
            
            string newGame = "  New Game  ";
            Console.SetCursorPosition(menuCenter - (newGame.Length/2), menuFirstRow);
            Console.Write(newGame);
            string selectLevel = "  Select Level  ";
            Console.SetCursorPosition(menuCenter - (selectLevel.Length / 2), menuFirstRow + 1);
            Console.Write(selectLevel);
            string quit = "  Quit  ";
            Console.SetCursorPosition(menuCenter - (quit.Length / 2), menuFirstRow + 2);
            Console.Write(quit);
        }

        static int menuItemLength(int count)
        {
            string menuItem1 = "New Game";
            string menuItem2 = "Select Level";
            string menuItem3 = "Quit";
            switch (count)
            {
                case 0:
                    return menuItem1.Length;
                    break;
                case 1:
                    return menuItem2.Length;
                    break;
                case 2:
                    return menuItem3.Length;
                    break;
                default:
                    return 0;
                    break;
            }
        }

        static void SelectLevel()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("How many falling blocks do you want: ");
            fallingRocksNumber = int.Parse(Console.ReadLine());
        }

        static void DisplayMenu(int[,] rocks)
        {
            int count = 0;
            int menuCenter = Console.WindowWidth / 2;
            int menuFirstRow = Console.WindowHeight / 2 - 1;
            RefreshMenu(menuCenter, menuFirstRow);
            Console.SetCursorPosition(menuCenter - (menuItemLength(count) / 2) - 2, menuFirstRow + count);
            Console.Write("*");
            Console.SetCursorPosition(menuCenter + (menuItemLength(count) / 2) + 1, menuFirstRow + count);
            Console.Write("*");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (count < 2)
                    {
                        RefreshMenu(menuCenter, menuFirstRow);
                        count++;
                    }
                }
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (count > 0)
                    {
                        RefreshMenu(menuCenter, menuFirstRow);
                        count--;
                    }
                }
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    switch (count)
                    {
                        case 0:
                            InitialPositions(rocks);
                            break;
                        case 1:
                            SelectLevel();
                            break;
                        case 2:
                            quitGame = true;
                            break;
                    }
                    break;
                }
                Console.SetCursorPosition(menuCenter - (menuItemLength(count) / 2) - 2, menuFirstRow + count);
                Console.Write("*");
                Console.SetCursorPosition(menuCenter + (menuItemLength(count) / 2) + 1, menuFirstRow + count);
                Console.Write("*");
            }
        }

        static void Main(string[] args)
        {
            int[,] rocks = new int[3, 100];
            RemoveScrollBars();
            DisplayMenu(rocks);
            InitialPositions(rocks);
            while (true)
            {
                if (quitGame)
                {
                    break;
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        MovePlayerRight();
                    }
                    if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        MovePlayerLeft();
                    }
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        DisplayMenu(rocks);
                    }
                }
                Console.Clear();
                DisplayScore();
                DrawPlayer();
                DrawRocks(rocks);
                CrashDetection(rocks);
                MoveRocks(rocks);
                Thread.Sleep(150);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight - 1);
        }
    }
}
