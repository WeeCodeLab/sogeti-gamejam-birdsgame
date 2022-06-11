using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Trash : MonoBehaviour
{ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerLayer = LayerMask.NameToLayer("Player");
        
        if (collision.collider.gameObject.layer == ~playerLayer)
        {
            //TODO: Deal damage to player
        }
    }
}
