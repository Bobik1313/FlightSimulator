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

        public void SetProperty(string property, double value)
        {
            JSBSimNative.JSB_SetProperty(property, value);
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
