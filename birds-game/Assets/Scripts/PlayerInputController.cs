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
        private bool _isFlying = false;
        private bool _isCrawling = false;
        private const float DEFAULT_SCALE_VALUE = 0.5f;
        private const int JUMP_ANIM_SLOW_COEFF = 10;
        private const float NORMAL_ANIM_SPEED = 1f;
        private GameObject _seagullGrounded;
        private GameObject _seagullFliying;

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
            _seagullFliying = transform.Find("Seagull_air").gameObject;
            _seagullGrounded = transform.Find("Seagull_ground").gameObject;
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
                _isCrawling = true;
            };
            _input.Player.Crawl.canceled += _ => 
            {
                _walkingSpeed = _character.WalkingSpeed;
                _isCrawling = false;
            };
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
                if(_isCrawling)
                {
                    _animator.speed = _walkingSpeed;
                    _animator.Play("Seagull_Crouch");
                }
                //if moveDirection is 0, we're standing still
                else if(moveDirection == 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Seagull_Idle"))
                {
                    _animator.speed = NORMAL_ANIM_SPEED;
                    _animator.Play("Seagull_Idle");
                }
                //if moveDirection== -1 we're moving left, and with moveDirection == 1 we're moving right
                else if(moveDirection != 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Seagull_Walk"))
                {
                    _animator.speed = _walkingSpeed;
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
            if(_isGrounded)
            {
                _isFlying = false;
                _seagullGrounded.SetActive(true);
                _seagullFliying.SetActive(false);
                _rigidbody2D.gravityScale = 1f;
            }
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            _isGrounded = collision.collider.gameObject.layer != LayerMask.NameToLayer("World");
        }
        private void HandleFlight()
        {
            if(!_isFlying)
            {
                _isFlying = true;
                _isGrounded = false;
                _seagullGrounded.SetActive(false);
                _seagullFliying.SetActive(true);
                _rigidbody2D.gravityScale = 0.5f;
            }            
        }
        private void Jump()
        {
            if(!_isGrounded)
            {
                HandleFlight();
            }
            _isCrawling = false;
            _isGrounded = false;
            _isFlying = false;
            
            _rigidbody2D.AddForce(Vector2.up * _character.JumpPower, ForceMode2D.Impulse);
        }
        private void Interact()
        {
            var layerId = LayerMask.NameToLayer("Interactable");
            var interactable = Physics2D.OverlapCircle(transform.position, IInteractable.INTERACTION_RADIUS, ~layerId).GetComponent<IInteractable>();
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
