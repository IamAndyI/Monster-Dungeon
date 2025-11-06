using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private BannerView currentBanner;
    WaveManager waveManager;
    private bool adDisplayed = false;
    public float adtimer = 0f;
    private float timeBetweenAds = 60f;

    private static AdManager instance;

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

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();
        print("New Ad!");
        adtimer = 0;
        adDisplayed = true;
    }

 public void DestroyBannerAd()
    {
        if (this.bannerAd != null)
        {
            print("Destroy Previous Ad");
            bannerAd.Destroy();
        }
    }

    private void RequestBanner()
    {
        if (this.bannerAd != null)
        {
            print("Destroy Previous Ad");
            bannerAd.Destroy();
        }

        //Real
        // string adUnitId = "ca-app-pub-9001960277734538/1863584076";

        //Test
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        this.bannerAd = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        
        currentBanner = this.bannerAd;

        AdRequest request = new AdRequest.Builder().Build();
        this.bannerAd.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        adtimer += Time.deltaTime;

        if(timeBetweenAds <= adtimer)
        {
            adtimer = 0;
            MobileAds.Initialize(InitializationStatus => { });
            this.RequestBanner();
            print("New Ad!");
            adDisplayed = true;
        }
        
    }
}
