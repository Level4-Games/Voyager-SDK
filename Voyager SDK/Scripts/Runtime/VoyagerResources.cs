using System.Linq;
using UnityEngine;

namespace Voyager_SDK
{
    public abstract class VoyagerResources
    {
        public const string AssetFolder = "Assets";
        public const string ResourceFolder = "Resources";
        public const string SettingsFolder = "Voyager SDK";

        public const string ResourcePath = AssetFolder + "/" + ResourceFolder;
        public const string SettingsPath = ResourcePath + "/" + SettingsFolder;

        public static readonly Color VoyagerSDKLogColor = new(1f, 0.4f, 1f);

        public static Y LoadFromResources<Y>() where Y : Object
        {
            // var settings = Resources.Load(nameof(T)) as T;
            var settings = Resources.LoadAll<Y>(SettingsFolder).FirstOrDefault();

            if (settings != null)
                return settings;

            VoyagerExtensions.VoyagerLog(" Settings File Missing !".RichTextColor(Color.white));

            return null;
        }
    }
}
