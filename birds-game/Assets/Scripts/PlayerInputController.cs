using birds_game.Assets.Scripts.Characters;
using UnityEngine;

namespace birds_game.Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInputController : MonoBehaviour
    {
        private BirdCharacter _character;
        private float _walkingSpeed;
        private Input _input;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private bool _facingRight = true;

        private void Start()
        {
            GameManager.Init();
            _character = GameManager.Instance.GetCurrentBirdCharacter();
            _walkingSpeed = _character.WalkingSpeed;
            _input = new Input();
            _input.Enable();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
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
            _input.Player.Crawl.started += _ => 
            {
                _walkingSpeed = _character.WalkingSpeed / 2; 
                //TODO: change current to crawling animation
            };
            _input.Player.Crawl.canceled += _ => 
            {
                _walkingSpeed = _character.WalkingSpeed;
                //TODO: change current animation to normal walking
            };
            //TODO: add bird character swapping here and setting of _walkingSpeed
        }

        private void Move()
        {
            var moveDirection = _input.Player.Move.ReadValue<float>();
            if(moveDirection == 1 && !_facingRight)
            {
                Flip();
            }
            else if(moveDirection == -1 && _facingRight)
            {
                Flip();
            }
            _rigidbody2D.velocity = new Vector2(moveDirection * _walkingSpeed, _rigidbody2D.velocity.y);
        }
        private void Flip()
        {
            _facingRight = !_facingRight;
            var currentScale = transform.localScale;
            currentScale.x  *= -1;
            transform.localScale = currentScale;
        }

        private void Jump()
        {
            if (!IsGrounded()) return;

            _rigidbody2D.AddForce(Vector2.up * _character.JumpPower, ForceMode2D.Impulse);
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
