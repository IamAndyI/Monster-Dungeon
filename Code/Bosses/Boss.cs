using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lowscope.Saving;

public class Boss : MonoBehaviour, ISaveable
{
    private static Boss instance;

    [System.Serializable]
    public struct SaveData
    {
         public float health;
         public float attackDamage;
         public float powerValue;
         public float leadershipValue;
    }


    [SerializeField] public float health = 0f;
    [SerializeField] public float attackDamage = 0f;
    [SerializeField] public float powerValue = 0f;
    [SerializeField] public float leadershipValue = 0f;
    public bool isDead = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            print("Boss Destroyed");
            Destroy(gameObject);
        }
    }


    private void Start()
    {

       
    }

    public void OnLoad(string data)
    {
      
            SaveData saveData = JsonUtility.FromJson<SaveData>(data);
            health = saveData.health;
            attackDamage = saveData.attackDamage;
            leadershipValue = saveData.leadershipValue;
            powerValue = saveData.powerValue;
       
    }

    public string OnSave()
    {
        return JsonUtility.ToJson(new SaveData() { health = this.health, attackDamage = this.attackDamage, 
            leadershipValue = this.leadershipValue, powerValue = this.powerValue});
    }

    public bool OnSaveCondition()
    {
        return true;
    }
}
