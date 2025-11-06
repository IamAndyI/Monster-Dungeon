using System.Collections;
using System.Collections.Generic;
using Lowscope.Saving;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveManager : MonoBehaviour, ISaveable
{
    public bool waveInProgress = true;
    public bool waveWasCompleted = false;
    public bool spawnChampion = false;
    public int numOfEnemiesToSpawn;
    public float waveNum;
    public int numEnemiesInScene = 0;
    public int maxNumEnemiesInScene = 10;
    int startingNumEnemies = 10;
    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] public GameObject[] championPrefabs;
   
    public int enemiesLeft;
    [SerializeField] public TMP_Text waveNumText;

     [SerializeField] float diffucltyCounter = 5;
    public float difficultyMultiplier = 0.2f;

    PlayerManager playerManager;



    [System.Serializable]
    public struct Savedata
    {
        public float waveNum;
        public bool waveWasCompleted;
        public bool waveInProgress;
        public float difficultyMultiplier;
        public int startingNumEnemies;
        public float difficultyCounter;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveWasCompleted = false;
        waveNumText.text = "Wave " + waveNum.ToString();
        playerManager = FindObjectOfType<PlayerManager>();

        if(waveNum == 1)
        {
            difficultyMultiplier = 0.2f;
            startingNumEnemies = 10;
            numOfEnemiesToSpawn = startingNumEnemies;
            enemiesLeft = startingNumEnemies;
            diffucltyCounter = 5;
        }
        else
        {
            numOfEnemiesToSpawn = startingNumEnemies;
            enemiesLeft = startingNumEnemies;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Hack...Delete Later
        if(Input.GetKey(KeyCode.P))
        {
            waveNum = 10;
        }


        if (enemiesLeft <= 0)
        {
            waveInProgress = false;
            waveWasCompleted = true;
            ResetHealth();
        }

        if (diffucltyCounter <= waveNum)
        {
            diffucltyCounter = waveNum + 5;
            difficultyMultiplier += 0.2f;

            if (playerManager.isEndlessMode == true)
            {
                //Next wave spawn a Champion
                spawnChampion = true;
                
            }
        }
    }

    public void NextWave()
    {
        if (!waveInProgress)
        {
            numOfEnemiesToSpawn = NumToSpawn();
            waveInProgress = true;
            waveWasCompleted = false;
            waveNum += 1;
            waveNumText.text = "Wave " + waveNum.ToString();
        }
    }

    public int NumToSpawn()
    {
        startingNumEnemies += 5;
        enemiesLeft = startingNumEnemies;
        return startingNumEnemies;
    }

    public void ResetHealth()
    {
        BossShop bossShop = FindObjectOfType<BossShop>();
        bossShop.bossTarget.healthVal = bossShop.bossTarget.targetSlider.maxValue;
        bossShop.bossTarget.GainHealth(0); 
        bossShop.UpdateStatUI();

    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata()
        {
            waveNum = this.waveNum,
            waveWasCompleted = this.waveInProgress,
            waveInProgress = this.waveInProgress,
            difficultyMultiplier = this.difficultyMultiplier,
            difficultyCounter = this.diffucltyCounter,
            startingNumEnemies = this.startingNumEnemies
        }) ;
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);
        waveNum = savedata.waveNum;
        waveWasCompleted = savedata.waveWasCompleted;
        waveInProgress = savedata.waveInProgress;
        waveNumText.text = "Wave " + waveNum.ToString();
        difficultyMultiplier = savedata.difficultyMultiplier;
        diffucltyCounter = savedata.difficultyCounter;
        startingNumEnemies = savedata.startingNumEnemies;
    }

    public bool OnSaveCondition()
    {
        return true;
    }

    
}
