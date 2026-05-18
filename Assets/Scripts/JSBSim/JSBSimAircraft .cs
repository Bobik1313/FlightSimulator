using UnityEngine;

namespace Assets.Scripts.JSBSim
{
    public class JSBSimAircraft : IJSBSimAircraft
    {
        public bool IsLoaded { get; private set; }

        public bool Load(string aircraftName)
        {
            IsLoaded = JSBSimNative.JSB_LoadAircraft(aircraftName);

            if (!IsLoaded)
                Debug.LogError(JSBSimNative.GetLastError());

            return IsLoaded;
        }
        public bool LoadScript(string scriptPath)
        {
            IsLoaded = JSBSimNative.JSB_LoadScript(scriptPath);

            if (!IsLoaded)
                Debug.LogError(JSBSimNative.GetLastError());

            return IsLoaded;
        }

        public void SetInitialConditions()
        {
            var result = JSBSimNative.JSB_RunInitialConditions();

            if (!result)
                Debug.LogError(JSBSimNative.GetLastError());
            else
            {
                Debug.Log("After IC Alt: " + JSBSimNative.JSB_GetProperty("position/h-sl-ft"));
                Debug.Log("After IC VC: " + JSBSimNative.JSB_GetProperty("velocities/vc-kts"));
                Debug.Log("After IC Pitch: " + JSBSimNative.JSB_GetProperty("attitude/theta-rad"));

                Debug.Log("After IC RPM: " + JSBSimNative.JSB_GetProperty("propulsion/engine[0]/rpm"));
                Debug.Log("After IC Throttle: " + JSBSimNative.JSB_GetProperty("fcs/throttle-cmd-norm"));
                Debug.Log("After IC Mixture: " + JSBSimNative.JSB_GetProperty("fcs/mixture-cmd-norm"));
            }
        }

        public void Tick()
        {
            if (!IsLoaded)
                return;

            JSBSimNative.JSB_RunStep();
        }

        public double GetProperty(string property)
        {
            return JSBSimNative.JSB_GetProperty(property);
        }

        public void SetControls(double pitch, double roll, double yaw, double throttle)
        {
            if (!IsLoaded)
                return;

            JSBSimNative.JSB_SetProperty("fcs/elevator-cmd-norm", pitch);
            JSBSimNative.JSB_SetProperty("fcs/aileron-cmd-norm", roll);
            JSBSimNative.JSB_SetProperty("fcs/rudder-cmd-norm", yaw);
            JSBSimNative.JSB_SetProperty("fcs/throttle-cmd-norm", throttle);

            JSBSimNative.JSB_SetProperty("fcs/center-brake-cmd-norm", 0.0);
            JSBSimNative.JSB_SetProperty("fcs/left-brake-cmd-norm", 0.0);
            JSBSimNative.JSB_SetProperty("fcs/right-brake-cmd-norm", 0.0);
        }
    }
}
