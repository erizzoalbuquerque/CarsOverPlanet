using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSoundController : MonoBehaviour
{
    [SerializeField] Vector2 engineVolumeRange = Vector2.zero;
    [SerializeField] Vector2 enginePitchRange = Vector2.zero;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float maxVariationPerSecond = 1f;

    [SerializeField, Range(0f,1f)] float intensity = 0f;

    public float TargetIntensity { get => intensity; set => intensity = value; }

    float currentPitch = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent <AudioSource>();
        audioSource.pitch = 0f;
        audioSource.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSound();
    }

    public void UpdateEngineSound()
    {
        float targetVolume = Mathf.Lerp(engineVolumeRange.x, engineVolumeRange.y, intensity);
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, maxVariationPerSecond * Time.deltaTime);

        float targetPitch = Mathf.Lerp(enginePitchRange.x, enginePitchRange.y, intensity);
        currentPitch = Mathf.MoveTowards(currentPitch, targetPitch, maxVariationPerSecond * Time.deltaTime);
        audioSource.pitch = currentPitch * Time.timeScale;
    }
}
