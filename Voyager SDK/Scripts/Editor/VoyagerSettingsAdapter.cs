namespace Voyager_SDK.Editor
{
    public abstract class VoyagerSettingsAdapter : SettingsAdapterBase
    {
        public static VoyagerSettings Settings => GetVoyagerSettings();

        private static VoyagerSettings GetVoyagerSettings()
        {
            var settings = GetFromDefaultSDKPath<VoyagerSettings>(VoyagerResources.SettingsPath);

            if (settings == null)
                settings = VoyagerResources.LoadFromResources<VoyagerSettings>();

            if (settings == null)
                settings = VoyagerSettingsEditor.CreateSettingAsset<VoyagerSettings>("Voyager Settings");

            return settings;
        }

        public static void ApplySettings()
        {
            GetVoyagerSettings();
            
            FacebookSettingsAdapter.ApplySettings();
            GASettingsAdapter.ApplySettings();
            ApplovinSettingsAdapter.ApplySettings();
        }
    }
}
