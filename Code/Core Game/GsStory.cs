using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GsStory : MonoBehaviour
{
    [Header("Scripts")]
    PlayerManager playerManager;
    WaveManager waveManager;
    SceneLoader sceneLoader;
    [SerializeField] SaveLoader saveLoader;

    [Header("UI")]
    [SerializeField] GameObject GsUnlockAnim;
    [SerializeField] GameObject GsUnlockText;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        waveManager = FindObjectOfType<WaveManager>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        GsUnlockAnim.SetActive(false);
        GsUnlockText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveManager.waveNum == 10 && !waveManager.waveInProgress)
        {
            StartCoroutine(EndStory());
        }
    }

    IEnumerator EndStory()
    {
        GsUnlockAnim.SetActive(true);
        GsUnlockText.SetActive(true);
        playerManager.endlessModeUnlocked = true;
        playerManager.gsUnlocked = true;
        playerManager.gsInEndlessMode = false;
        saveLoader.gsAlreadyLoaded = false;
        yield return new WaitForSeconds(2);
        sceneLoader.LoadMainMenu();
    }
}
