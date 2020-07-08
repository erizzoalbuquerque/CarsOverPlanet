using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Aligner : MonoBehaviour
{
    public Transform _center;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Application.isPlaying)
        {
            if (_rb != null)
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, this.transform.position - _center.position);
                _rb.MoveRotation(rotation * transform.rotation);
            }
        }
        else
        {
            return;
        }
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            Quaternion rotation = Quaternion.FromToRotation(transform.up, this.transform.position - _center.position);
            this.transform.rotation = rotation * transform.rotation;
        }
        else
        {
            return;
        }
    }
}
