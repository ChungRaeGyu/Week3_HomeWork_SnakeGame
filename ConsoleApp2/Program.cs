class Program
{
    static void Main(string[] args)
    {
        // 게임 속도를 조정하기 위한 변수입니다. 숫자가 클수록 게임이 느려집니다.
        int gameSpeed = 100;
        int foodCount = 0; // 먹은 음식 수

        // 게임을 시작할 때 벽을 그립니다.
        DrawWalls();

        // 뱀의 초기 위치와 방향을 설정하고, 그립니다.
        Point p = new Point(4, 5, '*');
        Snake snake = new Snake(p, 4, Direction.RIGHT);
        snake.Draw();

        // 음식의 위치를 무작위로 생성하고, 그립니다.
        FoodCreator foodCreator = new FoodCreator(80, 20, '$');
        Point food = foodCreator.CreateFood();
        food.Draw();

        // 게임 루프: 이 루프는 게임이 끝날 때까지 계속 실행됩니다.
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

            // 뱀이 음식을 먹었는지 확인합니다.
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
                // 뱀이 음식을 먹지 않았다면, 그냥 이동합니다.
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

    // 벽 그리는 메서드
    static void DrawWalls()
    {
        // 상하 벽 그리기
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

public class Point
{
    public int x { get; set; }
    public int y { get; set; }
    public char sym { get; set; }

    // Point 클래스 생성자
    public Point(int _x, int _y, char _sym)
    {
        x = _x;
        y = _y;
        sym = _sym;
    }

    // 점을 그리는 메서드
    public void Draw()
    {
        Console.SetCursorPosition(x, y);
        Console.Write(sym);
    }

    // 점을 지우는 메서드
    public void Clear()
    {
        sym = ' ';
        Draw();
    }

    // 두 점이 같은지 비교하는 메서드
    public bool IsHit(Point p)
    {
        Console.SetCursorPosition(1, 1);
        Console.WriteLine($"p.x = {p.x} , x = {x}, y.x = {p.y}, y = {y}");
        return p.x == x && p.y == y;
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

public class Snake
{
    public List<Point> body; // 뱀의 몸통을 리스트로 표현합니다.
    public Direction direction; // 뱀의 현재 방향을 저장합니다.

    public Snake(Point tail, int length, Direction _direction)
    {
        direction = _direction;
        body = new List<Point>();
        for (int i = 0; i < length; i++)
        {
            Point p = new Point(tail.x, tail.y, '*');
            body.Add(p);
            tail.x += 1;
        }
    }

    // 뱀을 그리는 메서드입니다.
    public void Draw()
    {
        foreach (Point p in body)
        {
            p.Draw();
        }
    }

    // 뱀이 음식을 먹었는지 판단하는 메서드입니다.
    public bool Eat(Point food)
    {
        Point head = GetNextPoint();
        if (head.IsHit(food))
        {
            food.sym = head.sym;
            body.Add(food);
            return true;
        }
        else
        {
            return false;
        }
    }

    // 뱀이 이동하는 메서드입니다.
    public void Move()
    {
        Point tail = body.First();
        body.Remove(tail);
        Point head = GetNextPoint();
        body.Add(head);

        tail.Clear();
        head.Draw();
    }

    // 다음에 이동할 위치를 반환하는 메서드입니다.
    public Point GetNextPoint()
    {
        Point head = body.Last();
        Point nextPoint = new Point(head.x, head.y, head.sym);
        switch (direction)
        {
            case Direction.LEFT:
                nextPoint.x -= 2;
                break;
            case Direction.RIGHT:
                nextPoint.x += 2;
                break;
            case Direction.UP:
                nextPoint.y -= 1;
                break;
            case Direction.DOWN:
                nextPoint.y += 1;
                break;
        }
        return nextPoint;
    }

    // 뱀이 자신의 몸에 부딪혔는지 확인하는 메서드입니다.
    public bool IsHitTail()
    {
        var head = body.Last();
        for (int i = 0; i < body.Count - 2; i++)
        {
            if (head.IsHit(body[i]))
                return true;
        }
        return false;
    }

    // 뱀이 벽에 부딪혔는지 확인하는 메서드입니다.
    public bool IsHitWall()
    {
        var head = body.Last();
        if (head.x <= 0 || head.x >= 80 || head.y <= 0 || head.y >= 20)
            return true;
        return false;
    }
}

public class FoodCreator
{
    int mapWidth;
    int mapHeight;
    char sym;

    Random random = new Random();

    public FoodCreator(int mapWidth, int mapHeight, char sym)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.sym = sym;
    }

    // 무작위 위치에 음식을 생성하는 메서드입니다.
    public Point CreateFood()
    {
        int x = random.Next(2, mapWidth - 2);
        // x 좌표를 2단위로 맞추기 위해 짝수로 만듭니다.
        x = x % 2 == 1 ? x : x + 1;
        int y = random.Next(2, mapHeight - 2);
        return new Point(x, y, sym);
    }
}