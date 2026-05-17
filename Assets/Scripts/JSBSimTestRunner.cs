using Assets.Scripts.JSBSim;
using UnityEngine;
using Zenject;


public class JSBSimTestRunner : MonoBehaviour
{
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
        if (!_aircraft.IsLoaded)
            return;

        _aircraft.Tick();

        var altitudeFt = _aircraft.GetProperty("position/h-sl-ft");
        var rollRad = _aircraft.GetProperty("attitude/phi-rad");
        var pitchRad = _aircraft.GetProperty("attitude/theta-rad");
        var headingRad = _aircraft.GetProperty("attitude/psi-rad");

        Debug.Log($"Alt: {altitudeFt}, Roll: {rollRad}, Pitch: {pitchRad}, Heading: {headingRad}");
    }
}