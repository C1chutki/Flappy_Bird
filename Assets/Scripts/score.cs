using UnityEngine;

public static class score
{

    public static void Start()
    {
        //ResetHighscore();
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    private static void Bird_OnDied(object sender, System.EventArgs e)
    {
        TrySetNewHighscore(Level.GetInstance().GetPipesPassedCount());
    }

    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscoreText");
    }

    public static bool TrySetNewHighscore(int score)
    {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("highscoreText", score);
            PlayerPrefs.Save();
            return true;
        } else
        {
            return false;
        }
    }

    public static void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscoreText", 0);
        PlayerPrefs.Save();
    }
}
