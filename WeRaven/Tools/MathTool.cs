namespace WeRaven.Tools;

public static class MathTool
{
    private static readonly Random _random = new();

    private static int _range(int min, int max)
    {
        return _random.Next(min, max);
    }

    public static int GenerateCode()
    {
        return _range(100000, 999999);
    }
}