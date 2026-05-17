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

        public void SetInitialConditions()
        {
            JSBSimNative.JSB_SetProperty("ic/h-sl-ft", 1000.0);
            JSBSimNative.JSB_SetProperty("ic/terrain-elevation-ft", 0.0);
            JSBSimNative.JSB_SetProperty("ic/vc-kts", 80.0);
            JSBSimNative.JSB_SetProperty("ic/gamma-deg", 0.0);

            var result = JSBSimNative.JSB_RunInitialConditions();

            if (!result)
                Debug.LogError(JSBSimNative.GetLastError());
            else
            {
                Debug.Log("After IC Alt: " + JSBSimNative.JSB_GetProperty("position/h-sl-ft"));
                Debug.Log("After IC VC: " + JSBSimNative.JSB_GetProperty("velocities/vc-kts"));
                Debug.Log("After IC Pitch: " + JSBSimNative.JSB_GetProperty("attitude/theta-rad"));
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
        }
    }
}
