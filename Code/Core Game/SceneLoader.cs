using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    int currentSceneIndex;
  public  PlayerManager playerManager;
  public  AdManager adManager;
   
   // [SerializeField]GameObject escapeMenu;


    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerManager = FindObjectOfType<PlayerManager>();
        adManager = playerManager.adManager;
       // escapeMenu.SetActive(false);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           // escapeMenu.SetActive(true);
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentSceneIndex);

    }

    public void LoadNextScene()
    {
      //  adManager.DestroyBannerAd();
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        //adManager.DestroyBannerAd();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGKStory()
    {

       // adManager.DestroyBannerAd();
        playerManager.isEndlessMode = false;
        SceneManager.LoadScene("Goblin King Story");
        
    }

    public void LoadGSStory()
    {
        //  adManager.DestroyBannerAd();
        playerManager.isEndlessMode = false;
        SceneManager.LoadScene("GiantSkeletonStory");
        
    }

    public void LoadKGStory()
    {
        //  adManager.DestroyBannerAd();
        playerManager.isEndlessMode = false;
        SceneManager.LoadScene("KingGhostStory");
       
    }

    public void LoadDBStory()
    {
        // adManager.DestroyBannerAd();
        playerManager.isEndlessMode = false;
        SceneManager.LoadScene("TheDragonStory");
     
    }


    public void LoadEndlessMode()
    {
       //  adManager.DestroyBannerAd();
        playerManager.isEndlessMode = true;
        SceneManager.LoadScene("Endless");
       

        if (playerManager.gkInEndlessMode)
        {
            playerManager.setGoblinKing(true);
        }
        else if(playerManager.gsInEndlessMode)
        {
            playerManager.setGiantSkeleton(true);
        }
        else if(playerManager.kgInEndlessMode)
        {
            playerManager.setKingGhost(true);
        }
        else
        {
            if (playerManager.goblinKing && playerManager.gkUnlocked)
            {
                playerManager.gkInEndlessMode = true;
            }
            else if (playerManager.giantSkeleton && playerManager.gsUnlocked)
            {
                playerManager.gsInEndlessMode = true;
            }
            else if (playerManager.kingGhost && playerManager.kgUnlocked)
            {
                playerManager.kgInEndlessMode = true;
            }
        }

        
    }

    public void QuitGame()
    {

      //  adManager.DestroyBannerAd();
        Application.Quit();
    }

    public void LoadMainMenu()
    {
       // adManager.DestroyBannerAd();
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
       
        SceneManager.LoadScene(0);
    }
}
