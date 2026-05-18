using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FlightHudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _altitudeText;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _throttleText;
        [SerializeField] private TMP_Text _rpmText;
        [SerializeField] private TMP_Text _pitchText;
        [SerializeField] private TMP_Text _rollText;
        [SerializeField] private TMP_Text _yawText;

        public void SetData(
            double altitudeFt,
            double speedKts,
            double throttle,
            double rpm,
            double pitchDeg,
            double rollDeg,
            double headingDeg)
        {
            _altitudeText.text = $"ALT: {altitudeFt:0} ft";
            _speedText.text = $"SPD: {speedKts:0.0} kts";
            _throttleText.text = $"THR: {throttle:0.00}";
            _rpmText.text = $"RPM: {rpm:0}";

            _pitchText.text = $"PITCH: {pitchDeg:0.0}";
            _rollText.text = $"ROLL: {rollDeg:0.0}";
            _yawText.text = $"HDG: {headingDeg:0.0}";
        }
    }
}
