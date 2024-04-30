
internal class Snake
{

    public List<Point> body;
    public Direction direction;
    public Snake(Point tail, int length, Direction direct)
    {
        direction = direct;
        body = new List<Point>();
        for(int i=0; i < length; i++)
        {
            Point p = new Point(tail.x, tail.y, '*');
            body.Add(p);
            tail.x = p.x;
        }
    }
    public void Draw()
    {
        foreach(Point p in body)
        {
            p.Draw();
        }
    }
    public void Move()
    {
        Point tail = body.First();
        body.Remove(tail);
        Point head = GetNextPoint();
        body.Add(head);

        tail.Clear();
        head.Draw();
    }
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