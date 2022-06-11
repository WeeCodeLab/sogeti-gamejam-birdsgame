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
        private bool _isGrounded = true;
        private const float DEFAULT_SCALE_VALUE = 0.5f;
        private const int JUMP_ANIM_SLOW_COEFF = 10;

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
                Flip(DEFAULT_SCALE_VALUE);
            }
            else if(moveDirection == -1 && _facingRight)
            {
                Flip(-DEFAULT_SCALE_VALUE);
            }

            SetAnimations(moveDirection);
            _rigidbody2D.velocity = new Vector2(moveDirection * _walkingSpeed, _rigidbody2D.velocity.y);
        }
        private void SetAnimations(float moveDirection)
        {            
            if(_isGrounded)
            {
                if(moveDirection == 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Seagull_Idle"))
                {
                    _animator.speed = 1f;
                    _animator.Play("Seagull_Idle");
                }
                else if(moveDirection != 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Seagull_Walk"))
                {
                    _animator.speed = _character.WalkingSpeed;
                    _animator.Play("Seagull_Walk");
                }
            }
            else
            {
                _animator.speed = _character.JumpPower / JUMP_ANIM_SLOW_COEFF;
                _animator.Play("Seagull_Jump");
            }
        }
        private void Flip(float scale)
        {
            _facingRight = !_facingRight;
            var currentScale = transform.localScale;
            currentScale.x  = scale;
            transform.localScale = currentScale;
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            _isGrounded = collision.collider.gameObject.layer == LayerMask.NameToLayer("World");
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            _isGrounded = collision.collider.gameObject.layer != LayerMask.NameToLayer("World");
        }

        private void Jump()
        {
            if (!_isGrounded) return;

            _rigidbody2D.AddForce(Vector2.up * _character.JumpPower, ForceMode2D.Impulse);
        }
        private void Interact()
        {
            var layerId = LayerMask.NameToLayer("Interactable");
            var interactable = Physics2D.OverlapCircle(transform.position, 2f, ~layerId).GetComponent<IInteractable>();
            interactable.Interact();
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
