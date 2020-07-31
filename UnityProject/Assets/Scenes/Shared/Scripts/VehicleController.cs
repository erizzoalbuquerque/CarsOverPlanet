using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VehicleController : MonoBehaviour
{
    [Space()]
    [Header("Movement Parameters")]
    /// <summary>
    /// Max acceleration towards foward speed target.
    /// </summary>
    [SerializeField] float _maxForwardAcceleration = 30f;
    /// <summary>
    /// Max acceleration towards steering speed target.
    /// </summary>
    [SerializeField] float _maxSteeringAcceleration = 1000f;
    /// <summary>
    /// Min forward speed so vehicle can steer at full speed. It can't steer when speed is 0. Then it linearly interpolates to full steering.
    /// </summary>
    [SerializeField] float _minForwardSpeedToFullSteering = 1f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _maxSteeringSpeed = 10f;

    [Space()]
    [Header("Other Settings")]
    [Space()]
    /// <summary>
    /// Min force to register crash event.
    /// </summary>
    [SerializeField] float _minForceToCrash = 100f;
    [SerializeField] LayerMask _crashLayerMask = -1;
    [SerializeField] float _minTimeBetweenCrashEvents = 0f;
    [SerializeField] UnityEvent _crashEvent = null;

    Rigidbody _rb;
    float _forwardInput,_steeringInput;
    float _lastCrashEventTime = 0f;

    float _forwardSpeed, _desiredForwardSpeed;
    float _steeringSpeed, _desiredSteeringSpeed;

    EngineSoundController engineSoundController;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _lastCrashEventTime = Time.time;
        engineSoundController = GetComponent<EngineSoundController>();
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

        if (engineSoundController != null)
        {
            engineSoundController.TargetIntensity = Mathf.Abs(forward);
        }
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
        _steeringSpeed = Mathf.MoveTowards(_steeringSpeed, _desiredSteeringSpeed, maxSteeringSpeedChange);
        //print("Desired Steering: " + _desiredSteeringSpeed + " Steering: " + _steeringSpeed);

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

        float crashForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        print("[" + this.gameObject.name + "]" + " COLLIDED against: [" + collision.transform.name + "] || Crash Force: " + crashForce);
        if (_minForceToCrash != 0f && (crashForce) > _minForceToCrash)
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
