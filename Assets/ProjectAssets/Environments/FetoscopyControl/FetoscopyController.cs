using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class FetoscopyController : MonoBehaviour
{
    public bool isGrabbed => controlPermission;

    [Header("Inputs")]
    [SerializeField] InputActionReference _toggleLaserRightInput;
    [SerializeField] InputActionReference _toggleLaserLeftInput;
    [SerializeField] InputActionReference _shotLaserRightInput;
    [SerializeField] InputActionReference _shotLaserLeftInput;

    [Header("Components")]
    [SerializeField] FetoscopyCamLaser _camLaser;
    [SerializeField] Camera _camera;

    [Header("Camera properties")]
    [SerializeField] float _cameraSpeed;

    [Header("Laser properties")]
    [SerializeField] float _laserMaxDistance;
    [SerializeField] float _shotsTime;
    [SerializeField] float _burnNormalOffset = 0.003f;
    [SerializeField] AudioSource _laserAudio;
    [SerializeField] AudioSource _laserToggleAudio;
    [SerializeField] LayerMask _surgeryMask;
    [SerializeField] LineRenderer _laserLine;
    [SerializeField] Transform _laserOrigin;
    [SerializeField] AudioClip _laserOutSound;
    [SerializeField] AudioClip _laserInSound;

    [Header("Laser effects")]
    [SerializeField] GameObject _laserDotPrefab;
    [SerializeField] Material _laserDotMaterial;
    [SerializeField] Color _laserDotIdleActive;
    [SerializeField] Color _laserDotShootingActive;
    [SerializeField] GameObject _laserBurnDecalPrefab;
    [SerializeField] GameObject _smokeParticlePrefab;
    [SerializeField] Material _laserHeadMaterial;

    [Header("Bleeding effects")]
    [SerializeField] Transform _laserBegin;
    [SerializeField] float _tresholdDistance;
    [SerializeField] float _animationDuration;
    [SerializeField] float _bleedFadeSpeed;
    [SerializeField] Image ScreenBorder;
    [SerializeField] Image TvBorder;

    private Transform laserDot;
    private GameObject laserBurnDecal;
    private ParticleSystem smokeParticle;
    private FetoscopyOrgan currentOrgan;

    private bool isShooting = false;
    private bool laserShooting = false;
    private bool laserActivated = false;
    private bool controlPermission = false;
    private bool inBleedAnimation = false;
    private float timer;
    private HandSides currentSide;
    private List<GameObject> laserBurns = new List<GameObject>();

    private void Start()
    {
        laserDot = Instantiate(_laserDotPrefab).transform;
        _laserDotMaterial.color = _laserDotIdleActive;
        laserBurnDecal = Instantiate(_laserBurnDecalPrefab);
        smokeParticle = Instantiate(_smokeParticlePrefab).GetComponent<ParticleSystem>();
        ChangeLaserHeadMaterial(0f, 1f);
        laserDot.gameObject.SetActive(false);
        _laserLine.enabled = false;
        timer = _shotsTime;
        TvBorder.color = new Color(TvBorder.color.r, TvBorder.color.g, TvBorder.color.b, 0f);
        ScreenBorder.color = new Color(ScreenBorder.color.r, ScreenBorder.color.g, ScreenBorder.color.b, 0f);
        inBleedAnimation = false;
    }

    private void Update()
    {
        if (controlPermission)
        {
            if (laserActivated)
            {
                float handGrabInput = GetHandGrabInput();
                if (handGrabInput > 0.9f)
                {
                    EnableLaserShooting();
                }
                else if (handGrabInput < 0.1f)
                {
                    DisableLaserShooting();
                }

                if (laserShooting) // Is shooting
                {
                    if (!isShooting)
                    {
                        EnableLaserShootingVFX();
                    }
                    else if (isShooting)
                    {
                        DisableLaserShootingVFX();
                    }

                    timer += Time.deltaTime;
                    if (timer >= _shotsTime)
                    {
                        LaserShooting();
                        timer = 0;
                    }
                }
                else               // Is not shooting
                {
                    if (isShooting)
                    {
                        DisableLaserShootingVFX();
                    }

                    LaserActiveIdle();
                }
            }
        }
    }

    private void EnableLaserShooting()
    {
        if(!laserShooting)
        {
            laserShooting = true;
            _laserToggleAudio.PlayOneShot(_laserInSound);
            ChangeLaserHeadMaterial(0.5f, 0.9f);
        }
    }

    private void DisableLaserShooting()
    {
        if(laserShooting)
        {
            laserShooting = false;
            if (isShooting) { DisableLaserShootingVFX(); }
            _laserToggleAudio.PlayOneShot(_laserOutSound);
            ChangeLaserHeadMaterial(0f, 1f);
        } 
    }

    private void StartLaser()
    {
        laserActivated = true;
        _camLaser.TurnOn();
        laserDot.gameObject.SetActive(true);
        _laserLine.enabled = true;

    }

    private void StopLaser()
    {
        DisableLaserShooting();
        laserActivated = false;
        _camLaser.TurnOff();
        laserDot.gameObject.SetActive(false);
        _laserLine.enabled = false;
    }

    private void ActivateLaserAction(InputAction.CallbackContext obj)
    {
        if (controlPermission)
        {
            laserActivated = !laserActivated;
            if (laserActivated)
                StartLaser();
            else
                StopLaser();
        }
    }

    private void LaserShooting()
    {
        RaycastHit laserRay = ShotRay();
        if (laserRay.collider)
        {
            
            _laserLine.SetPosition(1, Vector3.forward * (_laserOrigin.position - laserRay.point).magnitude);
            Vector3 burnPoint = laserRay.point + laserRay.normal * _burnNormalOffset;
            laserDot.SetPositionAndRotation(burnPoint, Quaternion.LookRotation(-laserRay.normal));
            _laserDotMaterial.color = _laserDotShootingActive;
            smokeParticle.transform.position = laserRay.point;
            if (isShooting)
            {
                GameObject burn = Instantiate(laserBurnDecal, burnPoint, Quaternion.LookRotation(-laserRay.normal));
                SkinBurn skinBurn = burn.GetComponent<SkinBurn>();
                skinBurn.Init();
                skinBurn.transform.SetParent(laserRay.collider.transform);
                laserBurns.Add(burn);
                ProcessBurnDistance(burnPoint, _laserBegin.position);

                FetoscopyOrgan organ = laserRay.collider.gameObject.GetComponent<FetoscopyOrgan>();
                if (organ)
                {
                    if (organ != currentOrgan)
                    {
                        if(currentOrgan)
                            currentOrgan.StopDamage();
                        currentOrgan = organ;
                    }
                    organ.Damage(laserRay.point);
                }
                else if(organ == null)
                {
                    if (currentOrgan)
                        currentOrgan.StopDamage();
                    currentOrgan = null;
                }
            }
        }
    }

    private void ProcessBurnDistance(Vector3 burnPoint, Vector3 laserPoint)
    {
        var distance = Vector3.Distance(burnPoint, laserPoint);
        if(distance <= _tresholdDistance)
        {
            if(!inBleedAnimation)
            {
                StartCoroutine(ProcessBurnsAnimation());
            }
        }
    }

    private IEnumerator ProcessBurnsAnimation() 
    {
        inBleedAnimation = true;
        float time = 0f;
        float alpha = 0f;
        float factorChange = 1;
        TvBorder.color = new Color(TvBorder.color.r, TvBorder.color.g, TvBorder.color.b, alpha);
        ScreenBorder.color = new Color(ScreenBorder.color.r, ScreenBorder.color.g, ScreenBorder.color.b, alpha);
        while (time <= _animationDuration) 
        {
            time += Time.deltaTime;
            yield return null;

            alpha += Time.deltaTime * _bleedFadeSpeed * factorChange;

            TvBorder.color = new Color(TvBorder.color.r, TvBorder.color.g, TvBorder.color.b, Mathf.Clamp(alpha, 0f, 1f));
            ScreenBorder.color = new Color(ScreenBorder.color.r, ScreenBorder.color.g, ScreenBorder.color.b, Mathf.Clamp(alpha, 0f, 1f));

            if (alpha > 1f) { factorChange = -1; }
            if(alpha < 0f) { factorChange = +1; }
        }
        inBleedAnimation = false;
    }

    private void LaserActiveIdle()
    {
        RaycastHit laserRay = ShotRay();
        if (laserRay.collider)
        {

            _laserLine.SetPosition(1, Vector3.forward * (_laserOrigin.position - laserRay.point).magnitude);
            laserDot.SetPositionAndRotation(laserRay.point + laserRay.normal * _burnNormalOffset, Quaternion.LookRotation(-laserRay.normal));
            _laserDotMaterial.color = _laserDotIdleActive;
        }
    }

    private void EnableLaserShootingVFX()
    {
        isShooting = true;
        smokeParticle.Play();
        float volume = 0;
        _laserAudio.volume = volume;
        _laserAudio.Play();
        DOTween.To(() => volume, x => volume = x, 1, 0.5f)
            .OnUpdate(() => { _laserAudio.volume = volume; });
    }

    private void DisableLaserShootingVFX()
    {
        isShooting = false;
        smokeParticle.Stop();
        if (currentOrgan)
            currentOrgan.StopDamage();
        float volume = 1;
        DOTween.To(() => volume, x => volume = x, 0, 0.5f)
            .OnUpdate(() => { _laserAudio.volume = volume; })
            .OnComplete(() => { _laserAudio.Stop(); });
        
    }

    private RaycastHit ShotRay()
    {
        Ray ray = new Ray(_laserOrigin.position, _laserOrigin.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, _laserMaxDistance, _surgeryMask);
        return hit;
    }

    public void ClearLaserBurns()
    {
        for (int i = 0; i < laserBurns.Count; i++)
        {
            Destroy(laserBurns[i].gameObject);
        }
        laserBurns.Clear();
    }

    public void GrandControlPermissions(HandSides side)
    {
        currentSide = side;
        if (side == HandSides.LeftHand)
        {
            _toggleLaserLeftInput.action.performed +=  ActivateLaserAction;
        }
        else
        {
            _toggleLaserRightInput.action.performed += ActivateLaserAction;
        }
        PlayerHandsManager.Instance.SetLocomotion(false);
        controlPermission = true;
    }

    public void RemoveControlPermissions(HandSides side)
    {
        if (side == HandSides.LeftHand)
        {
            _toggleLaserLeftInput.action.performed -= ActivateLaserAction;
        }
        else
        {
            _toggleLaserRightInput.action.performed -= ActivateLaserAction;
        }
        PlayerHandsManager.Instance.SetLocomotion(true);
        StopLaser();
        controlPermission = false;
    }

    private float GetHandGrabInput()
    {
        return currentSide == HandSides.LeftHand ?
            _shotLaserLeftInput.action.ReadValue<float>() :
            _shotLaserRightInput.action.ReadValue<float>();
    }

    private void ChangeLaserHeadMaterial(float alpha, float smoothness)
    {
        _laserHeadMaterial.color = new Color(_laserHeadMaterial.color.r,
            _laserHeadMaterial.color.g, _laserHeadMaterial.color.b, alpha);
        _laserHeadMaterial.SetFloat("_Glossiness", smoothness);
    }

    public Camera Camera { get => _camera; set => _camera = value; }
}
