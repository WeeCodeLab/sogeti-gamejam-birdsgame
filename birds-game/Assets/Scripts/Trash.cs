using birds_game.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Trash : MonoBehaviour
{
    private Vector2 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }
    
    private void Update()
    {
        if (Vector2.Distance(_startPosition, transform.position) > 20f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerLayer = LayerMask.NameToLayer("Player");
        
        if (collision.collider.gameObject.layer == playerLayer)
        {
            GameManager.Instance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
