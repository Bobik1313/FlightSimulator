using UnityEngine;

namespace Assets.Scripts.JSBSim
{
    public interface IJSBSimService
    {
        bool Initialize();
    }

    public sealed class JSBSimService : IJSBSimService
    {
        public bool Initialize()
        {
            var root = Application.streamingAssetsPath + "/JSBSim";

            var result = JSBSimNative.JSB_Init(
                root,
                root + "/aircraft",
                root + "/engine",
                root + "/systems");

            if (!result)
                Debug.LogError(JSBSimNative.GetLastError());

            return result;
        }
    }
}
