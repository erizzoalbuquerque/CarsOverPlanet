﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField]
    Text text;

    [SerializeField,Range(1.0f,2.0f)]
    float punchScaleMultiplier = 1.2f;

    [SerializeField, Range(0f, 1f)]
    float punchScaleDuration = 0.3f;

    int score;

    // Start is called before the first frame update
    void Start()
    {
        score = -1;
        UpdateScore(score, false);
    }

    void Display(bool animate = true)
    {
        text.text = score.ToString();

        if (animate)
        {
            text.rectTransform.localScale = punchScaleMultiplier * Vector3.one;
            text.rectTransform.DOScale(Vector3.one, punchScaleDuration).SetEase(Ease.OutBounce);
        }
    }

    public void UpdateScore(int newScore, bool animate = true)
    {
        score = newScore;
        Display(animate);
    }

    [ContextMenu("TestUpdate")]
    void TestUpdate()
    {
        UpdateScore(score + 1);
    }
}