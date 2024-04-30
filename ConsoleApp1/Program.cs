using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();
        gameManager.Initialize();
        gameManager.Start();
        // 게임 루프: 이 루프는 게임이 끝날 때까지 계속 실행됩니다.
    }
}

internal class GameManager
{
    Snake snake;
    Point p;
    FoodCreator foodCreator;
    Point food;
    int gameSpeed = 100;
    int foodCount = 0; // 먹은 음식 수
    public void Initialize()
    {
        // 뱀의 초기 위치와 방향을 설정하고
        p = new Point(4, 5, '*');
        snake = new Snake(p, 4, Direction.RIGHT);
        snake.Draw();

        // 음식의 위치를 무작위로 생성하고, 그립니다.
        foodCreator = new FoodCreator(80, 20, '$');
        food = foodCreator.CreateFood();
        food.Draw();
        DrawWall();
    }

    internal void Start()
    {
        while (true)
        {
            // 키 입력이 있는 경우에만 방향을 변경합니다.
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        snake.direction = Direction.UP;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.direction = Direction.DOWN;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.direction = Direction.LEFT;
                        break;
                    case ConsoleKey.RightArrow:
                        snake.direction = Direction.RIGHT;
                        break;
                }
            }
            if (snake.Eat(food))
            {
                foodCount++; // 먹은 음식 수를 증가
                food.Draw();

                // 뱀이 음식을 먹었다면, 새로운 음식을 만들고 그립니다.
                food = foodCreator.CreateFood();
                food.Draw();
                if (gameSpeed > 10) // 게임이 점점 빠르게
                {
                    gameSpeed -= 10;
                }
            }
            else
            {
                snake.Move();
            }
            Thread.Sleep(gameSpeed);

            // 벽이나 자신의 몸에 부딪히면 게임을 끝냅니다.
            if (snake.IsHitTail() || snake.IsHitWall())
            {
                break;
            }

            Console.SetCursorPosition(0, 21); // 커서 위치 설정
            Console.WriteLine($"먹은 음식 수: {foodCount}"); // 먹은 음식 수 출력
        }
        WriteGameOver();  // 게임 오버 메시지를 출력합니다.
        Console.ReadLine();

        // 뱀이 이동하고, 음식을 먹었는지, 벽이나 자신의 몸에 부딪혔는지 등을 확인하고 처리하는 로직을 작성하세요.
        // 이동, 음식 먹기, 충돌 처리 등의 로직을 완성하세요.

        Thread.Sleep(100); // 게임 속도 조절 (이 값을 변경하면 게임의 속도가 바뀝니다)

            // 뱀의 상태를 출력합니다 (예: 현재 길이, 먹은 음식의 수 등)
    }
    static void WriteGameOver()
    {
        int xOffset = 25;
        int yOffset = 22;
        Console.SetCursorPosition(xOffset, yOffset++);
        WriteText("============================", xOffset, yOffset++);
        WriteText("         GAME OVER", xOffset, yOffset++);
        WriteText("============================", xOffset, yOffset++);
    }

    static void WriteText(string text, int xOffset, int yOffset)
    {
        Console.SetCursorPosition(xOffset, yOffset);
        Console.WriteLine(text);
    }
    private void DrawWall()
    {
        for (int i = 0; i < 80; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("#");
            Console.SetCursorPosition(i, 20);
            Console.Write("#");
        }

        // 좌우 벽 그리기
        for (int i = 0; i < 20; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("#");
            Console.SetCursorPosition(80, i);
            Console.Write("#");
        }
    }
}

    // 방향을 표현하는 열거형입니다.
    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }