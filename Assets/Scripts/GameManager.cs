public static class GameManager
{
    public enum Accessory { Sunglasses, Hat, Carrot, None };
    public static int score = 0;
    public static int timeScore = 0;
    public static Accessory currentAccessory;

    public static void ResetScore()
    {
        score = 0;
        timeScore = 0;
    }

    public static void AddScore(int s)
    {
        score += s;
    }

    public static void AddTimerScore(int s)
    {
        timeScore += s;
    }

    public static void ChangeAccessory(Accessory a)
    {
        currentAccessory = a;
    }
}
