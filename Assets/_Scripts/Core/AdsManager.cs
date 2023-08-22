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

    [SerializeField] private string adUnitIdInterstitial = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private string adUnitIdRewarded = "ca-app-pub-3940256099942544/5224354917";

    private bool _premium = false;
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

        MobileAds.Initialize((initStatus) =>
        {

        });

        _premium = PlayerPrefsHelper.GetPremiumState();
    }
    public void PopupAdd(Action callback)
    {
        if (_premium == true)
        {
            callback.Invoke();
            return;
        }

        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        Debug.Log("pop up add");
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(adUnitIdInterstitial, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);

                    callback.Invoke();
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;

                ad.OnAdFullScreenContentClosed += () =>
                {
                    callback.Invoke();
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);
                    callback.Invoke();
                };
                ShowInterstitialAd();
            });
    }
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void WathcAd(Action onSuccess, Action onFail)
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
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(adUnitIdRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    onFail.Invoke();
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;

                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.LogError("Rewarded ad failed to open full screen content " +
                       "with error : " + error);
                    onFail.Invoke();
                };
                ShowRewardedAd(onSuccess);
            });

    }
    public void ShowRewardedAd(Action callback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                optionalAdsCounter++;
                Debug.Log("Watching Ads");

                if (optionalAdsCounter >= maxOptionalAdsCount)
                {
                    OnOptionalLimitReached?.Invoke();
                    Debug.Log("OptionalAds Limit reached.");
                }
                callback.Invoke();
            });
        }
    }


}
