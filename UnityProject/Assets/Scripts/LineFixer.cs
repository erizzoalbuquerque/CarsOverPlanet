using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineFixer : MonoBehaviour
{
    public Transform _origin;
    public Transform _dot;

    public float _distanceFromDot = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CorrectDistance();
    }

    void CorrectDistance()
    {
        Vector3 direction = _dot.position - _origin.position;
        Vector3 newPosition = _distanceFromDot * direction.normalized + _dot.transform.position;

        this.transform.position = newPosition;
    }
}
