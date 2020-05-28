using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float _gravity = 10f;
    public Transform _planet;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.AddForce((_planet.position - this.transform.position).normalized * _rb.mass * _gravity);
    }
}
