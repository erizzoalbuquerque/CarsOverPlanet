using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Aligner : MonoBehaviour
{
    public Transform _center;
    public bool _hasRigidBody;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Application.isPlaying)
            return;

        Align();
    }

    void Align()
    {
        Quaternion rotation = Quaternion.FromToRotation(transform.up, this.transform.position - _center.position);

        if (!_hasRigidBody)
        {
            this.transform.rotation = rotation * transform.rotation;
        }
        else
        {
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            _rb.MoveRotation(rotation * transform.rotation);
        }
    }

    void Update()
    {
        if (Application.isPlaying)
            return;

        Align();
    }

}
