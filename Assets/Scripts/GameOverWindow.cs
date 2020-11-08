using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;
    private Text highscoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();

        transform.Find("RetryButton").GetComponent<Button_UI>().ClickFunc = () =>{Loader.Load(Loader.Scene.GameScene);};
        transform.Find("RetryButton").GetComponent<Button_UI>().AddButtonSound();

        transform.Find("MainMenuButton").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.MainMenu); };
        transform.Find("MainMenuButton").GetComponent<Button_UI>().AddButtonSound();
    }

    private void Start()
    {
        Hide();
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        if(Level.GetInstance().GetPipesPassedCount() >= score.GetHighscore())
        {
            highscoreText.text = "NEW HIGHSCORE";
        }else
        {
            highscoreText.text = "HIGHSCORE:      " + score.GetHighscore();
        }
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}