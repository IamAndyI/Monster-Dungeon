using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour, ISaveable
{

    [Header("Stats")]
    public float moveSpeed = 2f;
    public float healthVal = 80f;

    [Header("Movement")]
    public bool facing = true;
    private bool canMove = true;
    private float horizontal;
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public LayerMask playerLayer;
    //  Collider2D collider;
    private float minX = -8;
    private float maxX = 8;
    private float minY = -9;
    private float maxY = 6;
    Vector3 pos;
    RaycastHit2D findTargetCircle;
    [SerializeField]float searchRadius = 0.5f;

    [Header("GameObjects")]
    [SerializeField] Slider minionSlider;
    private GameObject boss;
    public GameManager gameManager;
    public PlayerManager playerManager;
    [SerializeField] private GameObject target;

    AudioSource myAudioSource;
    [SerializeField] AudioClip attackClip;
    MusicPlayer musicPlayer;

    [Header("Attack")]
    private float timeSinceLastHit;
    private float timeSinceLastMove;
    private float timeBetweenMoves = 3f;
    private float untargetedAttackDir = 1f;
    private float radius = 0.5f;
    public float dmg = 10;
    bool isAttacking = false;
    Animator myAnimator;
    private Target myTarget;
    public bool alreadySetHealth = false;

    [System.Serializable]
    public struct Savedata
    {
        public float healthVal;
        public bool alreadySethealth;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Find objects in scene
        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        boss = FindObjectOfType<Boss>().gameObject;
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myTarget = GetComponent<Target>();
        findTargetCircle = Physics2D.CircleCast(transform.position, searchRadius, Vector2.zero);
        musicPlayer = FindObjectOfType<MusicPlayer>();
        myAudioSource.volume = musicPlayer.volume;

        if(playerManager.isEndlessMode)
        {
            maxY = 3;
            minX = -17;

            minY = -10;
            maxX = 17;
        }


        if (!alreadySetHealth)
        {
            healthVal += healthVal + boss.GetComponent<Boss>().leadershipValue;
            myTarget.healthVal = healthVal;
            SetHealthBar();
            alreadySetHealth = true;
        }
    }

    public void SetHealthBar()
    {
        myTarget.targetSlider.maxValue = myTarget.healthVal;
        myTarget.targetSlider.value = myTarget.healthVal;
    }

    // Update is called once per frame
    void Update()
    {
        //Variables to determine when the enemy can attack again
        timeSinceLastHit += Time.deltaTime;
        timeSinceLastMove += Time.deltaTime;

        if(boss.GetComponent<Target>().isDead)
        {
            Die();
        }

        //If a target is found
        if (target != null)
        {
            //If the enemy is not facing the target and the target is to the right flip to the right
            //If Target x position is greater then target is to the right
            if (target.transform.position.x > transform.position.x && !facing)
            {
                Flip();
            }//Else target is to the left, flip enemy to the left
            else if (target.transform.position.x < transform.position.x && facing)
            {
                Flip();
            }

            MoveToTarget();
          
        }
        else// If there is no target found
        {
            isAttacking = false;
            myAnimator.SetBool("IsAttacking", false);

            //If there is no target then face the boss instead
            if (boss.transform.position.x > transform.position.x && !facing)
            {
                Flip();
            }//Else target is to the left, flip enemy to the left
            else if (boss.transform.position.x < transform.position.x && facing)
            {
                Flip();
            }
           
            //Move Randomly, search for a target, and attack if player is in range
            MoveRandomly();
        }
    }

    //Called when an enemy has targeted this minion so this minion will target the enemy back
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void MoveToTarget()
    {
        //Change X pos later
        Vector3 targetPos = new Vector3(target.transform.position.x + TargetLeftOrRight(), target.transform.position.y, transform.position.y);

        //If move point is not colliding with something then move
        if (canMove)
        {
            //Move towards the targets y position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        if (transform.position == targetPos && target != null)
        {
           
            //Attack here
            isAttacking = true;
            myAnimator.SetBool("IsWalking", false);
            myAnimator.SetBool("IsAttacking", true);

        }
        else
        {
            myAnimator.SetBool("IsAttacking", false);
            isAttacking = false;
        }

    }

    //Called if the enemy dies
    public void TargetDefeated()
    {
        target = null;
    }

    public void FindTarget()
    {

        if(Vector2.Distance(transform.position, target.transform.position) < 2)
        {
            isAttacking = true;
            myAnimator.SetBool("IsAttacking", true);
        }
        
    }

    public void Attack()
    {
        myAudioSource.clip = attackClip;
        myAudioSource.Play();
        if (target != null)
        {
            target.GetComponent<EnemyMovement>().LoseHealth(boss.GetComponent<Boss>().leadershipValue);
        }
       
    }

    public void MoveRandomly()
    {
        //Random points to move to
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);

        //If time to move again
        if (timeBetweenMoves < timeSinceLastMove)
        {
            //Makes new position to move to
            Vector3 randPos = new Vector3(randX, randY);
            pos = randPos;
            timeSinceLastMove = 0;
        }

        //Move to random position
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);

        if (transform.position == pos)
        {
            myAnimator.SetBool("IsWalking", false);
        }
        else
        {
            myAnimator.SetBool("IsWalking", true);
        }
    }

    //Returns a value to have enemy stand one to the left or right of their target
    private int TargetLeftOrRight()
    {
        //If target is to the right of enemy, return -1
        if (target.transform.position.x > transform.position.x)
        {
            return -1;
        }
        else if (target.transform.position.x < transform.position.x)
        {
            return 1;
        }
        return 0;
    }


    //Flips sprite to face target
    private void Flip()
    {
        facing = !facing;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

   


    //Called when enemy is at 0 health
    public void Die()
    {
        //Destroys enemy
        Destroy(gameObject);
    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata() { healthVal = this.healthVal, alreadySethealth = this.alreadySetHealth });
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);
        healthVal = savedata.healthVal;
        alreadySetHealth = savedata.alreadySethealth;
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
