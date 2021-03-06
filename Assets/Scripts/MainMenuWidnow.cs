﻿using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWidnow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("PlayButton").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.GameScene); };
        transform.Find("PlayButton").GetComponent<Button_UI>().AddButtonSound();
        transform.Find("QuitButton").GetComponent<Button_UI>().ClickFunc = () => { Application.Quit(); };
        transform.Find("QuitButton").GetComponent<Button_UI>().AddButtonSound();
    }
}
