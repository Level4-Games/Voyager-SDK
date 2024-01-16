using System.Linq;
using UnityEditor;
using UnityEngine;
using static Voyager_SDK.VoyagerResources;

namespace Voyager_SDK.Editor
{
    [CustomEditor(typeof(VoyagerSettings))]
    public abstract class VoyagerSettingsEditor : UnityEditor.Editor
    {
        [MenuItem("Voyager/Voyager SDK/Show SDK Settings", false, 100)]
        private static void EditSDKSettings()
        {
            Selection.activeObject = VoyagerSettingsAdapter.Settings;
        }

        [MenuItem("Voyager/Voyager SDK/Show Facebook Settings", false, 100)]
        private static void EditFacebookSettings()
        {
            Selection.activeObject = FacebookSettingsAdapter.Settings;
        }

        [MenuItem("Voyager/Voyager SDK/Show GameAnalytics Settings", false, 100)]
        private static void EditGameAnalyticsSettings()
        {
            Selection.activeObject = GASettingsAdapter.Settings;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(15);
        
            if (GUILayout.Button("Apply Settings"))
            {
                ApplySettings();
            }
        
            EditorGUILayout.HelpBox("Unexpected Platform, Please set to Android or IOS ", MessageType.Error);   
        }
        
        [ContextMenu("Apply Settings")]
        private static void ApplySettings()
        {
            FacebookSettingsAdapter.ApplySettings();
            GASettingsAdapter.ApplySettings();
        }

        internal static T CreateSettingAsset<T>(string overrideName = "") where T : ScriptableObject
        {
            var settings = CreateInstance<T>();

            VoyagerExtensions.VoyagerLog($" Creating {typeof(T).Name} File Asset !".RichTextColor(Color.white));

            if (!AssetDatabase.IsValidFolder(ResourcePath))
                AssetDatabase.CreateFolder(AssetFolder, ResourceFolder);

            if (!AssetDatabase.IsValidFolder(SettingsPath))
                AssetDatabase.CreateFolder(ResourcePath, SettingsFolder);

            var name = string.IsNullOrEmpty(overrideName) ? typeof(T).Name : overrideName;
            
            AssetDatabase.CreateAsset(settings, SettingsPath + "/" + name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // settings = Resources.Load<T>(SettingsFolder + typeof(T).Name);
            settings = Resources.LoadAll<T>(SettingsFolder).FirstOrDefault();

            return settings;
        }
    }
}