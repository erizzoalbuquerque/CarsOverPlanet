using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public float _slowMoMultiplier = .1f;
    public float _slowMoTime = 1f;

    public CinemachineVirtualCamera playerVcam;
    public CinemachineVirtualCamera playerCrashVcam;
    public CinemachineVirtualCamera playerPostCrashVcam;

    public GameObject uiBusted;

    [Header("Player References")]
    public PlayerInput _playerInput;
    public VehicleController _VehicleController;
    public PlanetGravity _playerPlanetGravity;
    public Aligner _playerAligner;
    public RadialDistanceFixer _playerRadialDistanceFixer;
    public Rigidbody _playerRigidBody;
    public GameObject _smokeTrail;
    public GameObject _explosion;
    //public GameObject _crashSmoke;

    [Space(), Header("Score Display")]
    public ScoreDisplayer _scoreDisplayer;

    [Space(), Header("Audio")]
    public AudioMixerSnapshot _defaultAudioSnapshot;
    public AudioMixerSnapshot _crashingAudioSnapshot;
    public AudioMixerSnapshot _gameOverAudioSnapshot;

    bool _playerChrashed = false;

    bool _gameOver = false;

    int _score;


    void Start()
    {
        _gameOver = false;
        _score = 0;
        _scoreDisplayer.UpdateScore(_score, false);

        _defaultAudioSnapshot.TransitionTo(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    public void OnPlayerCrashed()
    {
        if (_playerChrashed == true)
            return;

        Time.timeScale = _slowMoMultiplier;
        Time.fixedDeltaTime = Time.fixedDeltaTime * _slowMoMultiplier;

        playerVcam.gameObject.SetActive(false);
        playerCrashVcam.gameObject.SetActive(true);
        playerCrashVcam.transform.SetPositionAndRotation(playerVcam.transform.position, playerVcam.transform.rotation);

        _playerInput.enabled = false;
        _VehicleController.enabled = false;
        _playerPlanetGravity.enabled = true;
        _playerAligner.enabled = false;
        _playerRadialDistanceFixer.enabled = false;
        _playerRigidBody.mass *= 0.5f;

        _smokeTrail.SetActive(false);
        _explosion.SetActive(true);

        //DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, _slowMoTime).SetEase(Ease.InOutCubic).OnComplete(DoSlowMotionCompleted);

        _playerChrashed = true;
        
        _crashingAudioSnapshot.TransitionTo(0.1f);

        Invoke("DoSlowMotionCompleted", _slowMoTime);
    }

    void DoSlowMotionCompleted()
    {
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, _slowMoTime);
        DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, Time.fixedDeltaTime * 1f / _slowMoMultiplier, _slowMoTime);
        playerPostCrashVcam.gameObject.SetActive(true);
        playerCrashVcam.gameObject.SetActive(false);

        Invoke("ActivateBustedText", 1.5f);

        //_crashSmoke.SetActive(true);
    }

    void ActivateBustedText()
    {
        uiBusted.SetActive(true);
        _gameOver = true;

        _gameOverAudioSnapshot.TransitionTo(5f);
    }

    void Restart()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnEnemyCrashed()
    {
       if (!_gameOver)
       {
            _score += 1;
            _scoreDisplayer.UpdateScore(_score);
       }
    }
}
