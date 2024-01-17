using System;
using com.adjust.sdk;
using UnityEngine;

namespace Voyager_SDK
{
    public abstract class ApplovinHelper : HelperBase
    {
        private static float _lastInterstitialRequestTime;
        private static float _nextInterstitialTime;

        private static VoyagerSettings _settings;
        private static VoyagerSettings Settings => _settings ??= VoyagerSettings.Instance;

        private static int _interstitialRetryAttempt;
        private static int _rewardedRetryAttempt;

        public static void Initialize()
        {
            _nextInterstitialTime = Time.time + Settings.InitialInterstitialDuration;

            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;

            InitializeMaxSDK();
        }
        
        private static void OnAdRevenuePaidEvent(string adUnitID, MaxSdkBase.AdInfo adInfo)
        {
            AdjustAdRevenue adjustAdRevenue = new(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adjustAdRevenue.setRevenue(adInfo.Revenue, "USD");
            adjustAdRevenue.setAdRevenueNetwork(adInfo.NetworkName);
            adjustAdRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
            adjustAdRevenue.setAdRevenuePlacement(adInfo.Placement);

            Adjust.trackAdRevenue(adjustAdRevenue);
        }

        private static void InitializeMaxSDK()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, configure and start loading ads.
                Debug.Log("MAX SDK Initialized");

                InitializeInterstitialAds();
                InitializeRewardedAds();
                InitializeBannerAds();

                // Initialize Adjust SDK
                var adjustConfig = new AdjustConfig(Settings.AppToken, AdjustEnvironment.Production);
                Adjust.start(adjustConfig);
            };

            MaxSdk.SetSdkKey(Settings.MaxSDKKey);
            MaxSdk.InitializeSdk();
        }

        #region Interstitial Ad Methods

        private static void InitializeInterstitialAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private static void LoadInterstitial()
        {
            Debug.Log("Inter " + "Loading...");
            MaxSdk.LoadInterstitial(Settings.InterstitialId);
        }

        public static void ShowInterstitial(string placement)
        {
            if (DataManager.NoAds)
                return;

            if (_nextInterstitialTime < Time.time && MaxSdk.IsInterstitialReady(Settings.InterstitialId))
            {
                RefreshInterTimer();
                Debug.Log("Inter " + "Showing");
                MaxSdk.ShowInterstitial(Settings.InterstitialId, placement);
            }
            else
            {
                Debug.Log("Inter " + "Ad not ready");
            }
        }

        private static void RefreshInterTimer()
        {
            _nextInterstitialTime = Time.time + Settings.InitialInterstitialDuration;
        }

        private static void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            Debug.Log("Inter " + "Loaded");

            // Reset retry attempt
            _interstitialRetryAttempt = 0;
        }

        private static void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            _interstitialRetryAttempt++;
            var retryDelay = Math.Pow(2, Math.Min(6, _interstitialRetryAttempt));

            Debug.Log("Inter " + "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...");

            AppManager.Instance.Invoke(nameof(LoadInterstitial), (float)retryDelay);
        }

        private static void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
            LoadInterstitial();
        }

        private static void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            Debug.Log("Interstitial dismissed");
            LoadInterstitial();
        }

        #endregion

        #region Rewarded Ad Methods

        private static void InitializeRewardedAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }

        private static void LoadRewardedAd()
        {
            Debug.Log("Rewarded " + "Loading...");
            MaxSdk.LoadRewardedAd(Settings.RewardedId);
        }

        private static Action<bool> _rewardedCallback;

        public static void ShowRewarded(string placement, Action<bool> rewardedCallback = null)
        {
            _rewardedCallback = rewardedCallback;

            if (MaxSdk.IsRewardedAdReady(Settings.RewardedId))
            {
                Debug.Log("Rewarded " + "Showing : " + placement);
                MaxSdk.ShowRewardedAd(Settings.RewardedId, placement);
            }
            else
            {
                _rewardedCallback?.Invoke(false);
                _rewardedCallback = null;

                Debug.Log("Rewarded " + "Ad not ready");
            }
        }

        private static void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded " + "Loaded");

            // Reset retry attempt
            _rewardedRetryAttempt = 0;
        }

        private static void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
            _rewardedRetryAttempt++;
            var retryDelay = Math.Pow(2, Math.Min(6, _rewardedRetryAttempt));

            Debug.Log("Rewarded " + "Load failed: " + errorInfo.Code + "\nRetrying in " + retryDelay + "s...");

            AppManager.Instance.Invoke(nameof(LoadRewardedAd), (float)retryDelay);
        }

        private static void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
            LoadRewardedAd();
        }

        private static void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad displayed");
        }

        private static void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad clicked");
        }

        private static void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _rewardedCallback?.Invoke(false);
            _rewardedCallback = null;

            // Rewarded ad is hidden. Pre-load the next ad
            Debug.Log("Rewarded ad dismissed");
            LoadRewardedAd();
        }

        private static void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward,
            MaxSdkBase.AdInfo adInfo)
        {
            _rewardedCallback?.Invoke(true);
            _rewardedCallback = null;

            RefreshInterTimer();

            // Rewarded ad was displayed and user should receive the reward
            Debug.Log("Rewarded ad received reward");
        }

        #endregion

        #region Banner Ad Methods

        private static void InitializeBannerAds()
        {
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            MaxSdk.CreateBanner(Settings.BannerId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(Settings.BannerId, new Color(255, 255, 255, 0));
            //
            // if (DataManager.NoAds)
            //     MaxSdk.HideBanner(Settings.BannerId);
            // else
            //     MaxSdk.ShowBanner(Settings.BannerId);
        }

        public static void ShowBanner()
        {
            MaxSdk.ShowBanner(Settings.BannerId);
        }

        public static void HideBanner()
        {
            MaxSdk.HideBanner(Settings.BannerId);
        }

        #endregion
    }
}