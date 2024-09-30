using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;  // Movement speed
    [SerializeField] private float dodgeForce = 20f;
    [SerializeField] private float dodgeDuration = 0.2f;  // How long the dodge lasts
    [SerializeField] private float dodgeCooldown = 1f;  // How long until you can use the dodge again

    private Rigidbody2D rb;  // Reference to Rigidbody2D
    private bool isDodging = false;
    private bool canDodge = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            MovePlayer();
        }
    }

    private void HandleInput()
    {
        // Check for dodge input
        if (Input.GetButtonDown("Jump") && !isDodging && canDodge)
        {
            StartDodge();
        }
    }

    private void MovePlayer()
    {
        // Capture movement input
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Set velocity based on movement direction
        rb.velocity = moveDirection * movementSpeed; //(moveDirection != Vector2.zero ? movementSpeed : 0);
    }

    private void StartDodge()
    {
        Vector2 dodgeDirection = GetDodgeDirection();
        StartCoroutine(Dodge(dodgeDirection));
    }

    private Vector2 GetDodgeDirection()
    {
        // Determine dodge direction based on current input
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        return moveDirection == Vector2.zero ? Vector2.right : moveDirection;
    }

    private IEnumerator Dodge(Vector2 dodgeDirection)
    {
        isDodging = true;
        canDodge = false;
        rb.velocity = dodgeDirection * dodgeForce;  // Apply dodge force
        yield return new WaitForSeconds(dodgeDuration);  // Wait for dodge duration
        isDodging = false;  // Reset dodge
        yield return new WaitForSeconds(dodgeCooldown);  // Wait for dodge cooldown
        canDodge = true;
    }
}