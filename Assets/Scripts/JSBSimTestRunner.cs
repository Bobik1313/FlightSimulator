using Assets.Scripts;
using Assets.Scripts.JSBSim;
using UnityEngine;
using Zenject;


public class JSBSimTestRunner : MonoBehaviour
{
    [SerializeField] private AircraftInput aircraftInput;
    [SerializeField] private Transform aircraft;

    private IJSBSimService _service;
    private IJSBSimAircraft _aircraft;

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

        if (!_aircraft.Load("c172x"))
            return;

        _aircraft.SetInitialConditions();
    }

    private void FixedUpdate()
    {
        if (!_aircraft.IsLoaded || aircraftInput == null)
            return;

        _aircraft.SetControls(aircraftInput.Pitch, aircraftInput.Roll, aircraftInput.Yaw, aircraftInput.Throttle);

        _aircraft.Tick();

        var altitudeFt = _aircraft.GetProperty("position/h-sl-ft");
        var rollRad = _aircraft.GetProperty("attitude/phi-rad");
        var pitchRad = _aircraft.GetProperty("attitude/theta-rad");
        var headingRad = _aircraft.GetProperty("attitude/psi-rad");

        ApplyRotation(rollRad, pitchRad, headingRad);

        Debug.Log($"Alt: {altitudeFt}, Roll: {rollRad}, Pitch: {pitchRad}, Heading: {headingRad}");
    }

    private void ApplyRotation(double rollRad, double pitchRad, double headingRad)
    {
        if (aircraft == null)
            return;

        var rollDeg = (float)(rollRad * Mathf.Rad2Deg);
        var pitchDeg = (float)(pitchRad * Mathf.Rad2Deg);
        var headingDeg = (float)(headingRad * Mathf.Rad2Deg);

        aircraft.rotation = Quaternion.Euler(
            pitchDeg,
            headingDeg,
            -rollDeg);
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