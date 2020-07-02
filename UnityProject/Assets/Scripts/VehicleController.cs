using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleController : MonoBehaviour
{
    public float _maxForwardAcceleration;
    public float _maxSteeringAcceleration;
    public float _minForwardSpeedToFullSteering = 1f;
    public float _maxSpeed = 10f;
    public float _maxSteeringSpeed = 10f;

    public float _minForceToCrash = 100f;
    public LayerMask _crashLayerMask;
    public float _minTimeBetweenCrashEvents = 0f;
    public UnityEvent _crashEvent;

    Rigidbody _rb;
    float _forwardInput,_steeringInput;
    float _lastCrashEventTime = 0f;

    float _forwardSpeed, _desiredForwardSpeed;
    float _steeringSpeed, _desiredSteeringSpeed;



    void Awake()
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

        _desiredForwardSpeed = _forwardInput * _maxSpeed;
        _desiredSteeringSpeed = _steeringInput * _maxSteeringSpeed;
    }

    void Move()
    {
        //Forward Speed --------
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);

        _forwardSpeed = localVelocity.z;
        float maxSpeedChange = _maxForwardAcceleration * Time.deltaTime;
        _forwardSpeed = Mathf.MoveTowards(_forwardSpeed, _desiredForwardSpeed, maxSpeedChange);

        //print("desired: " + _desiredForwardSpeed.ToString() + " current: " +  _forwardSpeed.ToString());

        localVelocity = new Vector3(localVelocity.x, localVelocity.y, _forwardSpeed);
        _rb.velocity = transform.TransformDirection(localVelocity);

        //Steering Speed --------
        Vector3 angularVelocity = _rb.angularVelocity;
        Vector3 localAngularVelocity = _rb.transform.InverseTransformDirection(angularVelocity);

        _steeringSpeed = localAngularVelocity.y;
        float maxSteeringSpeedChange = _maxSteeringAcceleration * Time.deltaTime;
        //_steeringSpeed = Mathf.MoveTowards(_steeringSpeed, _desiredSteeringSpeed, maxSteeringSpeedChange);
        _steeringSpeed = _desiredSteeringSpeed;

        //Slow Down Steering if Forward Speed is too slow
        _steeringSpeed = _steeringSpeed * Mathf.Clamp01(_forwardSpeed / _minForwardSpeedToFullSteering);

        localAngularVelocity = new Vector3(
            localAngularVelocity.x,
            _steeringSpeed,
            localAngularVelocity.z);
        angularVelocity = _rb.transform.TransformDirection(localAngularVelocity);
        _rb.angularVelocity = angularVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_crashLayerMask != (_crashLayerMask | (1 << collision.collider.gameObject.layer)))
            return;
        print(this.gameObject.name + " COLLIDED: " + collision.impulse.magnitude / Time.fixedDeltaTime);

        if (_minForceToCrash != 0f && (collision.impulse.magnitude / Time.fixedDeltaTime) > _minForceToCrash)
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
