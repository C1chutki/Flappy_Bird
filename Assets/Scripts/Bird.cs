using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdbody2D;
    private const float JUMP_AMOUNT = 120f;

    private void Awake()
    {
        birdbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }
    private void Jump()
    {
        birdbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CMDebug.TextPopupMouse("Dead!");
    }
}
