using UnityEditor;

namespace Voyager_SDK.Editor
{
    public abstract class ApplovinSettingsAdapter : SettingsAdapterBase
    {
        //TODO: Automatize this : 
        /*
         * For Android builds, the AppLovin MAX plugin requires that you enable Jetifier. To enable Jetifier, take the following steps:
             - In Unity, select Assets > External Dependency Manager > Android Resolver > Settings.
             - In the Android Resolver Settings dialog that appears, check Use Jetifier.
             - Click OK.
         */

        //TODO: Control this : 
        /*
         * For iOS builds:
            - Disable bitcode. Building with bitcode is no longer supported. Apple deprecated Bitcode in Xcode 14.
            - The AppLovin MAX plugin requires CocoaPods. Install CocoaPods by following the instructions at the CocoaPods Getting Started guide.
         */
        
        public static AppLovinSettings Settings => GetApplovinSettings();

        private static AppLovinSettings GetApplovinSettings()
        {
            // var directoryPath = Path.GetDirectoryName(AppLovinSettings.SettingsExportPath);
            // var settings = GetFromDefaultSDKPath<AppLovinSettings>(directoryPath);
            var settings = AppLovinSettings.Instance;

            if (settings == null)
                settings = VoyagerResources.LoadFromResources<AppLovinSettings>();

            if (settings == null)
                settings = VoyagerSettingsEditor.CreateSettingAsset<AppLovinSettings>("Applovin Settings");

            return settings;
        }

        public static void ApplySettings()
        {
            var voyagerSettings = VoyagerSettingsAdapter.Settings;
            GetApplovinSettings();
            
            Settings.AdMobAndroidAppId = voyagerSettings.AndroidAdmobAppID;
            Settings.AdMobIosAppId = voyagerSettings.IOSAdmobAppID;
            Settings.SdkKey = voyagerSettings.MaxSDKKey;
            Settings.QualityServiceEnabled = true;
            Settings.ConsentFlowEnabled = false;
            
            AssetDatabase.Refresh();
        }
    }
}