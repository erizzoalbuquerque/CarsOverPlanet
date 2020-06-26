using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadialDistanceFixer : MonoBehaviour
{
    public Transform _center;
    Transform _oldCenter;
    Rigidbody _rb;

    public float _distance = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Application.isPlaying)
            return;

        if (_center != _oldCenter)
        {
            _distance = (this.transform.position - _center.position).magnitude;
            _oldCenter = _center;
        }

        if (_center != null)
            CorrectDistance();              
    }

    void CorrectDistance()
    {
        Vector3 delta = this.transform.position - _center.position;
        if((_rb == null) || !Application.isPlaying)
        {
            this.transform.position = delta.normalized * _distance;
        }
        else
        {
            _rb.MovePosition(delta.normalized * _distance);
        }
    }

    void Update()
    {
        if (Application.isPlaying)
            return;

        if (_center != _oldCenter)
        {
            _distance = (this.transform.position - _center.position).magnitude;
            _oldCenter = _center;
        }

        if (_center != null)
            CorrectDistance();
    }
}
