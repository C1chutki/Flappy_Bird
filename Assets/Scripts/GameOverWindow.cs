using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();

        transform.Find("RetryButton").GetComponent<Button_UI>().ClickFunc = () =>{Loader.Load(Loader.Scene.GameScene);};
        transform.Find("RetryButton").GetComponent<Button_UI>().AddButtonSound();
        transform.Find("MainMenuButton").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.MainMenu); };
        transform.Find("MainMenuButton").GetComponent<Button_UI>().AddButtonSound();
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
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