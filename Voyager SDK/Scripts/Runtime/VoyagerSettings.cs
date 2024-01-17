using UnityEngine;

namespace Voyager_SDK
{
    public class VoyagerSettings : SingletonSettings<VoyagerSettings>
    {
        [Header("Facebook")]
        [SerializeField] private string facebookID;
        [SerializeField] private string facebookClientToken;
        
        public string FacebookID => facebookID.Trim();
        public string FacebookClientToken => facebookClientToken.Trim();
        
        [Header("Game Analytics")]
        [SerializeField] private string androidGameKey;
        [SerializeField] private string androidSecretKey;
        
        [SerializeField] private string iOSGameKey;
        [SerializeField] private string iOSSecretKey;

        public string AndroidGameKey => androidGameKey.Trim();
        public string AndroidSecretKey => androidSecretKey.Trim();
        public string IOSGameKey => iOSGameKey.Trim();
        public string IOSSecretKey => iOSSecretKey.Trim();

        
        [Header("Adjust")]
        [SerializeField] private string androidAppToken;
        [SerializeField] private string iOSAppToken;
        
        public string AppToken => Application.platform == RuntimePlatform.Android
            ? androidAppToken.Trim()
            : iOSAppToken.Trim();
        
        [Header("Applovin Max Sdk"), Multiline]
        [SerializeField] private string maxSdkKey = "09mBPe6fn7Tg_xo6p4-shNiAaXlBrtK4zAFXmPKNwdK3df-td8R7o5CgUWUpH3LQb2Mxxmp8AKngmcXgROmQJV";

        [SerializeField] private string androidAdmobAppID;
        [SerializeField] private string iOSAdmobAppID;
        
        public string MaxSDKKey => maxSdkKey.Trim();
        public string AndroidAdmobAppID => androidAdmobAppID.Trim();
        public string IOSAdmobAppID => iOSAdmobAppID.Trim();

        [Header("Android Ad Units")]
        [SerializeField] private string androidBannerAdUnitId = string.Empty;
        [SerializeField] private string androidInterstitialAdUnitId = string.Empty;
        [SerializeField] private string androidRewardedAdUnitId = string.Empty;
        
        [Header("IOS Ad Units")]
        [SerializeField] private string iOSBannerAdUnitId = string.Empty;
        [SerializeField] private string iOSInterstitialAdUnitId = string.Empty;
        [SerializeField] private string iOSRewardedAdUnitId = string.Empty;
        
        public string InterstitialId => Application.platform == RuntimePlatform.Android
            ? androidInterstitialAdUnitId.Trim()
            : iOSInterstitialAdUnitId.Trim();

        public string RewardedId => Application.platform == RuntimePlatform.Android
            ? androidRewardedAdUnitId.Trim()
            : iOSRewardedAdUnitId.Trim();

        public string BannerId => Application.platform == RuntimePlatform.Android
            ? androidBannerAdUnitId.Trim()
            : iOSBannerAdUnitId.Trim();
        
        [Header("Ad Settings")] 
        public int InitialInterstitialDuration = 120;
        public int InterstitialInterval = 90;
    }
}