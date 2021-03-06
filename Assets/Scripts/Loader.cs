﻿using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
    }

    private static Scene TargetScene;

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());
        TargetScene = scene;
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(TargetScene.ToString());
    }
}
