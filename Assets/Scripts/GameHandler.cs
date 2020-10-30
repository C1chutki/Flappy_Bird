﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("GameHandler.Start");

        GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;


    }
}
