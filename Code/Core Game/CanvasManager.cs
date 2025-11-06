using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CanvasManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] GameObject storyModeCanvas;
    [SerializeField] GameObject endlessModeCanvas;
    [SerializeField] GameObject bossCanvas;
    [SerializeField] GameObject optionsCanvas;

    [Header("UI")]
    [SerializeField] GameObject endlessModeButton;
    [SerializeField] GameObject gkButton;
    [SerializeField] GameObject gsButton;
    [SerializeField] GameObject kgButton;
    [SerializeField] GameObject dbButton;

    [SerializeField] GameObject endlessModeLock;
    [SerializeField] GameObject gkLock;
    [SerializeField] GameObject gsLock;
    [SerializeField] GameObject kgLock;
    public Slider volumeSlider;

    [Header("Scripts")]
    PlayerManager playerManager;

    private bool canUseCanvas = true;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        gkLock.SetActive(true);
        gsLock.SetActive(true);
        gkButton.GetComponent<Button>().interactable = false;
        gsButton.GetComponent<Button>().interactable = false;
        kgButton.GetComponent<Button>().interactable = false;
        dbButton.GetComponent<Button>().interactable = false;


        volumeSlider.value = FindObjectOfType<MusicPlayer>().volume;

        UnlockBosses();
        SetEndlessMode();
        SetCanvases();
    }

    private void Update()
    {
        if(playerManager.endlessModeUnlocked)
        {
            endlessModeButton.GetComponent<Button>().interactable = true;

            if(playerManager.gkInEndlessMode)
            {
                gkButton.GetComponent<Button>().interactable = true;
                gsButton.GetComponent<Button>().interactable = false;
                kgButton.GetComponent<Button>().interactable = false;
                dbButton.GetComponent<Button>().interactable = false;
            }
            else if(playerManager.gsInEndlessMode)
            {
                gkButton.GetComponent<Button>().interactable = false;
                gsButton.GetComponent<Button>().interactable = true;
                kgButton.GetComponent<Button>().interactable = false;
                dbButton.GetComponent<Button>().interactable = false;
            }
            else if(playerManager.kgInEndlessMode)
            {
                gkButton.GetComponent<Button>().interactable = false;
                gsButton.GetComponent<Button>().interactable = false;
                kgButton.GetComponent<Button>().interactable = true;
                dbButton.GetComponent<Button>().interactable = false;
            }
            else if(playerManager.dbInEndlessMode)
            {
                gkButton.GetComponent<Button>().interactable = false;
                gsButton.GetComponent<Button>().interactable = false;
                kgButton.GetComponent<Button>().interactable = false;
                dbButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                UnlockedButtonsAreInteractable();

            }

        }
    }

    public void UnlockedButtonsAreInteractable()
    {

        if (playerManager.gkUnlocked)
        {
            gkButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            gkButton.GetComponent<Button>().interactable = false;
        }

        if (playerManager.gsUnlocked)
        {
            gsButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            gsButton.GetComponent<Button>().interactable = false;
        }

        if (playerManager.kgUnlocked)
        {
            kgButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            kgButton.GetComponent<Button>().interactable = false;
        }
    }

    public void ChangeVolume()
    {
        MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.volume = volumeSlider.value;
        musicPlayer.audioSource.volume = musicPlayer.volume;
    }

    public void toggleCanvas(GameObject myCanvas)
    {
        if(myCanvas.activeSelf == true)
        {
            myCanvas.SetActive(false);
            canUseCanvas = true;
           
        }
        else if(myCanvas.activeSelf == false && canUseCanvas)
        {
            myCanvas.SetActive(true);
            canUseCanvas = false;
            
        }
    }

    private void UnlockBosses()
    {
        if(playerManager.gkUnlocked)
        {
            gkLock.SetActive(false);
            gkButton.GetComponent<Button>().interactable = true;
        }

        if(playerManager.gsUnlocked)
        {
            gsLock.SetActive(false);
            gsButton.GetComponent<Button>().interactable = true;
        }

        if(playerManager.kgUnlocked)
        {
            kgLock.SetActive(false);
            kgButton.GetComponent<Button>().interactable = true;
        }

        if(playerManager.dbUnlocked)
        {
            dbButton.GetComponent<Button>().interactable = true;
        }
    }

    private void SetEndlessMode()
    {
        if (playerManager.endlessModeUnlocked)
        {
            endlessModeButton.GetComponent<Button>().interactable = true;
            endlessModeLock.SetActive(false);
        }
        else
        {
            endlessModeButton.GetComponent<Button>().interactable = false;
            endlessModeLock.SetActive(true);
        }
    }

    private void SetCanvases()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        storyModeCanvas.SetActive(false);
        endlessModeCanvas.SetActive(false);
        bossCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
