using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public Transform _target;

    public float _smooth = 0.9f;

    Vector3 _currentVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Vector3.Lerp(_target.transform.position, this.transform.position, _smooth);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, _target.position, ref _currentVelocity, 0.1f);
    }
}
