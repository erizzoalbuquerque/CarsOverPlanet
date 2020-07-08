using UnityEngine;
using DG.Tweening;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField] float minForceToCrash = 0f;
    [SerializeField] float minTimeBetweenCrash = 0f;
    [SerializeField] LayerMask crashLayerMask = -1;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeStrength = 1f;
    [SerializeField] int shakeVibratto = 10;

    float lastCrashTime = 0f;
    ParticleSystem foilageParticle;

    // Start is called before the first frame update
    void Awake()
    {
        foilageParticle = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crashLayerMask != (crashLayerMask | (1 << collision.collider.gameObject.layer)))
            return;

        float crashForce = collision.impulse.magnitude / Time.deltaTime;
        //print(this.gameObject.name + " Crash Force: " + crashForce);
        if (crashForce > minForceToCrash)
            Crash();    
    }

    [ContextMenu("Do Crash")]
    void Crash()
    {
        if (Time.time - lastCrashTime < minTimeBetweenCrash)
            return; 
        
        this.transform.DOShakeRotation(shakeDuration, shakeStrength,shakeVibratto,90,true);
        foilageParticle.Play();
        lastCrashTime = Time.time;
    }
}
