using System.Collections;
using Lowscope.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BossShop : MonoBehaviour, ISaveable
{
    [Header("Upgrade Costs")]
    public float gold;
    public float gkGold;
    public float gsGold;
    public float kgGold;
    public float dbGold;
    private float healthCost = 100;
    private float attackCost = 100;
    private float powerCost = 200;
    private float leadershipCost = 100;
    private float trapCost;
    private float minionCost = 250;
    public float minionCount = 0;

    public Boss boss;
    public Target bossTarget;

   [Header ("Stats")]
    public TMP_Text healthTotal;
    public TMP_Text attackTotal;
    public TMP_Text powerTotal;
    public TMP_Text leadershipTotal;

    public bool playerIsGoblinKing = false;
    float goblinGoldBonus = 10;

    MinionSpawner myMinionSpawner;
    PlayerManager playerManager;

    [Header("Other")]
    [SerializeField] GameObject minionButton;
    [SerializeField] TMP_Text minionCapacityText;
    [SerializeField] SaveLoader saveLoader;
    AudioSource audioSource;
    Animator myAnimator;
    [SerializeField]AudioClip coinPickup;
    [SerializeField] AudioClip upgradeSound;
    [SerializeField] GameObject goldImage;
    

    [System.Serializable]
    public struct Savedata
    {
        public float gold;
        public float gkGold;
        public float gsGold;
        public float kgGold;
        public float dbGold;
        public float minionCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        myMinionSpawner = FindObjectOfType<MinionSpawner>();
        playerManager = FindObjectOfType<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        minionCapacityText.text = "";

        if(!saveLoader.gkAlreadyLoaded && playerManager.goblinKing)
        {
            gold = 0;
            gkGold = 0;
        }
        else if (!saveLoader.gsAlreadyLoaded && playerManager.giantSkeleton)
        {
            gold = 0;
            gsGold = 0;
        }
        else if(!saveLoader.kgAlreadyLoaded && playerManager.kingGhost)
        {
            gold = 0;
            kgGold = 0;
        }
        else if(!saveLoader.dbAlreadyLoaded && playerManager.dragonBoss)
        {
            gold = 0;
            dbGold = 0;
        }

        if (playerManager.goblinKing)
        {
            gold = gkGold;
        }
        else if (playerManager.giantSkeleton)
        {
            gold = gsGold;
        }
        else if (playerManager.kingGhost)
        {
            gold = kgGold;
        }
        else if(playerManager.dragonBoss)
        {
            gold = dbGold;
        }

    }

    private void Update()
    {
        if(minionCount == 10)
        {
            minionButton.GetComponent<Button>().interactable = false;
            minionCapacityText.text = "*Minions at max capacity";
        }
        else
        {
            minionButton.GetComponent<Button>().interactable = true;
            minionCapacityText.text = "";
        }
    }


    public void UpdateStatUI()
    {
        bossTarget.targetSlider.value = bossTarget.healthVal;
        healthTotal.text = "Health " + bossTarget.healthVal.ToString();
        attackTotal.text = "Attack " + boss.attackDamage.ToString();
        powerTotal.text = "Power " + boss.powerValue.ToString();
        leadershipTotal.text = "Leadership " + boss.leadershipValue.ToString();

    }

    public void AddGold(float value)
    {
        goldImage.GetComponent<Animator>().Play("Gold Anim");
        audioSource.clip = coinPickup;
        audioSource.Play();

        if (playerIsGoblinKing)
        {
            gold += (value + goblinGoldBonus);
        }
        else
        {
            gold += value;
        }

        if(playerManager.goblinKing)
        {
            gkGold = gold;
        }
        else if(playerManager.giantSkeleton)
        {
            gsGold = gold;
        }
        else if (playerManager.kingGhost)
        {
            kgGold = gold;
        }
    }

    public float GetGold()
    {
        return gold;
    }

   public void IncreaseHealthButton(float increaseAmount)
    {
        if (gold >= healthCost)
        {
            audioSource.clip = upgradeSound;
            audioSource.Play();
            bossTarget.healthVal += increaseAmount;
            bossTarget.targetSlider.maxValue = bossTarget.healthVal;
            bossTarget.maxHealth = bossTarget.healthVal;
            gold -= healthCost;
            SubtractBossGold(healthCost);
            myAnimator.SetBool("Health", true);
            UpdateStatUI();
           
        }
    }
   public void IncreaseAttackButton(float increaseAmount)
    {
        if (gold >= attackCost)
        {
            audioSource.clip = upgradeSound;
            audioSource.Play();
            boss.attackDamage += increaseAmount;
            gold -= attackCost;
            SubtractBossGold(attackCost);
            myAnimator.SetBool("Attack", true);
            UpdateStatUI();
            
        }
    }
public void IncreasePowerButton(float increaseAmount)
    {
        if(gold >= powerCost)
        {
            audioSource.clip = upgradeSound;
            audioSource.Play();
            boss.powerValue += increaseAmount;
            gold -= powerCost;
            SubtractBossGold(powerCost);
            myAnimator.SetBool("Power", true);
            UpdateStatUI();
         
        }
    }
    public void IncreaseLeadershipButton(float increaseAmount)
    {
        if (gold >= leadershipCost)
        {
            audioSource.clip = upgradeSound;
            audioSource.Play();
            boss.leadershipValue += increaseAmount;
            gold -= leadershipCost;
            SubtractBossGold(leadershipCost);
            myAnimator.SetBool("Leadership", true);
            UpdateStatUI();
       
        }
    }

    public void StopAnim(int num)
    {
        if(num == 1)
        {
            myAnimator.SetBool("Health", false);
        }
        else if(num == 2)
        {
            myAnimator.SetBool("Attack", false);
        }
        else if(num == 3)
        {
            myAnimator.SetBool("Power", false);
        }
        else if(num == 4)
        {
            myAnimator.SetBool("Leadership", false);
        }
    }

    public void AddTrapButton(float increaseAmount)
    {

    }
    public void BuyMinionButton()
    {
        if(gold >= minionCost && minionCount < 10)
        {
            audioSource.clip = upgradeSound;
            audioSource.Play();
            myMinionSpawner.numMinionsToSpawn += 1;
            gold -= minionCost;
            SubtractBossGold(minionCost);
            minionCount++;
        }
    }

    public void SubtractBossGold(float cost)
    {
        if (playerManager.goblinKing)
        {
            gkGold -= cost;
        }
        else if (playerManager.giantSkeleton)
        {
            gsGold -= cost;
        }
    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata() { gold = this.gold, gsGold = this.gsGold, gkGold = this.gkGold, kgGold = this.kgGold, minionCount = this.minionCount, dbGold = this.dbGold });
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);
        gold = savedata.gold;
        gkGold = savedata.gkGold;
        gsGold = savedata.gsGold;
        kgGold = savedata.kgGold;
        minionCount = savedata.minionCount;
        dbGold = savedata.dbGold;
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
