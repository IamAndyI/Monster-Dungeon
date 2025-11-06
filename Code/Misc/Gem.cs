using Lowscope.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [System.Serializable]
    public struct Savedata
    {
        public int saveNum;
    }

    GameManager gameManager;
    [SerializeField] int saveNum = 10;
    [SerializeField] public SaveLoader saveLoader;
    bool isLoaded = false;

    // Start is called before the first frame update
    void Start()
    {


        //Find gamemanager
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //call add gem in gamemanager
            //destroy gameobject
            gameManager.AddGems(1);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
       
    }

   
}
