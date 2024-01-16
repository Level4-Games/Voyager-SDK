using UnityEditor;
using UnityEngine;

namespace Voyager_SDK.Editor
{
    public abstract class SettingsAdapterBase
    {
        protected static T GetFromDefaultSDKPath<T>(string path) where T : Object
        {
            return !AssetDatabase.IsValidFolder(path)
                ? null
                : AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
