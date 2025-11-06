using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lowscope.Saving;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField] GameObject gkMinionPrefab;
    [SerializeField] GameObject gsMinionPrefab;
    private float timeBetweenSpawns = 1;
    private float timeToSpawn;
    public int numMinionsToSpawn = 0;

    PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToSpawn += Time.deltaTime;
        if (numMinionsToSpawn > 0 && timeToSpawn > timeBetweenSpawns)
        {
            if (playerManager.goblinKing)
            {
                GameObject minion = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Goblin Minion");
                minion.transform.position = transform.position;
                timeToSpawn = 0;
                numMinionsToSpawn--;
            }
            else if(playerManager.giantSkeleton)
            {
                GameObject minion = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Skeleton Minion");
                minion.transform.position = transform.position;
                timeToSpawn = 0;
                numMinionsToSpawn--;
            }
            else if(playerManager.kingGhost)
            {
                GameObject minion = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Ghost Minion");
                minion.transform.position = transform.position;
                timeToSpawn = 0;
                numMinionsToSpawn--;
            }
            else if(playerManager.dragonBoss)
            {
                GameObject minion = SaveMaster.SpawnSavedPrefab(InstanceSource.Resources, "Dragon Minion");
                minion.transform.position = transform.position;
                timeToSpawn = 0;
                numMinionsToSpawn--;
            }
        }

    }
}
