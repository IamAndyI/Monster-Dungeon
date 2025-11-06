using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveable
{
    private static PlayerManager instance;

    public bool goblinKing = false;
    public bool giantSkeleton = false;
    public bool kingGhost = false;
    public bool dragonBoss = false;
    public bool isEndlessMode = false;
    public bool gkUnlocked = false;
    public bool gsUnlocked = false;
    public bool kgUnlocked = false;
    public bool dbUnlocked = false;
    public bool endlessModeUnlocked = false;
    //public bool endlessModeInProgress = false;
    public bool gkInEndlessMode = false;
    public bool gsInEndlessMode = false;
    public bool kgInEndlessMode = false;
    public bool dbInEndlessMode = false;
    public int gemTotal = 0;
    public AdManager adManager;

    [System.Serializable]
    public struct Savedata
    {
        public bool gkUnlocked;
        public bool gsUnlocked;
        public bool kgUnlocked;
        public bool dbUnlocked;
        public bool endlessModeUnlocked;
        public bool gkInEndlessMode;
        public bool gsInEndlessMode;
        public bool kgInEndlessMode;
        public bool dbInEndlessMode;
        public int gemTotal;
    }

    public bool getGoblinKing() { return goblinKing; }
    public void setGoblinKing(bool selectedPlayer) { goblinKing = selectedPlayer; giantSkeleton = !selectedPlayer; kingGhost = !selectedPlayer; dragonBoss = !selectedPlayer; }

    public bool getGiantSkeleton() { return giantSkeleton; }
    public void setGiantSkeleton(bool selectedPlayer) { giantSkeleton = selectedPlayer; goblinKing = !selectedPlayer; kingGhost = !selectedPlayer; dragonBoss = !selectedPlayer; }

    public bool getKingGhost() { return kingGhost; }
    public void setKingGhost(bool selectedPlayer) { kingGhost = selectedPlayer; goblinKing = !selectedPlayer; giantSkeleton = !selectedPlayer; dragonBoss = !selectedPlayer; }

    public bool getDragonBoss() { return dragonBoss; }
    public void setDragonBoss(bool selectedPlayer) { dragonBoss = selectedPlayer; kingGhost = !selectedPlayer; goblinKing = !selectedPlayer; giantSkeleton = !selectedPlayer; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        setGoblinKing(true);
        adManager = FindObjectOfType<AdManager>();
    }

    public void EndlessModeSelected()
    {
        isEndlessMode = true;
    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata()
        {
            gkUnlocked = this.gkUnlocked,
            gsUnlocked = this.gsUnlocked,
            kgUnlocked = this.kgUnlocked,
            dbUnlocked = this.dbUnlocked,
            gemTotal = this.gemTotal,
            endlessModeUnlocked = this.endlessModeUnlocked,
            gkInEndlessMode = this.gkInEndlessMode,
            gsInEndlessMode = this.gsInEndlessMode,
            kgInEndlessMode = this.kgInEndlessMode,
            dbInEndlessMode = this.dbInEndlessMode

        }); 
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);
        gkUnlocked = savedata.gkUnlocked;
        gsUnlocked = savedata.gsUnlocked;
        kgUnlocked = savedata.kgUnlocked;
        dbUnlocked = savedata.dbUnlocked;
        gemTotal = savedata.gemTotal;
        endlessModeUnlocked = savedata.endlessModeUnlocked;
        gkInEndlessMode = savedata.gkInEndlessMode;
        gsInEndlessMode = savedata.gsInEndlessMode;
        kgInEndlessMode = savedata.kgInEndlessMode;
        dbInEndlessMode = savedata.dbInEndlessMode;
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
