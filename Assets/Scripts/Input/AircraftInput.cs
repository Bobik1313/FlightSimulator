using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class AircraftInput : MonoBehaviour
    {
        private AircraftInputActions _input;

        public Vector2 Move { get; private set; }

        public float Pitch => -Move.y;
        public float Roll => Move.x;

        public float Yaw { get; private set; }

        public float Throttle { get; private set; }

        [SerializeField]
        private float throttleSpeed = 0.5f;

        private void Awake()
        {
            _input = new AircraftInputActions();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            Move = _input.Player.Move.ReadValue<Vector2>();

            Yaw = -_input.Player.Yaw.ReadValue<float>();

            float throttleUp =
                _input.Player.ThrottleUp.ReadValue<float>();

            float throttleDown =
                _input.Player.ThrottleDown.ReadValue<float>();

            Throttle += throttleUp * throttleSpeed * Time.deltaTime;
            Throttle -= throttleDown * throttleSpeed * Time.deltaTime;

            Throttle = Mathf.Clamp01(Throttle);
        }
    }
}
