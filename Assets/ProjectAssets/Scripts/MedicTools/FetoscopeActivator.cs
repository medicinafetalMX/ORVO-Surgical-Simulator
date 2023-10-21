using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FetoscopeActivator : MonoBehaviour
{
    public UnityAction OnFetoscopeActivated;

    [SerializeField] GameObject _laparoscopeSurgery;
    [SerializeField] FetoscopyController _laparoscopeController;
    [SerializeField] Transform _center;

    private FetoscopyIndicatorScreen _indicatorScreen;

    private IEnumerator Start()
    {
        _indicatorScreen = FindObjectOfType<FetoscopyIndicatorScreen>();
        yield return new WaitForEndOfFrame();
        _laparoscopeSurgery.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StabActivator>())
        {
            OnFetoscopeActivated?.Invoke();
            _laparoscopeSurgery.transform.position = other.transform.position;
            //Vector3 up = (transform.position - _center.position).normalized;
            //Vector3 fwd = Vector3.Cross(up, Vector3.right);
            _laparoscopeSurgery.transform.rotation = Quaternion.LookRotation(other.transform.forward, other.transform.up);
            _laparoscopeSurgery.SetActive(true);
            _indicatorScreen.SetFetoscopyCamera(_laparoscopeController.Camera);
        }
    }
}
