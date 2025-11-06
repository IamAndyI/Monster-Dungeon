using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoader : MonoBehaviour, ISaveable
{

    private static SaveLoader instance;

    public bool gkAlreadyLoaded = false;
    public bool gsAlreadyLoaded = false;
    public bool kgAlreadyLoaded = false;
    public bool dbAlreadyLoaded = false;

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

    [System.Serializable]
    public struct Savedata
    {
        public bool gkAlreadyLoaded;
        public bool gsAlreadyLoaded;
        public bool kgAlreadyLoaded;
        public bool dbAlreadyLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new Savedata()
        {
            kgAlreadyLoaded = this.kgAlreadyLoaded,
            gsAlreadyLoaded = this.gsAlreadyLoaded,
            gkAlreadyLoaded = this.gkAlreadyLoaded,
            dbAlreadyLoaded = this.dbAlreadyLoaded

        });
    }

    public void OnLoad(string data)
    {
        Savedata savedata = JsonUtility.FromJson<Savedata>(data);
        kgAlreadyLoaded = savedata.kgAlreadyLoaded;
        gsAlreadyLoaded = savedata.gsAlreadyLoaded;
        gkAlreadyLoaded = savedata.gkAlreadyLoaded;
        dbAlreadyLoaded =savedata.dbAlreadyLoaded;
    }

    public bool OnSaveCondition()
    {
        return true;
    }

}
