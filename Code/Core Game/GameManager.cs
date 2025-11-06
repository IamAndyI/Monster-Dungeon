using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lowscope.Saving;


public class GameManager : MonoBehaviour
{
   
    
    [Header("Variables")]
    int[] bosses;
    
    int upgradeCost = 1;
    private Transform spawnpoint;
    [SerializeField]public List<GameObject> players = new List<GameObject>();
    [SerializeField] GameObject goblinKingBoss;
    [SerializeField] GameObject giantSkeletonBoss;
    [SerializeField] GameObject kingGhostBoss;
    [SerializeField] GameObject dragonBoss;
    bool inGame = false;
    public bool wasInGame = false;
    public bool gamePaused = false;
   
   

    [Header("UI")]
    [SerializeField] GameObject toggleShopButton;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject nextWaveButton;
    [SerializeField] GameObject gkStoryButton;
    [SerializeField] GameObject gsStoryButton;
    [SerializeField] GameObject kgStoryButton;
    [SerializeField] GameObject dbStoryButton;

    [SerializeField] public TMP_Text goldText;
    [SerializeField] public TMP_Text gemText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Image bossImage;
    [SerializeField] Sprite gKImage;
    [SerializeField] Sprite gSImage;
    [SerializeField] Sprite kgImage;
    [SerializeField] Sprite dbImage;

    GameObject bossSelectionCanvas;

    [Header("Scripts")]
    public WaveManager waveManager;
    public PlayerManager playerManager;
    public BossShop bossShop;
    public MainMenuManager mainMenu;
    [SerializeField] SaveLoader saveLoader;


    // Start is called before the first frame update
    void Start()
    {
        
        if (FindObjectOfType<MainMenuManager>())
        {
            mainMenu = FindObjectOfType<MainMenuManager>();
            playerManager = FindObjectOfType<PlayerManager>();

            if(playerManager.gkUnlocked)
            {
                gkStoryButton.GetComponent<Button>().interactable = false;
            }
            
            if(playerManager.gsUnlocked)
            {
                gsStoryButton.GetComponent<Button>().interactable = false;
            }

            if(playerManager.kgUnlocked)
            {
                kgStoryButton.GetComponent<Button>().interactable = false;
            }
            if(playerManager.dbUnlocked)
            {
                dbStoryButton.GetComponent<Button>().interactable = false;
            }

        }

        if (FindObjectOfType<WaveManager>() != null)
        {
            SetupGame();
        }
        //If wavemanager is in scene then the scene is a game scene 
        //Set these objects to non active
        gemText.text = playerManager.gemTotal.ToString();

    }

