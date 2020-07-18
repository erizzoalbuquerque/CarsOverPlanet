using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSoundController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Vector2 pitchRange = Vector2.zero;
    [SerializeField] float volume = 1f;
    [SerializeField] float maxForce;
    [SerializeField] AudioClip[] audios;
    [SerializeField] LayerMask crashLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayCrashSound(float intensity)
    {
        AudioClip audio = audios[Random.Range(0, audios.Length - 1)];

        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;

        audioSource.PlayOneShot(audio, intensity * volume);
        print("played");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crashLayerMask != (crashLayerMask | (1 << collision.collider.gameObject.layer)))
            return;

        float crashForce = collision.impulse.magnitude / Time.fixedDeltaTime;

        PlayCrashSound(Mathf.Clamp01(crashForce / maxForce));
    }
}
