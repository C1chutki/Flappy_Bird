using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using System;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdbody2D;
    private const float JUMP_AMOUNT = 120f;
    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    private static Bird instance;
    private State state;
    public static Bird GetInstance()
    {
        return instance;
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }

    private void Awake()
    {
        instance = this;
        birdbody2D = GetComponent<Rigidbody2D>();
        birdbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {

                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
        
    }
    private void Jump()
    {
        birdbody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundMenager.PlaySound(SoundMenager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdbody2D.bodyType = RigidbodyType2D.Static;
        SoundMenager.PlaySound(SoundMenager.Sound.Lose);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }
}
