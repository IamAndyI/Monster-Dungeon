using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float moveSpeed = 6f;

    [Header("Movement")]
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    private Vector2 dirFacing;
    public bool facing = true;
    private float horizontal;
    private float vertical;
    float moveLimiter = 0.7f;

    public Joystick joyStick;
    RaycastHit2D[] hit;
    Rigidbody2D body;


    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        body = GetComponent<Rigidbody2D>();
    }

 
    // Update is called once per frame
    void Update()
    {
        //Cast a ray from self in the direction player is facing
        hit = Physics2D.RaycastAll(transform.position, dirFacing, attackRange);
        Debug.DrawRay(transform.position, attackRange * dirFacing, Color.green);

        Movement();

        if(Input.GetKey("E"))
        {
            AttackButton();
        }

        //Determines which way the player is moving and faces them that direction
        if (horizontal > 0 && !facing)
        {
            Flip();
            dirFacing = Vector2.right;
        }
        else if (horizontal < 0 && facing)
        {
            Flip();
            dirFacing = Vector2.left;
        }
    }

    public void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }

    //***This movement for moblie version***
    /*
    public void Movement()
    {
        
        //Horizontal value used to determine which way the player is facing based on joystick movement
        horizontal = joyStick.Horizontal;

        //Move player towards direction of the move point
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);


        //If distance between player and and move point is greater than or equal to .05
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            //If the player is moving the joystick left or right
            if (Mathf.Abs(joyStick.Horizontal) == 1f)
            {
                //If the area where the player is moving to can be moved to
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(joyStick.Horizontal, 0f, 0f), .2f, whatStopsMovement))
                {
                    //Move the movepoint the direction of the joystick
                    movePoint.position += new Vector3(joyStick.Horizontal, 0f, 0f);
                }
            }

            // If the player is moving the joystick up or down
            if (Mathf.Abs(joyStick.Vertical) == 1f)
            {
                //If the area where the player is moving to can be moved to
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, joyStick.Vertical, 0f), .2f, whatStopsMovement))
                {
                    //Move the movepoint the direction of the joystick
                    movePoint.position += new Vector3(0f, joyStick.Vertical, 0f);
                }
            }

        }
    }*/

    //Flips the player sprite
    private void Flip()
    {
        facing = !facing;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //If pressed activates attack animation
    public void AttackButton()
    {
        //Hits all enemies hitting the ray for testing until animation
        for(int i = 0; i < hit.Length; i++)

        if (hit[i].collider != null && hit[i].collider.gameObject.tag == "Enemy")
        {
           
            hit[i].collider.gameObject.GetComponent<EnemyMovement>().LoseHealth(attackDamage);
        }
    }
}
