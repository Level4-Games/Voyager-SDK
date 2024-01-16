using GameAnalyticsSDK.Setup;
using UnityEditor;
using UnityEngine;

namespace Voyager_SDK.Editor
{
    public abstract class GASettingsAdapter : SettingsAdapterBase
    {
        private const string GameAnalyticsPath = "/Resources/GameAnalytics";

        public static Settings Settings => GetSettings();

        private static Settings GetSettings()
        {
            var settings = GetFromDefaultSDKPath<Settings>(GameAnalyticsPath);

            if (settings == null)
                settings = VoyagerResources.LoadFromResources<Settings>();

            if (settings == null)
                settings = VoyagerSettingsEditor.CreateSettingAsset<Settings>();

            return settings;
        }

        public static void ApplySettings()
        {
            var voyagerSettings = VoyagerSettingsAdapter.Settings;

            UpdatePlatform(RuntimePlatform.Android, voyagerSettings.AndroidGameKey, voyagerSettings.AndroidSecretKey);
            UpdatePlatform(RuntimePlatform.IPhonePlayer, voyagerSettings.IOSGameKey, voyagerSettings.IOSSecretKey);

            Settings.UsePlayerSettingsBuildNumber = true;
            Settings.SubmitFpsAverage = false;
            Settings.SubmitFpsCritical = false;
            Settings.SubmitErrors = false;
            Settings.InfoLogBuild = true;
            Settings.InfoLogEditor = true;

            EditorUtility.SetDirty(Settings);
        }

        private static void UpdatePlatform(RuntimePlatform platform, string gameKey, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(gameKey) || string.IsNullOrWhiteSpace(secretKey))
                return;

            if (!Settings.Platforms.Contains(platform))
                Settings.AddPlatform(platform);

            var platformIndex = Settings.Platforms.IndexOf(platform);

            Settings.UpdateGameKey(platformIndex, gameKey);
            Settings.UpdateSecretKey(platformIndex, secretKey);
        }
    }
}