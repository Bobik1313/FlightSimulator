using Assets.Scripts;
using Assets.Scripts.JSBSim;
using UnityEngine;
using Zenject;


public class JSBSimTestRunner : MonoBehaviour
{
    [SerializeField] private AircraftInput _aircraftInput;
    [SerializeField] private Transform _aircraftTransform;

    private IJSBSimService _service;
    private IJSBSimAircraft _aircraft;

    private float _startAltitudeMeters;
    private float _startUnityY;
    private bool _hasStartAltitude;

    [Inject]
    private void Construct(IJSBSimService service, IJSBSimAircraft aircraft)
    {
        _service = service;
        _aircraft = aircraft;
    }

    private void Start()
    {
        if (!_service.Initialize())
            return;

        var scriptPath = Application.streamingAssetsPath + "/JSBSim/scripts/c172_unity.xml";

        if (!_aircraft.LoadScript(scriptPath))
            return;

        _aircraft.SetInitialConditions();
    }

    private void FixedUpdate()
    {
        if (!_aircraft.IsLoaded || _aircraftInput == null)
            return;

        _aircraft.SetControls(_aircraftInput.Pitch, _aircraftInput.Roll, _aircraftInput.Yaw, _aircraftInput.Throttle);

        _aircraft.Tick();

        var altitudeFt = _aircraft.GetProperty("position/h-sl-ft");
        var rollRad = _aircraft.GetProperty("attitude/phi-rad");
        var pitchRad = _aircraft.GetProperty("attitude/theta-rad");
        var headingRad = _aircraft.GetProperty("attitude/psi-rad");

        ApplyRotation(rollRad, pitchRad, headingRad);

        var altitudeMeters = (float)(altitudeFt * 0.3048);

        ApplyAltitude(altitudeMeters);

        var speedFps = _aircraft.GetProperty("velocities/vt-fps");
        var speedMps = speedFps * 0.3048;

        ApplyPosition(speedMps);


        var speedKts = _aircraft.GetProperty("velocities/vc-kts");
        var rpm = _aircraft.GetProperty("propulsion/engine[0]/rpm");
        var throttleCmd = _aircraft.GetProperty("fcs/throttle-cmd-norm");

        Debug.Log($"Alt: {altitudeFt}, Speed: {speedKts:F1}, Throttle: {throttleCmd:F2}, RPM: {rpm:F0}");
        var rpm0 = _aircraft.GetProperty("propulsion/engine[0]/rpm");
        var rpm1 = _aircraft.GetProperty("propulsion/engine/rpm");
        var rpm2 = _aircraft.GetProperty("propulsion/engine/engine-rpm");
        var thrust = _aircraft.GetProperty("propulsion/engine[0]/thruster/thrust-lbs");

        Debug.Log($"RPM0: {rpm0}, RPM1: {rpm1}, RPM2: {rpm2}, Thrust: {thrust}");
    }

    private void ApplyRotation(double rollRad, double pitchRad, double headingRad)
    {
        if (_aircraftTransform == null)
            return;

        var rollDeg = (float)(rollRad * Mathf.Rad2Deg);
        var pitchDeg = (float)(pitchRad * Mathf.Rad2Deg);
        var headingDeg = (float)(headingRad * Mathf.Rad2Deg);

        _aircraftTransform.rotation = Quaternion.Euler(
            -pitchDeg,
            headingDeg,
            -rollDeg);
    }

    private void ApplyAltitude(float altitudeMeters)
    {
        if (_aircraftTransform == null)
            return;

        if (!_hasStartAltitude)
        {
            _startAltitudeMeters = altitudeMeters;
            _startUnityY = _aircraftTransform.position.y;
            _hasStartAltitude = true;
        }

        var pos = _aircraftTransform.position;
        pos.y = _startUnityY + (altitudeMeters - _startAltitudeMeters);
        _aircraftTransform.position = pos;
    }

    private void ApplyPosition(double speedMps)
    {
        if (_aircraftTransform == null)
            return;

        var forward = _aircraftTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        _aircraftTransform.position += forward * (float)(speedMps * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        _service.Shutdown();
    }

    private void OnApplicationQuit()
    {
        _service.Shutdown();
    }
}