using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private float _jumpForce = 400;

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * _speed;
        var rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(horizontal, rigidbody2D.velocity.y);

        var animator = GetComponent<Animator>();
        bool walking = horizontal != 0;
        animator.SetBool("Walk", walking);

        if (horizontal != 0)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = horizontal < 0;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            rigidbody2D.AddForce(Vector2.up * _jumpForce);
        }
    }
}