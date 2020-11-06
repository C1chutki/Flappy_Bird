using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;
    public Transform pfGround;

    public SoundAudioClip[] soundAudioClipsArray;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundMenager.Sound sound;
        public AudioClip audioClip;

    }
}
