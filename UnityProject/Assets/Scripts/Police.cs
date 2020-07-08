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
    public Vector3 _followOffset = Vector3.zero;
    public bool _periodicOffset = false;
    public float _period = 0f;
    public AnimationCurve _offsetGainCurve;
    public bool _debugTarget = false;
    public Color _debugTargetColor = Color.red;

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
            float offsetGain;
            if (_periodicOffset)
            {
                offsetGain = _offsetGainCurve.Evaluate((Time.time % _period) / _period);
            }
            else
                offsetGain = 1f;

            _targetLastKnownPosition = _playerTransform.position + _playerTransform.TransformDirection(_followOffset) * offsetGain;
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

    private void OnDrawGizmos()
    {
        if (_debugTarget)
        {
            Gizmos.color = _debugTargetColor;
            Gizmos.DrawSphere(_targetLastKnownPosition, 0.2f);
        }
    }
}
