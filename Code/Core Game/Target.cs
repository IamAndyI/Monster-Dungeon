using System.Collections;
using Lowscope.Saving;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour, ISaveable
{
    public bool isBeingTargeted = false;
    public bool isDead = false;
    public float healthVal = 100f;
    public float maxHealth;
    float kgMissChance = 25f;

    GameManager gameManager;
    public PlayerManager playerManager;
    [SerializeField] public Slider targetSlider;
    public bool isBoss = false;
    public bool isMinion = false;
    SceneLoader sceneLoader;

    AudioSource myAudioSource;
  

    [System.Serializable]
    public struct Savedata
    {
        public float healthVal;
        public float maxHealth;
    }

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        gameManager = FindObjectOfType<GameManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        gameManager.players.Add(gameObject);

        if(gameObject.GetComponent<Boss>() != null)
        {
            myAudioSource = GetComponent<AudioSource>();
            isBoss = true;
        }
        else if(gameObject.GetComponent<Minion>() != null)
        {
            isMinion = true;
        }
    }

    private void Update()
    {
        if(isBoss)
        {

        }
    }

    public void GainHealth(float health)
    {
        if(healthVal <= maxHealth)
        {
            healthVal += health;
            targetSlider.value = healthVal;
            StartCoroutine(HealthUpColor());
        }else if(healthVal > maxHealth)
        {
            healthVal = maxHealth;
            targetSlider.value = healthVal;
        }
       
    }


    public void LoseHealth(float dmg)
    {
        if(GetComponent<KingGhost>())
        {
            KingGhostHit(dmg);
           
        }
        else
        {
            healthVal -= dmg;
            targetSlider.value = healthVal;
            StartCoroutine(HitColor());

            if (healthVal <= 0)
            {
                isDead = true;

            }
        }
    }

    private void KingGhostHit(float dmg)
    {
        float randVal = Random.Range(1, 100);
        if (kgMissChance >= randVal)
        {
            print("Hit Missed");
            return;
        }
        else
        {
            healthVal -= dmg;
            targetSlider.value = healthVal;
            StartCoroutine(HitColor());

            if (healthVal <= 0)
            {
                isDead = true;

            }
        }
    }

    IEnumerator HitColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator HealthUpColor()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

   

    public void Die()
    {
        if (isBoss)
        {

            if (playerManager.goblinKing)
            {
                GoblinKing goblinking = FindObjectOfType<GoblinKing>();
                goblinking.Die();
                gameManager.waveManager.waveNum = 1;
                gameManager.wasInGame = false;
                playerManager.gkInEndlessMode = false;

                sceneLoader.LoadMainMenu();
                Destroy(gameObject);
            }
            else if(playerManager.giantSkeleton)
            {
                GiantSkeleton giantSkeleton = FindObjectOfType<GiantSkeleton>();
                giantSkeleton.Die();
                gameManager.waveManager.waveNum = 1;
                gameManager.wasInGame = false;
                playerManager.gsInEndlessMode = false;
                sceneLoader.LoadMainMenu();
                Destroy(gameObject);
            }
            else if(playerManager.kingGhost)
            {
                KingGhost kingGhost = FindObjectOfType<KingGhost>();
                kingGhost.Die();
                gameManager.waveManager.waveNum = 1;
                gameManager.wasInGame = false;
                playerManager.kgInEndlessMode = false;
                sceneLoader.LoadMainMenu();
                Destroy(gameObject);
            }
            else if(playerManager.dragonBoss)
            {
                DragonBoss dragonBoss = FindObjectOfType<DragonBoss>();
                dragonBoss.Die();
                gameManager.waveManager.waveNum = 1;
                gameManager.wasInGame = false;
                playerManager.dbInEndlessMode = false;
                sceneLoader.LoadMainMenu();
                Destroy(gameObject);
            }
            
        }
        else if(isMinion)
        {
            gameManager.bossShop.minionCount--;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string OnSave()
    {
       
            return JsonUtility.ToJson(new Savedata() { healthVal = this.healthVal, maxHealth = this.maxHealth });
        
       
    }

    public void OnLoad(string data)
    {
            Savedata savedata = JsonUtility.FromJson<Savedata>(data);
            healthVal = savedata.healthVal;
            maxHealth = savedata.maxHealth;
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
