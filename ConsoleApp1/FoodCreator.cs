
using System;

internal class FoodCreator
{
    int mapWidth;
    int mapHeight;
    char sym;

    public FoodCreator(int mapWidth, int mapHeight, char sym)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.sym = sym;
    }

    internal Point CreateFood()
    {
        Random random = new Random();
        int x = random.Next(2, mapWidth - 2);
        // x 좌표를 2단위로 맞추기 위해 짝수로 만듭니다.
        x = x % 2 == 0 ? x : x + 1;
        int y = random.Next(2, mapHeight - 2);
        return new Point(x, y, sym);
    }
}