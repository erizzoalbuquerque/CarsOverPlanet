using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget: MonoBehaviour
{
    public Transform _target;
    public Transform _upReference;

    public float smooth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(_target.position - this.transform.position, _upReference.up);
        this.transform.rotation = Quaternion.Lerp(rotation, this.transform.rotation, smooth);
    }
}
