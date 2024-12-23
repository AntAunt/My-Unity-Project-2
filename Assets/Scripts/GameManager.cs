using System;
using System.Diagnostics;

public static class GameManager
{
    public enum Accessory { Sunglasses, Hat, Carrot, None };
    public static int score = 0;
    public static int timeScore = 0;
    public static Accessory currentAccessory = Accessory.None;
    public static string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8", 
                                            "Level9", "Level10", "Level11", "Level12", "Level13", "Level14", "Level15",};
    private static int[] sizeScores = { 50, 30, 20, 10, 5 };

    public static void ResetScore()
    {
        score = 0;
        timeScore = 0;
    }

    public static void AddScore(int s)
    {
        score += s;
    }

    public static void SetTimerScore(int s)
    {
        timeScore = s;
    }

    public static int GetTotalScore() {
        return score + timeScore;
    }

    public static void ChangeAccessory(Accessory a)
    {
        currentAccessory = a;
    }

    public static void SetSizeScore(int par, int snowballs)
    {
        int snowballsFromPar = Math.Abs(snowballs - par);

        if (snowballsFromPar > sizeScores.Length - 1)
        {
            snowballsFromPar = sizeScores.Length - 1;
        }

        score += sizeScores[snowballsFromPar];
    }
}
