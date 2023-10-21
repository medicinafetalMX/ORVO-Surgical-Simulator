using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LaparoscopeScreenRotator : MonoBehaviour
{
    [SerializeField] Transform _laparoscopeScreen;
    [SerializeField] Transform _laparoscopeTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _laparoscopeScreen.DOMove(_laparoscopeTarget.position, 2);
            _laparoscopeScreen.DORotate(_laparoscopeTarget.rotation.eulerAngles, 2);
        }
    }
}
