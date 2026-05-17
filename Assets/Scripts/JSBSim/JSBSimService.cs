using UnityEngine;

namespace Assets.Scripts.JSBSim
{
    public interface IJSBSimService
    {
        bool IsInitialized { get; }
        bool Initialize();
        void Shutdown();
    }

    public sealed class JSBSimService : IJSBSimService
    {
        public bool IsInitialized { get; private set; }

        public bool Initialize()
        {
            Debug.Log("[JSBSim] Init start");

            var root = Application.streamingAssetsPath + "/JSBSim";

            var result = JSBSimNative.JSB_Init(
                root,
                root + "/aircraft",
                root + "/engine",
                root + "/systems");

            if (!result)
                Debug.LogError(JSBSimNative.GetLastError());

            IsInitialized = result;
            return result;
        }

        public void Shutdown()
        {
            if (!IsInitialized)
                return;

            JSBSimNative.JSB_Shutdown();

            IsInitialized = false;

            Debug.Log("JSBSim shutdown");
        }
    }
}