    private void SetupGame()
    {
        spawnpoint = FindObjectOfType<Spawnpoint>().gameObject.transform;
        waveManager = FindObjectOfType<WaveManager>();
        bossShop = FindObjectOfType<BossShop>();
        playerManager = FindObjectOfType<PlayerManager>();
        toggleShopButton.SetActive(false);
        shopPanel.SetActive(false);
        pauseMenu.SetActive(false);
        nextWaveButton.SetActive(false);
        goldText.text = bossShop.GetGold().ToString();
        inGame = true;

      

        if (inGame)
        {
            
            if (!waveManager.waveInProgress)
            {
                toggleShopButton.SetActive(true);
                nextWaveButton.SetActive(true);
            }
            else
            {
                toggleShopButton.SetActive(false);
                nextWaveButton.SetActive(false);
            }

            if (!saveLoader.gkAlreadyLoaded )
            {
                if (playerManager.getGoblinKing() == true)
                {
                    GameObject player = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Goblin King");
                    if (waveManager.waveNum == 1)
                    {
                        player.transform.position = spawnpoint.position;
                    }
                    bossShop.boss = player.GetComponent<Boss>();
                    bossShop.bossTarget = bossShop.boss.gameObject.GetComponent<Target>();
                    bossShop.UpdateStatUI();
                    waveManager.waveNum = 1;
                    waveManager.waveInProgress = true;
                    bossShop.playerIsGoblinKing = true;
                    saveLoader.gkAlreadyLoaded = true;
                }
            }

            if(!saveLoader.gsAlreadyLoaded)
            {
                if (playerManager.getGiantSkeleton() == true)
                {
                    GameObject player = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Giant Skeleton");
                    if (waveManager.waveNum == 1)
                    {
                        player.transform.position = spawnpoint.position;
                    }
                    bossShop.boss = player.GetComponent<Boss>();
                    bossShop.bossTarget = bossShop.boss.gameObject.GetComponent<Target>();
                    waveManager.waveNum = 1;
                    waveManager.waveInProgress = true;
                    bossShop.UpdateStatUI();
                    saveLoader.gsAlreadyLoaded = true;
                }

            }

            if (!saveLoader.kgAlreadyLoaded)
            {
                if (playerManager.getKingGhost() == true)
                {
                    GameObject player = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "King Ghost");
                    if (waveManager.waveNum == 1)
                    {
                        player.transform.position = spawnpoint.position;
                    }
                    bossShop.boss = player.GetComponent<Boss>();
                    bossShop.bossTarget = bossShop.boss.gameObject.GetComponent<Target>();
                    waveManager.waveNum = 1;
                    waveManager.waveInProgress = true;
                    bossShop.UpdateStatUI();
                    saveLoader.kgAlreadyLoaded = true;
                }

            }

            if (!saveLoader.dbAlreadyLoaded)
            {
                if (playerManager.getDragonBoss() == true)
                {
                    GameObject player = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Dragon");
                    if (waveManager.waveNum == 1)
                    {
                        player.transform.position = spawnpoint.position;
                    }
                    bossShop.boss = player.GetComponent<Boss>();
                    bossShop.bossTarget = bossShop.boss.gameObject.GetComponent<Target>();
                    waveManager.waveNum = 1;
                    waveManager.waveInProgress = true;
                    bossShop.UpdateStatUI();
                    saveLoader.dbAlreadyLoaded = true;
                }

            }

            wasInGame = true;
        }
        else
        {
            playerManager.isEndlessMode = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame) // Set ingame back to false later when player completes a story or finishes in endless mode
        {
            goldText.text = bossShop.GetGold().ToString();
            if (!waveManager.waveInProgress)
            {
                if(waveManager.waveNum == 10 && !playerManager.isEndlessMode)
                {
                    toggleShopButton.SetActive(false);
                    nextWaveButton.SetActive(false);
                }
                else
                {
                    toggleShopButton.SetActive(true);
                    nextWaveButton.SetActive(true);
                }
              
            }
            else
            {
                toggleShopButton.SetActive(false);
                nextWaveButton.SetActive(false);
            }
        }
    }

    public void PauseGame()
    {
        if(pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartEndlessMode()
    {
        if (playerManager.gkInEndlessMode)
        {
            playerManager.gkInEndlessMode = false;
        }
        else if(playerManager.gsInEndlessMode)
        {
            playerManager.gsInEndlessMode = false;
        }
        else if(playerManager.kgInEndlessMode)
        {
            playerManager.kgInEndlessMode = false;
        }
        else if(playerManager.dbInEndlessMode)
            {
            playerManager.dbInEndlessMode=false;
        }

        GameObject player = FindObjectOfType<Boss>().gameObject;

        //Minions check if dead and if true get deleted
        player.GetComponent<Target>().isDead = true;

        //Deletes player before restart
        player.GetComponent<Target>().Die();

    }

    public void ToggleBossShop()
    {
        if (!waveManager.waveInProgress)
        {
            if (shopPanel.activeSelf == true)
            {
                shopPanel.SetActive(false);
            }
            else
            {
                shopPanel.SetActive(true);
            }
        }
    }
   
    
    public void AddGems(int num)
    {
        playerManager.gemTotal += num;
        gemText.text = playerManager.gemTotal.ToString();
    }
   

    public int GetBossStats(int bossNum)
    {
        return 0; //Change to stats
    }

    //Each increase stat button will have a value for each boss 
    //And will increase the overall base power of this stat outside of the game
    //Using gems
    public void IncreaseAttackButton(int bossNum)
    {
        float attackIncrease = 5;

        if (playerManager.gemTotal >= upgradeCost)
        {
            if (bossNum == 0)
            {
                goblinKingBoss.GetComponent<GoblinKing>().attackDamage += attackIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 1)
            {
                giantSkeletonBoss.GetComponent<GiantSkeleton>().attackDamage += attackIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 2)
            {
                kingGhostBoss.GetComponent<KingGhost>().attackDamage += attackIncrease;
                mainMenu.UpdateUI();
            }
            else if(bossNum == 3)
            {
                dragonBoss.GetComponent<DragonBoss>().attackDamage += attackIncrease;
                mainMenu.UpdateUI();
            }

            playerManager.gemTotal--;
            gemText.text = playerManager.gemTotal.ToString();
        }
    }

    public void IncreaseHealthButton(int bossNum)
    {
        float healthIncrease = 5;
        if (playerManager.gemTotal >= upgradeCost)
        {

            if (bossNum == 0)
            {
                goblinKingBoss.GetComponent<GoblinKing>().health += healthIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 1)
            {
                giantSkeletonBoss.GetComponent<GiantSkeleton>().health += healthIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 2)
            {
                kingGhostBoss.GetComponent<KingGhost>().health += healthIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 3)
            {
                dragonBoss.GetComponent<DragonBoss>().health += healthIncrease;
                mainMenu.UpdateUI();
            }
            playerManager.gemTotal--;
            gemText.text = playerManager.gemTotal.ToString();
        }
    }

    public void IncreasePowerButton(int bossNum)
    {
        float powerIncrease = 5;

        if (playerManager.gemTotal >= upgradeCost)
        {

            if (bossNum == 0)
            {
                goblinKingBoss.GetComponent<GoblinKing>().powerValue += powerIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 1)
            {
                giantSkeletonBoss.GetComponent<GiantSkeleton>().powerValue += powerIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 2)
            {
                kingGhostBoss.GetComponent<KingGhost>().powerValue += powerIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 3)
            {
                dragonBoss.GetComponent<DragonBoss>().powerValue += powerIncrease;
                mainMenu.UpdateUI();
            }
            playerManager.gemTotal--;
            gemText.text = playerManager.gemTotal.ToString();
        }
    }

    public void IncreaseLeadershipButton(int bossNum)
    {
        float leadershipIncrease = 5;

        if (playerManager.gemTotal >= upgradeCost)
        {

            if (bossNum == 0)
            {
                goblinKingBoss.GetComponent<GoblinKing>().leadershipValue += leadershipIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 1)
            {
                giantSkeletonBoss.GetComponent<GiantSkeleton>().leadershipValue += leadershipIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 2)
            {
                kingGhostBoss.GetComponent<KingGhost>().leadershipValue += leadershipIncrease;
                mainMenu.UpdateUI();
            }
            else if (bossNum == 3)
            {
                dragonBoss.GetComponent<DragonBoss>().leadershipValue += leadershipIncrease;
                mainMenu.UpdateUI();
            }
            playerManager.gemTotal--;
            gemText.text = playerManager.gemTotal.ToString();
        }
    }

    public void SetGKBoss()
    {
        playerManager.setGoblinKing(true);
        bossImage.sprite = gKImage;
    }

    public void SetGSBoss()
    {
        playerManager.setGiantSkeleton(true);
        bossImage.sprite = gSImage;
    }
    public void SetKGBoss()
    {
        playerManager.setKingGhost(true);
        bossImage.sprite = kgImage;
    }

    public void SetDBoss()
    {
        playerManager.setDragonBoss(true);
        bossImage.sprite = dbImage;                            
    }

}
