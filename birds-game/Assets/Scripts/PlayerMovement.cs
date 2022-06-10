using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpPower;

    private Input _input;
    private Rigidbody2D _rigidbody2D;
    
    private void Start()
    {
        _input = new Input();
        _input.Enable();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var moveDirection = _input.Player.Move.ReadValue<float>();
        _rigidbody2D.velocity = new Vector2(moveDirection * _movementSpeed, _rigidbody2D.velocity.y);
    }

    private void RegisterInput()
    {
        _input.Player.Jump.started += _ => Jump();
    }

    private void Jump()
    {
        if (!IsGrounded()) return;

        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }
    
    private bool IsGrounded() //+
    {
        var layerId = LayerMask.NameToLayer("World");
        var groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, ~layerId);
        return groundCheck.collider != null;
    }

    private void OnEnable()
    {
        _input?.Enable();
    }

    private void OnDisable()
    {
        _input?.Disable();
    }
}
