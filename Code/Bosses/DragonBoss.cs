using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lowscope.Saving;

public class DragonBoss : MonoBehaviour, ISaveable
{
    [Header("Boss Stats")]
    [SerializeField] public float bossNum = 2;
    [SerializeField] public float health = 100;
    [SerializeField] public float moveSpeed = 5;
    [SerializeField] public float attackDamage = 10;
    [SerializeField] public float specialDamage = 1f;
    [SerializeField] public float powerValue = 30;
    [SerializeField] public float leadershipValue = 1;
    public float attackRange = 2f;
    float startingHealth = 0;
    float startingAttack = 0;
    float startingPower = 0;
    float startingLeadership = 0;

    [Header("Movement")]
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    private Vector3 dirFacing;
    public bool facing = false;
    public float horizontal;

    public Joystick joyStick;
    RaycastHit2D[] hit;
    RaycastHit2D[] fireHit;
    float spinRadius = 3;

    private Boss myBossScript;
    public BossShop bossShop;
    private Target myTarget;
    GameManager gameManager;

    private Animator myAnimator;

    [SerializeField] GameObject specialButton;
    private bool attacking = false;
    private bool specialAttacking = false;
    private bool canSpecial = false;
    private bool moving = false;

    AudioSource myAudioSource;
    //   [SerializeField] AudioClip specialClip;
    [SerializeField] AudioClip attackClip;
    MusicPlayer musicPlayer;

    float timeSinceLastSpecial = 3;
    float timeBetweenSpecial = 3f;

    [SerializeField] SaveLoader saveLoader;

    public bool alreadySetStats = false;


    [System.Serializable]
    public struct Savedata
    {
        public float health;
        public float attackDamage;
        public float specialDamage;
        public float powerValue;
        public float leadershipValue;
        public bool alreadySetStats;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spinRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        startingHealth = health;
        startingAttack = attackDamage;
        startingPower = powerValue;
        startingLeadership = leadershipValue;

        movePoint.parent = null;
        myAnimator = GetComponent<Animator>();
        myBossScript = GetComponent<Boss>();
        myAudioSource = GetComponent<AudioSource>();
        bossShop = FindObjectOfType<BossShop>();
        myTarget = gameObject.GetComponent<Target>();
        gameManager = FindObjectOfType<GameManager>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        myAudioSource.volume = musicPlayer.volume;
        dirFacing = Vector2.right;

        bossShop.boss = GetComponent<Boss>();
        bossShop.bossTarget = GetComponent<Target>();
        bossShop.UpdateStatUI();


        if (!alreadySetStats)
        {
            SetBossStats();
        }
        myTarget.healthVal = myTarget.maxHealth;
        myTarget.targetSlider.maxValue = myTarget.healthVal;
        myTarget.targetSlider.value = myTarget.healthVal;
    }


    // Update is called once per frame
    void Update()
    {
        //Cast a ray from self in the direction player is facing
        hit = Physics2D.RaycastAll(transform.position, dirFacing, attackRange);
        fireHit = Physics2D.RaycastAll(transform.position, dirFacing, attackRange + 10);
        Debug.DrawRay(transform.position, attackRange * dirFacing, Color.green);

        if (Input.GetKeyDown(KeyCode.E))
        {
            AttackButton();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canSpecial)
            {
                SpecialButton();
            }
        }

        Movement();


        if (!moving && !attacking && !specialAttacking)
        {

            myAnimator.Play("Dragon idle");                                /* Change Animation Name */
            myAnimator.SetBool("IsWalking", false);
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

        if (timeSinceLastSpecial > timeBetweenSpecial)
        {
            specialButton.GetComponent<Button>().interactable = true;
            canSpecial = true;
        }
        timeSinceLastSpecial += Time.deltaTime;

    }

