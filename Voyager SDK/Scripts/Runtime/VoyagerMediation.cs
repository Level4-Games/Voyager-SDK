using System;

namespace Voyager_SDK
{
    internal abstract class VoyagerMediation
    {
        internal const string DefaultInterstitialPlacementName = "default_interstitial";
        internal const string DefaultRewardedPlacementName = "default_rewarded";

        internal static void Initialize()
        {
            ApplovinHelper.Initialize();
        }

        internal static void ShowInterstitial(string placementName)
        {
            ApplovinHelper.ShowInterstitial(placementName);
        }

        internal static void ShowRewarded(string placementName, Action<bool> rewardedCallback)
        {
            ApplovinHelper.ShowRewarded(placementName, rewardedCallback);
        }
        
        internal static void ShowBanner()
        {
            ApplovinHelper.ShowBanner();
        }
        
        internal static void HideBanner()
        {
            ApplovinHelper.HideBanner();
        }
    }
}
