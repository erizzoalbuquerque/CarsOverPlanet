using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogueController : MonoBehaviour
{
    public TextMeshPro _tmp;

    public float _duration = 5f;

    public bool _debugTrigger = false;

    Sequence speakSequence;

    public List<string> _curseLines;
    public List<string> _victoryLines;

    // Start is called before the first frame update
    void Start()
    {
        _debugTrigger = false;

        this.transform.localScale = Vector3.zero;        
    }

    // Update is called once per frame
    void Update()
    {
        if (_debugTrigger)
            Speak();
    }

    [ContextMenu("Curse")]
    public void SayACurse()
    {
        int index = Random.Range(0, _curseLines.Count - 1);
        _tmp.text = _curseLines[index];

        Speak();
    }

    [ContextMenu("Curse")]
    public void SayAVictoryLine()
    {
        int index = Random.Range(0, _victoryLines.Count - 1);
        _tmp.text = _victoryLines[index];

        Speak();
    }


    [ContextMenu("Speak")]
    public void Speak()
    {
        this.transform.localScale = Vector3.zero;

        speakSequence.Kill();
        speakSequence = DOTween.Sequence();

        speakSequence.Append(transform.DOScale(1f, 1f).SetEase(Ease.OutElastic));
        speakSequence.AppendInterval(_duration);
        speakSequence.Append(transform.DOScale(0f, 0.2f));
    }

    private void OnDestroy()
    {
        speakSequence.Kill();
    }
}
