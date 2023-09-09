using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    public static event Action OnOptionalLimitReached;

    private int maxOptionalAdsCount = 2;
    private int optionalAdsCounter;

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    [SerializeField] private string adUnitIdInterstitial = "ca-app-pub-1229677241121323/8094694660";
    [SerializeField] private string adUnitIdRewarded = "ca-app-pub-1229677241121323/3057727076";

    private bool _premium = false;
    private Action onAfterInterstitialAdd;
    private void OnEnable()
    {
        IAPManager.OnPremiumPurchase += IAPManager_OnPremiumPurchase;
    }
    private void OnDisable()
    {
        IAPManager.OnPremiumPurchase -= IAPManager_OnPremiumPurchase;
    }

    private void IAPManager_OnPremiumPurchase()
    {
        _premium = PlayerPrefsHelper.GetPremiumState();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        _premium = PlayerPrefsHelper.GetPremiumState();

        MobileAds.Initialize((initStatus) =>
        {
            LoadInterstitialAdd();
            LoadRewardedAdd();
        });
    }
    public void LoadInterstitialAdd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        if (_premium == true)
        {
            return;
        }
        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(adUnitIdInterstitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);

                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }
    public void LoadRewardedAdd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(adUnitIdRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                Debug.Log("rewarded ad loaded");
                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });


    }
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            onAfterInterstitialAdd?.Invoke();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            onAfterInterstitialAdd?.Invoke();
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAdd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAdd();
        };
    }
    public void ShowInterstitialAd(Action callback)
    {
        if(_premium)
        {
            callback.Invoke();
            return;
        }
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
            onAfterInterstitialAdd = callback;
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            callback.Invoke();
        }
    }

   
    public void ShowRewardedAd(Action onSuccess, Action onFail)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                optionalAdsCounter++;
                Debug.Log("Showing rewarded add.");

                if (optionalAdsCounter >= maxOptionalAdsCount)
                {
                    Debug.Log("OptionalAds Limit reached.");
                    OnOptionalLimitReached?.Invoke();
                }
                onSuccess.Invoke();
            });
        }
        else
        {
            onFail.Invoke();
        }
    }
}
