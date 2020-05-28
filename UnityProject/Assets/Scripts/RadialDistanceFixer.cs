using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadialDistanceFixer : MonoBehaviour
{
    public Transform _center;
    public bool _hasRigidBody = false;
    Transform _oldCenter;
    Rigidbody _rb;

    public float _distance = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if(!_hasRigidBody)
        {
            this.transform.position = delta.normalized * _distance;
        }
        else
        {
            if(_rb == null)  _rb = GetComponent<Rigidbody>();
            _rb.MovePosition(delta.normalized * _distance);
        }
    }

    void Update()
    {
        if (!Application.isEditor)
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
