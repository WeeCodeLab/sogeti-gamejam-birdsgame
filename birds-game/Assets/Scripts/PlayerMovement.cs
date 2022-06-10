using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace birds_game.Assets.Scripts.Characters
{
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
            RegisterInput();
        }

        // Update is called once per frame
        private void Update()
        {
            Move();
        }
        private void RegisterInput()
        {
            _input.Player.Jump.started += _ => Jump();
            _input.Player.Interact.started += _ => Interact();
            _input.Player.Crawl.started += _ => _movementSpeed = 2.5f;
            _input.Player.Crawl.canceled += _ => _movementSpeed = 5f;
        }

        private void Move()
        {
            var moveDirection = _input.Player.Move.ReadValue<float>();
            _rigidbody2D.velocity = new Vector2(moveDirection * _movementSpeed, _rigidbody2D.velocity.y);
        }

        private void Jump()
        {
            if (!IsGrounded()) return;

            _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
        private void Interact()
        {
            var layerId = LayerMask.NameToLayer("Interactable");
            var interactable = Physics2D.OverlapCircle(transform.position, 2f, ~layerId).GetComponent<IInteractable>();
            interactable.Interact();
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

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
        #endif
    }
}
