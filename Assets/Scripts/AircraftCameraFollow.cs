using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 3f, -10f);

    private void LateUpdate()
    {
        if (_target == null)
            return;

        transform.position = _target.TransformPoint(_offset);
        transform.LookAt(_target.position, Vector3.up);
    }
}
