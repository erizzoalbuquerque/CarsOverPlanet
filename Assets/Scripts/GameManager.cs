using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;

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

    bool _playerChrashed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    public void DoSlowMotion()
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
        
        Invoke("DoSlowMotionCompleted", _slowMoTime);
    }

    void DoSlowMotionCompleted()
    {
        //Time.timeScale = 1.0f;
        //Time.fixedDeltaTime = Time.fixedDeltaTime * 1f / _slowMoMultiplier;
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
    }

    void Restart()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
