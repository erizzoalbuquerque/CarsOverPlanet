using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField]
    Text text = null;

    [SerializeField,Range(1.0f,2.0f)]
    float punchScaleMultiplier = 1.2f;

    [SerializeField, Range(0f, 1f)]
    float punchScaleDuration = 0.3f;

    [SerializeField]
    AudioClip scoreSound;

    [SerializeField]
    float soundVolume = 1f;

    [SerializeField]
    float delayInSeconds = 1f;

    int score;
    AudioSource audioSource;

    void Awake()
    {
        score = -1;
        UpdateScore(score, false);
        audioSource = GetComponent<AudioSource>();
    }

    void Display(bool animate = true)
    {
        text.text = score.ToString();

        if (animate)
        {
            text.rectTransform.localScale = punchScaleMultiplier * Vector3.one;
            text.rectTransform.DOScale(Vector3.one, punchScaleDuration).SetEase(Ease.OutBounce);
            
            if (audioSource != null && scoreSound != null)
                audioSource.PlayOneShot(scoreSound, soundVolume);
        }
    }

    public void UpdateScore(int newScore, bool animate = true)
    {
        StartCoroutine(UpdateScoreCoroutine(newScore, animate));
    }

    IEnumerator UpdateScoreCoroutine(int newScore, bool animate)
    {
        yield return new WaitForSeconds(delayInSeconds);

        score = newScore;
        Display(animate);

        yield return 0;
    }


    [ContextMenu("TestUpdateScore")]
    void TestUpdate()
    {
        UpdateScore(score + 1);
    }
}
