using UnityEngine;

public class CrateKicker : MonoBehaviour
{
    [SerializeField] 
    LayerMask _kickableLayerMask = -1;

    [SerializeField]
    [Tooltip("Multiple of the collision magnitude applied into the other collider center of gravity.")]
    float _cogKick = 0.0f;

    [SerializeField]
    [Tooltip("Multiple of the collision magnitude applied as a random rotation into the other collider.")]
    float _rotationKick = 0.0f;

    void OnCollisionEnter(Collision collision)
    {
        if ((_kickableLayerMask & (1 << collision.collider.gameObject.layer)) == 0)
            return;

        Rigidbody otherrb = collision.collider.attachedRigidbody;
        if (otherrb == null)
            return;

        float collisionMagnitude = collision.impulse.magnitude;

        Vector3 toCog = otherrb.worldCenterOfMass - collision.GetContact(0).point;
        otherrb.AddForce(toCog.normalized * _cogKick * collisionMagnitude, ForceMode.Impulse);

        otherrb.maxAngularVelocity = 100.0f;
        otherrb.AddTorque(Random.onUnitSphere * collisionMagnitude * _rotationKick, ForceMode.Impulse);
    }
}
