using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    public float maxHealth = 10f;
    public float health;
    public float moveSpeed = 1f;
    public StatusBarPlayer healthBar;
    public PlayerHealthDisplay healthDisplay;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public Sword_Attack swordAttack;
    public GameObject floatingPoints;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;      
        healthBar.SetHealth(health, maxHealth);
    }
    
    //health handler
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.SetHealth(health, maxHealth);
        healthDisplay.SetSize(health);
        if (health < .3f)
        {
            //under 30% health
            if ((health * 100f) % 3 == 0)
            {
                healthDisplay.SetColor(Color.white);
            }
            else
            {
                healthDisplay.SetColor(Color.red);
            }
        }

        GameObject hitPoints = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
        hitPoints.transform.GetChild(0).GetComponent<TextMesh>().text = "" + damageAmount + "[" + health + "]";
        if (health <= 0)
        {
            animator.SetTrigger("defeated");
        }
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {

                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // Set direction of sprite to movement direction
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
        }

    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    //attack handler
    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
    public void Defeated()
    {
        animator.SetTrigger("defeated");
    }

    public void RemovePlayer()
    {
        Destroy(gameObject);
    }
}
