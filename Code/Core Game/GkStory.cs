using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GkStory : MonoBehaviour
{
    [Header("Scripts")]
    PlayerManager playerManager;
    WaveManager waveManager;
    SceneLoader sceneLoader;
    [SerializeField] SaveLoader saveLoader;

    [Header("UI")]
    [SerializeField] GameObject GkUnlockAnim;
    [SerializeField] GameObject GkUnlockText;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        waveManager = FindObjectOfType<WaveManager>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        GkUnlockAnim.SetActive(false);
        GkUnlockText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(waveManager.waveNum == 10 && !waveManager.waveInProgress)
        {
            StartCoroutine(EndStory());
        }
    }

    IEnumerator EndStory()
    {
        GkUnlockAnim.SetActive(true);
        GkUnlockText.SetActive(true);
        playerManager.endlessModeUnlocked = true;
        playerManager.gkUnlocked = true;
        playerManager.gkInEndlessMode = false;
        saveLoader.gkAlreadyLoaded = false;
        yield return new WaitForSeconds(2);
        sceneLoader.LoadMainMenu();
    }
}
