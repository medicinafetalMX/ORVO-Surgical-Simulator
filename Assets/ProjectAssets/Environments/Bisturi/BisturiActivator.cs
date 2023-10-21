using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BisturiActivator : MonoBehaviour
{
    [SerializeField] Transform _bisturiLever;
    [SerializeField] GameObject _bisturi;
    [SerializeField] BisturiLeverController _bisturiLevelController;
    private bool alreadyActivated = false;
    private void Start()
    {
        _bisturiLevelController.OnCutFinished += EndCut;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyActivated && other.gameObject == _bisturi)
        {
            _bisturi.SetActive(false);
            _bisturiLever.transform.LookAt(transform.position);
            _bisturiLever.RotateAround(_bisturiLever.position, _bisturiLever.right, 90f);
            _bisturiLever.RotateAround(_bisturiLever.position, _bisturiLever.up, 180f);
            _bisturiLevelController.CalculateRadius(transform.position);
            _bisturiLever.gameObject.SetActive(true);
            alreadyActivated = true;
        }
    }

    private void EndCut()
    {
        StartCoroutine(WaitForEndCut());
    }

    private IEnumerator WaitForEndCut()
    {
        yield return new WaitForSeconds(2);
        _bisturi.transform.position = _bisturiLevelController.Bisturi.position;
        _bisturi.transform.rotation = _bisturiLevelController.Bisturi.rotation;
        _bisturiLevelController.Bisturi.gameObject.SetActive(false);
        _bisturi.SetActive(true);
    }
}