    public void Movement()
    {


        if (!attacking && !specialAttacking && !gameManager.gamePaused)
        {

            //Horizontal value used to determine which way the player is facing based on joystick movement
            horizontal = Input.GetAxis("Horizontal");

            //Move player towards direction of the move point
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);


            //If distance between player and and move point is greater than or equal to .05
            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                //If the player is moving the joystick left or right
                if (Mathf.Abs(/*joyStick.Horizontal*/ Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    moving = true;
                    myAnimator.SetBool("IsWalking", true);

                    //If the area where the player is moving to can be moved to
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(/*joyStick.Horizontal*/Input.GetAxisRaw("Horizontal"),
                        0f, 0f), .2f, whatStopsMovement))
                    {
                        //Move the movepoint the direction of the joystick
                        movePoint.position += new Vector3(/*joyStick.Horizontal*/Input.GetAxisRaw("Horizontal"), 0f, 0f);

                    }
                }
                // If the player is moving the joystick up or down
                else if (Mathf.Abs(/*joyStick.Vertical*/Input.GetAxisRaw("Vertical")) == 1f)
                {
                    moving = true;
                    myAnimator.SetBool("IsWalking", true);

                    //If the area where the player is moving to can be moved to
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, /*joyStick.Vertical*/ Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                    {
                        //Move the movepoint the direction of the joystick
                        movePoint.position += new Vector3(0f, /*joyStick.Vertical*/ Input.GetAxisRaw("Vertical"), 0f);

                    }
                }
                else
                {
                    myAnimator.SetBool("IsWalking", false);
                }


            }
        }
    }

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
       
        attacking = true;
        myAnimator.SetBool("IsWalking", false);
        myAnimator.SetBool("IsAttacking", true);
        // myAnimator.Play("Attack");

    }

    public void SpecialButton()
    {
        canSpecial = false;
        specialButton.GetComponent<Button>().interactable = false;
        timeSinceLastSpecial = 0;
        myAnimator.SetBool("IsSpecialAttacking", true);
        myAnimator.Play("Fire Breath");                            /* Add Special anim */
    }

    public void FireOver()
    {
        specialAttacking = false;
        myAnimator.SetBool("IsSpecialAttacking", false);
    }

    public void Pause()
    {
        gameManager.PauseGame();
    }

    public void SetBossStats()
    {
        myTarget.healthVal += health;
        myTarget.maxHealth = myTarget.healthVal;
        myBossScript.attackDamage += attackDamage;
        myBossScript.powerValue += powerValue;
        myBossScript.leadershipValue += leadershipValue;
        alreadySetStats = true;
    }

    public void Attack()
    {
        myAudioSource.clip = attackClip;
        myAudioSource.Play();
        

        //Hits all enemies hitting the ray for testing until animation
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null && hit[i].collider.gameObject.tag == "Enemy")
            {
                hit[i].collider.gameObject.GetComponent<EnemyMovement>().LoseHealth(myBossScript.attackDamage);
            }
        }
        //After attack stop attacking
        attacking = false;
        myAnimator.SetBool("IsAttacking", false);
    }

    public void SpecialAttack()
    {
        myAudioSource.clip = attackClip;
        myAudioSource.Play();


        //Hits all enemies hitting the ray for testing until animation
        for (int i = 0; i < fireHit.Length; i++)
        {
            if (fireHit[i].collider != null && fireHit[i].collider.gameObject.tag == "Enemy")
            {
                fireHit[i].collider.gameObject.GetComponent<EnemyMovement>().LoseHealth(myBossScript.powerValue);
            }
        }
       
    }


    public void Die()
    {
        saveLoader.dbAlreadyLoaded = false;
        gameManager.wasInGame = false;
        myBossScript.isDead = true;
        alreadySetStats = false;
        health = startingHealth;
        attackDamage = startingAttack;
        powerValue = startingPower;
        leadershipValue = startingLeadership;
        bossShop.kgGold = 0;
    }


    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata()
        {
            health = this.health,
            attackDamage = this.attackDamage,
            leadershipValue = this.leadershipValue,
            powerValue = this.powerValue,
            alreadySetStats = this.alreadySetStats
        });
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);

        health = savedata.health;
        attackDamage = savedata.attackDamage;
        leadershipValue = savedata.leadershipValue;
        powerValue = savedata.powerValue;
        alreadySetStats = savedata.alreadySetStats;
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
