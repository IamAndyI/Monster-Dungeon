using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //[SerializeField] GameObject enemyPrefab;
    private float timeBetweenSpawns = 2;
    private float timeToSpawn;

    public WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveManager.waveInProgress)
        {

            timeToSpawn += Time.deltaTime;
            if (waveManager.numOfEnemiesToSpawn > 0 && timeToSpawn > timeBetweenSpawns
                && waveManager.numEnemiesInScene < waveManager.maxNumEnemiesInScene)
            {
                if (waveManager.spawnChampion)
                {
                    GameObject champion = Instantiate(GetRandomChampion(), transform.position, Quaternion.identity);

                    champion.GetComponent<EnemyMovement>().healthVal += DifficultyMultiplier() * champion.GetComponent<EnemyMovement>().healthVal;
                    champion.GetComponent<EnemyMovement>().dmg += DifficultyMultiplier() * champion.GetComponent<EnemyMovement>().dmg;
                   // champion.GetComponent<EnemyMovement>().SetHealthBar();
                    champion.GetComponent<EnemyMovement>().isChampion = true;
                    waveManager.spawnChampion = false;
                }
                else
                {
                    GameObject enemy = Instantiate(GetRandomEnemy(), transform.position, Quaternion.identity);
                    enemy.GetComponent<EnemyMovement>().healthVal += DifficultyMultiplier() * enemy.GetComponent<EnemyMovement>().healthVal;
                    enemy.GetComponent<EnemyMovement>().dmg += DifficultyMultiplier() * enemy.GetComponent<EnemyMovement>().dmg;
                    //enemy.GetComponent<EnemyMovement>().SetHealthBar();
                    waveManager.numEnemiesInScene++;
                    timeToSpawn = 0;
                    waveManager.numOfEnemiesToSpawn--;
                }
            }
        }

    }

    private GameObject GetRandomEnemy()
    {
        //Gets a randon prefab from the enemy array in the wavemanager
        int randNum = Random.Range(0, waveManager.enemyPrefabs.Length);

        return waveManager.enemyPrefabs[randNum];

    }

    private GameObject GetRandomChampion()
    {
        //Gets a randon prefab from the enemy array in the wavemanager
        int randNum = Random.Range(0, waveManager.championPrefabs.Length);

        return waveManager.championPrefabs[randNum];

    }

    private float DifficultyMultiplier()
    {
        return waveManager.difficultyMultiplier;
    }

    
}
