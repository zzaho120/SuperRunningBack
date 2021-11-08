using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdManager
{
    public static readonly string interstitial1Id = "ca-app-pub-1195551850458243/6457843040";
    public static readonly string reward1Id = "ca-app-pub-1195551850458243/2518598035";

    private static InterstitialAd interstitial;
    private static RewardedAd rewardedAd;
    private static bool isInit;

    public static void AdInit()
    {
        if(!isInit)
        {
            Debug.Log("init");
            isInit = true;
            //List<string> deviceIds = new List<string>();
            //deviceIds.Add("A27CE6AACBA15404D499EF2B6E14C7DA"); // ³ì½º
            //deviceIds.Add("ECE95E14E279E322062C1351518298E4"); // Æù
            //RequestConfiguration requestConfiguration = new RequestConfiguration
            //    .Builder()
            //    .SetTestDeviceIds(deviceIds)
            //    .build();
            //MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(initStatus =>
            { OnClickRequestInterstitial();
                OnClickRequestReward();
            });
        }
    }
    public static void OnClickRequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial = new InterstitialAd(interstitial1Id);
        interstitial.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public static void HandleOnAdClosed(object sender, EventArgs args)
    {
        OnClickRequestInterstitial();

        GameManager.Instance.isAdEnd = true;
    }

    public static void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            GameManager.Instance.isAdEnd = true;
        }
    }
    public static void OnClickRequestReward()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        rewardedAd = new RewardedAd(reward1Id);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    public static void OnClickReward()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            GameManager.Instance.isAdEnd = true;
        }
    }

    public static void HandleUserEarnedReward(object sender, Reward args)
    {
        GameManager.Instance.isAdEnd = true;
    }

    public static void HandleRewardedOnAdClosed(object sender, EventArgs args)
    {
        OnClickRequestReward();
    }
}
