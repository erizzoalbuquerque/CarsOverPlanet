using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public class Police : MonoBehaviour
{
    public Transform _playerTransform;
    public float _forwardGain = 1f;
    public float _steeringGain = 1f;
    public float _waitTime = 2f;

    private VehicleController _vc;
    private Vector3 _targetLastKnownPosition = Vector3.zero;
    private bool _chasing;

    // Start is called before the first frame update
    void Awake()
    {
        _vc = GetComponent<VehicleController>();
    }

    private void Start()
    {
        Invoke("Chase", _waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (_chasing == false)
            return;

        if (_playerTransform != null)
        {
            _targetLastKnownPosition = _playerTransform.position;
        }

        Vector3 directionToPlayerInLocalFrame = transform.InverseTransformDirection (_targetLastKnownPosition -  this.transform.position);
        float distance = directionToPlayerInLocalFrame.magnitude;

        float forward = _forwardGain * distance * Mathf.Sign(directionToPlayerInLocalFrame.z);
        float steering = _steeringGain * directionToPlayerInLocalFrame.x;

        _vc.SetInput(forward, steering);
    }

    void Chase()
    {
        _chasing = true;
    }
}
