using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNameAdvice : MonoBehaviour
{
    [SerializeField] string _message;
    [SerializeField] GameObject _advicePrefab;
    [SerializeField] Transform _specificAdvicePosition;
    [SerializeField] Vector3 _offset;

    private ObjectUIPopUp popUp;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + _offset, 0.05f);
    }

    private void Start()
    {
        if (!_specificAdvicePosition)
        {
            _specificAdvicePosition = new GameObject("advicePoint").transform;
            _specificAdvicePosition.parent = transform;
            _specificAdvicePosition.localPosition = _offset;
        }
        GameObject popUpObject = Instantiate(_advicePrefab, transform);
        popUpObject.transform.position = _specificAdvicePosition.position;
        popUp = popUpObject.GetComponent<ObjectUIPopUp>();
    }
}
