using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class SurgeryPlaceSelector : GrabbableObjectChecker
{
    public UnityAction OnSurgeryPlaceSelected;

    [SerializeField] LineRenderer _line;
    [SerializeField] Transform _cross;
    [SerializeField] Transform _masterTransform;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] InputActionReference _placeRightInput;
    [SerializeField] InputActionReference _placeLeftInput;
    [Header("Raycasting")]
    [SerializeField] float _raycastTime = 0.1f;
    [SerializeField] LayerMask _rayMask;

    private bool allowedToPlace = false;
    private float raycastTimer = 0;
    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        AddHandListeners();
        _line.enabled = false;
        _cross.gameObject.SetActive(false);
    }

    public override void CheckGrab(GameObject gameObject, HandSides side)
    {
        base.CheckGrab(gameObject, side);
        if (isGrabbed)
        {
            if (isLeftGrabbing)
                _placeLeftInput.action.performed += Place;
            if(isRightGrabbing)
                _placeRightInput.action.performed += Place;
            _line.enabled = true;
        }
    }

    public override void CheckRelease(GameObject gameObject, HandSides side)
    {
        base.CheckRelease(gameObject, side);
        if (gameObject == this.gameObject)
        {
            if (!isLeftGrabbing)
                _placeLeftInput.action.performed -= Place;
            if (!isRightGrabbing)
                _placeRightInput.action.performed -= Place;
            if (!isGrabbed)
            {
                _line.enabled = false;
                _cross.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isGrabbed)
        {
            raycastTimer += Time.deltaTime;
            if(raycastTimer >= _raycastTime)
            {
                raycastTimer = 0;
                ShotRaycast();
            }
        }
    }
    private void ShotRaycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(_rayOrigin.position, -transform.up);
        if (Physics.Raycast(ray, out hit,_rayMask))
        {
            allowedToPlace = true;
            _cross.gameObject.SetActive(true);
            _cross.position = hit.point + hit.normal * 0.001f;
            _cross.rotation = Quaternion.LookRotation(-hit.normal);
            _line.SetPosition(1,Vector3.down * hit.distance + Vector3.up * 0.05f);
        }
        else
        {
            allowedToPlace = false;
            _cross.gameObject.SetActive(false);
            _line.SetPosition(1,Vector3.down);
        }
    }
    private void Place(InputAction.CallbackContext obj)
    {
        if (allowedToPlace)
        {
            OnSurgeryPlaceSelected?.Invoke();
            audio.Play();
            _masterTransform.position = _cross.transform.position + _cross.forward * 0.001f;
            _masterTransform.rotation = Quaternion.LookRotation(-Vector3.Cross(-_cross.forward, Vector3.right), -_cross.forward);
        }
    }
}
