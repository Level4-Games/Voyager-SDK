using System.Collections.Generic;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using UnityEditor;

namespace Voyager_SDK.Editor
{
    public abstract class FacebookSettingsAdapter : SettingsAdapterBase
    {
        public static FacebookSettings Settings => GetFacebookSettings();

        private static FacebookSettings GetFacebookSettings()
        {
            var settings = GetFromDefaultSDKPath<FacebookSettings>(FacebookSettings.FacebookSettingsPath);

            if (settings == null)
                settings = VoyagerResources.LoadFromResources<FacebookSettings>();

            if (settings == null)
                settings = VoyagerSettingsEditor.CreateSettingAsset<FacebookSettings>("Facebook Settings");

            return settings;
        }

        public static void ApplySettings()
        {
            var voyagerSettings = VoyagerSettingsAdapter.Settings;
            GetFacebookSettings();
            
            if (!string.IsNullOrEmpty(voyagerSettings.FacebookID))
            {
                FacebookSettings.AppIds = new List<string> { voyagerSettings.FacebookID };
                EditorUtility.SetDirty(FacebookSettings.Instance);
            }
            
            if (!string.IsNullOrEmpty(voyagerSettings.FacebookClientToken))
            {
                FacebookSettings.ClientTokens = new List<string> { voyagerSettings.FacebookClientToken };
                EditorUtility.SetDirty(FacebookSettings.Instance);
            }

            if (!FacebookSettings.AppLabels.Contains(UnityEngine.Application.productName))
            {
                FacebookSettings.AppLabels = new List<string> { UnityEngine.Application.productName };
                EditorUtility.SetDirty(FacebookSettings.Instance);
            }

            if (ManifestMod.CheckManifest())
            {
                ManifestMod.GenerateManifest();
            }
        }
    }
}