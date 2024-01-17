using UnityEngine;

namespace Voyager_SDK
{
    public class VoyagerInitializer : MonoBehaviour
    {
        private void Awake()
        {
            VoyagerMediation.Initialize();
        }
    }
}
