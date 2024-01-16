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
                settings = VoyagerSettingsEditor.CreateSettingAsset<VoyagerSettings>();

            return settings;
        }
    }
}
