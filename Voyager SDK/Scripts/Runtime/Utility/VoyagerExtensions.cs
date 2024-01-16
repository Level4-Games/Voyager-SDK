using UnityEngine;

namespace Voyager_SDK
{
    public static class VoyagerExtensions
    {
        public static string RichTextColor(this string log, Color color)
        {
            return
                $"<color=#{(byte)(color.r * 255f):X2}{(byte)(color.g * 255f):X2}{(byte)(color.b * 255f):X2}>{log}</color>";
        }

        public static void VoyagerLog(string message)
        {
            Debug.LogWarning("Voyager SDK :".RichTextColor(VoyagerResources.VoyagerSDKLogColor) + message);
        }
    }
}
