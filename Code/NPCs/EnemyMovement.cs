using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float healthVal = 100f;
    public float goldVal = 25f;
    public bool isChampion = false;
    private float healthDropChance = 20f;

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

    [Header("GameObjects")]
    [SerializeField] Slider enemySlider;
    [SerializeField] GameObject enemyCanvas;
    [SerializeField] public GameObject healthBarBorder;
    [SerializeField] public GameObject healthUp;
    private GameObject boss;
    public GameManager gameManager;
    [SerializeField] private GameObject target;
    [SerializeField] public GameObject gemPrefab;

    AudioSource myAudioSource;
    [SerializeField] AudioClip attackClip;
    MusicPlayer musicPlayer;

    [Header("Attack")]
    private float timeSinceLastHit;
    private float timeSinceLastMove;
    private float timeBetweenMoves = 3f;
    private float untargetedAttackDir = 1f;
    private float radius = 1f;
    public float dmg = 10;
    public bool isAttacking = false;
    public bool isUntargetedAttacking = false;
    RaycastHit2D unTargetedHit;

    public WaveManager waveManager;
    Animator myAnimator;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Find objects in scene
        gameManager = FindObjectOfType<GameManager>();
        boss = FindObjectOfType<Boss>().gameObject;
        waveManager = FindObjectOfType<WaveManager>();
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        myAudioSource.volume = musicPlayer.volume;
        SetHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myAnimator.GetBool("IsDead"))
        {
            //Variables to determine when the enemy can attack again
            timeSinceLastHit += Time.deltaTime;
            timeSinceLastMove += Time.deltaTime;


            //If a target is found
            if (target != null && !isUntargetedAttacking)
            {
                //Check if target is still alive, important for targets that despawn such as ghost clone
                if(target.GetComponent<Target>().isDead)
                {
                    gameManager.players.Remove(target);

                   
                    target.GetComponent<Target>().Die();
                    target = null;
                    myAnimator.SetBool("IsAttacking", false);
                    isAttacking = false;
                }

                //Stops walk animation when attacking
                if (!isAttacking)
                {
                    myAnimator.SetBool("IsWalking", true);
                }
                else
                {
                    myAnimator.SetBool("IsWalking", false);
                }

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

                }

                if (target != null && target.GetComponent<Target>().isDead)
                {

                    myAnimator.SetBool("IsAttacking", false);
                    isAttacking = false;
                    gameManager.players.Remove(target);

                    
                    target.GetComponent<Target>().Die();
                    target = null;
                }

                    //Move to target
                    MoveToTarget(target);
                
            }
            else// If there is no target found
            {

                //If there is no target then face the boss instead
                if (boss.transform.position.x > transform.position.x && !facing)
                {
                    Flip();
                    untargetedAttackDir = 1f;
                }//Else target is to the left, flip enemy to the left
                else if (boss.transform.position.x < transform.position.x && facing)
                {
                    Flip();
                    untargetedAttackDir = -1f;
                   
                }

                if (!isAttacking)
                {
                    //Move Randomly, search for a target, and attack if player is in range
                    MoveRandomly();
                    FindTarget();

                }
                else
                {
                    myAnimator.SetBool("IsWalking", false);
                }


            }


            UntargetedAttack();
        }
        
    }

    public void SetHealthBar()
    {
        enemySlider.maxValue = healthVal;
        enemySlider.value = healthVal;
        SetHealthBarDifficultyColor();
    }

    //Sets color of healthbar border based on wave number
    public void SetHealthBarDifficultyColor()
    {
        if (waveManager.waveNum <= 25)
        {
            healthBarBorder.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (waveManager.waveNum > 25 && waveManager.waveNum <= 50)
        {
            healthBarBorder.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (waveManager.waveNum > 50 && waveManager.waveNum <= 75)
        {
            healthBarBorder.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (waveManager.waveNum > 75 && waveManager.waveNum <= 100)
        {
            healthBarBorder.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else if (waveManager.waveNum > 100)
        {
            healthBarBorder.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {

        }
    }


    //Moves to target
    private void MoveToTarget(GameObject target)
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
            myAnimator.SetBool("IsAttacking", true);
           
            }
            else
            {
            myAnimator.SetBool("IsAttacking", false);
            isAttacking = false;
            }
        
    }

    //Moves enemy randomly when no target is found
    public void MoveRandomly()
    {
        //Random points to move to
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);
       
        //If time to move again
        if(timeBetweenMoves < timeSinceLastMove)
        {
            //Makes new position to move to
            Vector3 randPos = new Vector3(randX, randY);
            pos = randPos;
            timeSinceLastMove = 0;
        }

        //Move to random position
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);

        if(transform.position == pos)
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
        if(target.transform.position.x > transform.position.x)
        {
            return -1;
        }
        else if(target.transform.position.x < transform.position.x)
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

        Vector3 canvasScale = enemyCanvas.transform.localScale;
        canvasScale.x *= -1;
        enemyCanvas.transform.localScale = canvasScale;
    }

    //Searches for target
  public void FindTarget()
    {
        //Checks through all players in game which is stored in the gamemanager
        for (int i = 0; i < gameManager.players.Count; i++)
        {
            //Gets the script of each player
            Target targetScript = gameManager.players[i].GetComponent<Target>();

            //Checks each script to see if they are targeted or not
            //If not targeted
            if (!targetScript.isBeingTargeted)
            {
                targetScript.isBeingTargeted = true;
                //This player becomes targeted 
                target = gameManager.players[i];

                //If the target is a minion
                if(targetScript.isMinion == true)
                {
                    //Have minion target this enemy
                    targetScript.GetComponent<Minion>().SetTarget(gameObject);
                }
                
                break;
            }
        }
    }

    //Activates enemy attack when attack is allowed
    public void Attack()
    {
        //Make attack animation and do damage during that, this is for testing
        myAudioSource.clip = attackClip;
        myAudioSource.Play();

        if (target != null && target.GetComponent<Target>().isDead)
            {

                myAnimator.SetBool("IsAttacking", false);
                isAttacking = false;
                gameManager.players.Remove(target);

            

           target.GetComponent<Target>().Die();
                target = null;
            }
            else if (target != null)
            {
                target.GetComponent<Target>().LoseHealth(dmg);
            }
       
        
    }

    //Activated when player goes near enemy without a target
    public void UntargetedAttack()
    {

        //Check if boss is nearby to attack
        
        Vector2 dirToFace = new Vector2(untargetedAttackDir, 0);
        unTargetedHit = Physics2D.Raycast(transform.position, dirToFace, 1, playerLayer);

        if (target == null && Physics2D.CircleCast(transform.position, radius, Vector2.zero, 1, playerLayer).collider != null)
        {
            target = Physics2D.CircleCast(transform.position, radius, Vector2.zero, 1, playerLayer).collider.gameObject;
            isAttacking = true;
            isUntargetedAttacking = true;
            myAnimator.SetBool("IsAttacking", true);
           
        }
        else if(Physics2D.CircleCast(transform.position, radius, Vector2.zero, 1, playerLayer).collider == null && isUntargetedAttacking == true)
        {
            myAnimator.SetBool("IsAttacking", false);
            target = null;
           isAttacking = false;
            isUntargetedAttacking = false;
        }
    }

    //Called when enemy is attacked by a player
    //Makes enemy lose health
    public void LoseHealth(float dmg)
    {
        if (!myAnimator.GetBool("IsDead"))
        {
            StartCoroutine(HitColor());
            
            healthVal -= dmg;
            enemySlider.value = healthVal;

            if (healthVal <= 0)
            {
                Die();
            }
           // GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    IEnumerator HitColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void TrySpawnHealthUP()
    {
        float val =  Random.Range(0, 100);

        if(val <= healthDropChance)
        {
            Instantiate(healthUp, transform.position, Quaternion.identity);
        }
    }

    //Called when enemy is at 0 health
    public void Die()
    {
        //Give player money moves
        gameManager.bossShop.AddGold(goldVal);
        myAnimator.SetBool("IsWalking", false);
        myAnimator.SetBool("IsAttacking", false);
        myAnimator.SetBool("IsDead", true);
        TrySpawnHealthUP();

        if (target != null)
        {
            //If the target is a minion
            if (target.GetComponent<Target>().isMinion == true)
            {
                //Have minion untarget this enemy
                target.GetComponent<Target>().GetComponent<Minion>().TargetDefeated();
            }

            //Untargets the enemy's target 
            target.GetComponent<Target>().isBeingTargeted = false;
        }

        if(isChampion)
        {
            Instantiate(gemPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            waveManager.enemiesLeft -= 1;
            waveManager.numEnemiesInScene--;
        }
        
        //Destroys enemy
        Destroy(gameObject, 1);
    }

}
