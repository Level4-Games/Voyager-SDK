using System;

namespace Voyager_SDK
{
    public class VoyagerSDK
    {
        /// <summary>
        /// Display an Interstitial Ad
        /// </summary>
        /// <param name="placementName">The name of the Ad placement</param>
        public static void ShowInterstitial(string placementName = VoyagerMediation.DefaultInterstitialPlacementName) =>
            VoyagerMediation.ShowInterstitial(placementName);

        /// <summary>
        /// Display a Rewarded Ad
        /// </summary>
        /// <param name="placementName">The name of the Ad placement</param>
        /// <param name="rewardedCallback"> The action to perform when the user can receive the reward. The boolean corresponds to the status of the reward callback (success or fail) </param>
        public static void ShowRewarded(string placementName = VoyagerMediation.DefaultRewardedPlacementName,
            Action<bool> rewardedCallback = null) => VoyagerMediation.ShowRewarded(placementName, rewardedCallback);
        
        /// <summary>
        /// Display a Banner Ad
        /// </summary>
        public static void ShowBanner() => VoyagerMediation.ShowBanner();
        
        /// <summary>
        /// Close current Banner Ad
        /// </summary>
        public static void HideBanner() => VoyagerMediation.HideBanner();
    }
}