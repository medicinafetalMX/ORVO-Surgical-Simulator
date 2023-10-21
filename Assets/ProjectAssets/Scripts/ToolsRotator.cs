using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsRotator : MonoBehaviour
{
    [SerializeField] Transform _toolsTray;
    [SerializeField] Transform _positionTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _toolsTray.DOMove(_positionTarget.position, 2);
            _toolsTray.DORotate(_positionTarget.rotation.eulerAngles, 2);
        }
    }
}
