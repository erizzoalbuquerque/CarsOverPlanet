using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleController : MonoBehaviour
{
    public float _forwardMultiplier;
    public float _steeringMultiplier;
    public float _steeringConstant = 1f;
    public float _maxSpeed = 10f;

    public float _minSpeedToCrash = 5f;
    public LayerMask _crashLayerMask;
    public float _minTimeBetweenCrashEvents = 0f;
    public UnityEvent _crashEvent;

    Rigidbody _rb;
    float _forwardInput,_steeringInput;
    float _lastCrashEventTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lastCrashEventTime = Time.time;
    }

    void FixedUpdate()
    {
        Move();
    }

    public void SetInput(float forward, float steering)
    {
        _forwardInput = Mathf.Clamp(forward, -1f, 1f);
        _steeringInput = Mathf.Clamp(steering, -1f, 1f);
    }

    void Move()
    {
        _rb.AddForce(_rb.transform.forward * _forwardMultiplier * _forwardInput);
        //_rb.AddTorque(_rb.transform.up * _steeringInput * _steeringMultiplier);
        var newAngVel = _rb.angularVelocity;
        var newLocalAngVel = _rb.transform.InverseTransformDirection(newAngVel);
        newLocalAngVel = new Vector3(
            newLocalAngVel.x,
            _steeringInput * _steeringMultiplier * Mathf.Clamp01(_rb.velocity.magnitude * _steeringConstant),
            newLocalAngVel.z);
        newAngVel = _rb.transform.TransformDirection(newLocalAngVel);
        _rb.angularVelocity = newAngVel;

        //MaxSpeed
        if (_rb.velocity.magnitude > _maxSpeed)
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_crashLayerMask != (_crashLayerMask | (1 << collision.collider.gameObject.layer)))
            return;

        if (_minSpeedToCrash != 0f && collision.relativeVelocity.magnitude > _minSpeedToCrash)
            Crash();
    }

    void Crash()
    {
        if (Time.time - _lastCrashEventTime > _minTimeBetweenCrashEvents)
        {
            _crashEvent.Invoke();
            _lastCrashEventTime = Time.time;
        }
    }
}
