internal class Point
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
        return p.x == x && p.y == y;
    }
}