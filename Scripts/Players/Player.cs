using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpVelocity = 5.5f;
    [SerializeField] int _maxJumps = 2;
    [SerializeField] float _maxJumpDuration = 0.2f;
    [SerializeField] float _downPull = 0.06f;
    [SerializeField] Transform _feet;

    Vector2 _startPosition;
    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;

    void Start()
    {
        _startPosition = transform.position;  // save start position
        _jumpsRemaining = _maxJumps;  // set amount of jumps
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * _speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();

        // Walk
        if (Mathf.Abs(horizontal) >= 1)
        {
            rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);
        }
        
        // Walk Animation
        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        // Flip Sprite & Animation when Walking to Left
        if (horizontal != 0)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }

        // Ground Check
        var hitGround = Physics2D.OverlapCircle(_feet.position, 0.1f, LayerMask.GetMask("Ground"));
        bool isGrounded = hitGround != null;

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            // Start Jump
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _jumpsRemaining--;
            _fallTimer = 0;
            _jumpTimer = 0;
        }
        else if (Input.GetButton("Fire1") && _jumpTimer <= _maxJumpDuration)
        {
            // Hold Jump
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, _jumpVelocity);
            _fallTimer = 0;
            _jumpTimer += Time.deltaTime;
        }

        if (isGrounded)
        {
            // reset fall timer + jumps remaining
            _fallTimer = 0;
            _jumpsRemaining = _maxJumps;
        }
        else
        {
            // add fall speed
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y - downForce);
        }
    }

    internal void ResetToStart()
    {
        transform.position = _startPosition;
    }
}