using System;

namespace Voyager_SDK
{
    public class VoyagerAppManager : MonoSingleton<VoyagerAppManager>
    {
        public Action<bool> OnApplicationPauseListener;

        protected override void Init()
        {
            base.Init();
        }

        private void OnApplicationPause(bool paused)
        {
            OnApplicationPauseListener?.Invoke(paused);
        }
    }
}